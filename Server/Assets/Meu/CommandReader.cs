using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommandReader : MonoBehaviour {

    Chat chat;
    ServerManager serverManager;
    bool enabled = true;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        serverManager = GetComponent<ServerManager>();
    }

    public void Read(string text, Player player) {
        if (enabled) {
            CheckCommand(text, player);
        }
    }

    void CheckCommand(string text, Player player) {
        string commandName = GetParams(text)[0].ToLower();
        switch (commandName) {
            case "m":
            case "move":
                Move(GetParams(text), player);
                break;
            case "s":
            case "say":
                Say(GetSayParams(text), player);
                break;
            case "w":
            case "whisper":
                Whisper(GetWhisperParams(text), player);
                break;
            case "ex":
            case "examine":
                Examine(GetParams(text), player);
                break;
            case "p":
            case "pickup":
                PickUp(GetParams(text), player);
                break;
            case "u":
            case "use":
                Use(GetParams(text), player);
                break;
            case "d":
            case "drop":
                Drop(GetParams(text), player);
                break;
            case "search":
                Search(GetParams(text), player);
                break;
            case "map":
                Map(GetParams(text), player);
                break;
            case "i":
            case "inventory":
                Inventory(GetParams(text), player);
                break;
            case "help":
                Help(GetParams(text), player);
                break;
            default:
                Debug.Log("WRONG COMMAND: " + text);
                string name = "Server: ";
                string error = "Invalid Command";
                ChatToPlayerOrServer(player, name, error);
                break;
        }
    }

    void EndGame(Player winner) {
        enabled = false;
        chat.networkView.RPC("ApplyGlobalChatText", RPCMode.All, "", "Game Over! " + winner.name + " wins!");
    }

    string[] GetParams(string text) {
        string[] str = text.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < str.Length; i++) {
            Debug.Log("GetParams " + str[i]);
        }
        return str;
    }

    string[] GetSayParams(string text) {
        int index = text.IndexOf(" ");
        string speechOnly = text.Substring(index);
        string commandOnly = text.Substring(0, index);
        string[] str = new string[2] { commandOnly, speechOnly };
        for (int i = 0; i < str.Length; i++) {
            Debug.Log("GetParams " + str[i]);
        }
        return str;
    }

    string[] GetWhisperParams(string text) {
        int indexName = text.IndexOf(" ");
        string commandOnly = text.Substring(0, indexName);

        string nameOnly = text.Substring(indexName);
        string[] strTemp = nameOnly.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
        string newNameOnly = strTemp[0];

        string speechOnly = text.Substring(commandOnly.Length + newNameOnly.Length + 2);

        string[] str = new string[3] { commandOnly, newNameOnly, speechOnly };
        for (int i = 0; i < str.Length; i++) {
            Debug.Log("GetParams " + str[i]);
        }
        return str;
    }

    void Move(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            string direction = command[1].ToLower();
            bool moved = false;
            Room oldRoom;
            string moveResult = actingPlayer.Move(direction, out moved, out oldRoom);
            ChatToPlayerOrServer(actingPlayer, "", moveResult);
            if (moved) {
                NoticeToOthers(actingPlayer, actingPlayer.name + " went through the " + GetDirection(command[1]) + " Door", oldRoom);
                string[] exam = new string[2] { "examine", "room" };
                Examine(exam, actingPlayer);
            }
            Player winner;
            if (serverManager.CheckGameOver(out winner)) {
                EndGame(winner);
                Debug.Log("WIN");
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Say(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            string text = command[1];
            foreach (Player pl in serverManager.playerList) {
                if (pl.room == actingPlayer.room) {
                    ChatToPlayerOrServer(pl, actingPlayer.name, text);
                }
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Whisper(string[] command, Player actingPlayer) {
        if (command.Length == 3) {
            string playerName = command[1];
            string text = command[2];
            Player target = serverManager.GetPlayerNode(playerName);
            if (target != null) {
                if (actingPlayer.room == target.room) {
                    playerName = "whisper to [" + command[1] + "]";
                    ChatToPlayerOrServer(actingPlayer, playerName, text);
                    playerName = "[" + actingPlayer.name + "] whispers";
                    ChatToPlayerOrServer(target, playerName, text);
                }
                else {
                    playerName = "Server: ";
                    text = "There is no one with the name " + command[1] + " in the room you are in.";
                    ChatToPlayerOrServer(actingPlayer, playerName, text);
                }
            }
            else {
                playerName = "Server: ";
                text = "There is no one with the name " + command[1] + " in the room you are in.";
                ChatToPlayerOrServer(actingPlayer, playerName, text);
            }
        }
        else {
            ChatToPlayerOrServer(actingPlayer, "Server", "Invalid Command");
        }
    }

    void Examine(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            if (command[1].ToLower() == "room") {
                ChatToPlayerOrServer(actingPlayer, "", actingPlayer.room.description);
                string examineResult = "You see: ";
                bool foundSomething = false;
                foreach (MudObject obj in actingPlayer.room.mudObjectList) {
                    if (obj != actingPlayer) {
                        examineResult += "\n" + obj.description;
                        foundSomething = true;
                    }
                }
                if (!foundSomething) examineResult = "You can't see shit captain.";
                ChatToPlayerOrServer(actingPlayer, "", examineResult);
            }
            else {
                string objName = command[1];
                MudObject item = actingPlayer.room.GetItem(objName);
                if (item != null) {
                    ChatToPlayerOrServer(actingPlayer, "", item.description);
                }
                else {
                    ChatToPlayerOrServer(actingPlayer, "", "There is no such thing to examine.");
                }
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void PickUp(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            if (actingPlayer.Pickup(command[1])) {
                ChatToPlayerOrServer(actingPlayer, "", "You put " + command[1] + " in your inventory.");
                NoticeToOthers(actingPlayer, actingPlayer.name + " picked up the " + command[1] + ".");
            }
            else {
                ChatToPlayerOrServer(actingPlayer, "", "There is no such thing in the room.");
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Drop(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            if (actingPlayer.Drop(command[1])) {
                ChatToPlayerOrServer(actingPlayer, "", "You dropped " + command[1] + " on the floor.");
                NoticeToOthers(actingPlayer, actingPlayer.name + " dropped " + command[1] + " on the floor.");
              //  chat.networkView.RPC("ApplyGlobalChatText", RPCMode.Others, actingPlayer.name + " dropped " + command[1] + " on the floor");
            }
            else {
                ChatToPlayerOrServer(actingPlayer, "", "There is no such thing in your inventory.");
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Search(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            string result = "";
            if (command[1].ToLower() == "room") {
                IList<Item> itemList = actingPlayer.room.Search();
                if (itemList.Count > 0) {
                    foreach (Item item in itemList) {
                        result += "You find " + item.name + " ";
                    }
                }
                else {
                    result = "You find nothing.";
                }
            }
            ChatToPlayerOrServer(actingPlayer, "", result);
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Use(string[] command, Player actingPlayer) {
        if (command.Length == 3) {
            string itemName = command[1];
            string targetName = command[2];
            Item item = actingPlayer.GetItem(itemName);
            //  MudObject target = actingPlayer.room.GetMudObject(targetName);
            Door target = actingPlayer.room.GetMudObject(targetName) as Door;
            if (item != null && target != null) {
                if (target.Usable == item) {
                    item.Use(target);
                    ChatToPlayerOrServer(actingPlayer, "", "You unlock the door.");
                    NoticeToOthers(actingPlayer, actingPlayer.name + " unlocked the door with the " + command[1] + ".");
                }
                else {
                    Debug.Log("111");
                    ChatToPlayerOrServer(actingPlayer, "", "You can't use it like this.");
                }
            }
            else {
                Debug.Log("222");
                ChatToPlayerOrServer(actingPlayer, "", "You can't use it like this.");
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Inventory(string[] command, Player actingPlayer) {
        if (command.Length == 1) {
            string itemList = actingPlayer.GetInventoryList();
            ChatToPlayerOrServer(actingPlayer, "", "Inventory list: " + itemList);
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Map(string[] command, Player actingPlayer) {
        if (command.Length == 1) {
            ChatToPlayerOrServer(actingPlayer, "", GameManager.UpdateMapPosition(actingPlayer));
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Help(string[] command, Player actingPlayer) {
        if (command.Length == 1) {
            string helpText = "Available Commands: \n\nExamine [room/object] \nMove [North/South/East/West] \nPickup[Object] \nDrop [Object] \nInventory \nUse [Object] {Target} \nSay [Text] \nWhisper [Player] [Text] \nHelp";
            ChatToPlayerOrServer(actingPlayer, "", helpText);
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void ChatToPlayerOrServer(Player player, string playerName, string text) {
        if (!player.isServer) {
            chat.networkView.RPC("ApplyGlobalChatText", player.networkPlayer, playerName, text);
        }
        else {
            chat.ApplyGlobalChatText(playerName, text);
        }
    }

    void NoticeToOthers(Player player, string text, Room room) {
        foreach (Player pl in serverManager.playerList) {
            if (pl != player && pl.room == room) {
                ChatToPlayerOrServer(pl, "", text);
            }
        }
    }

    void NoticeToOthers(Player player, string text) {
        foreach (Player pl in serverManager.playerList) {
            if (pl != player && pl.room == player.room) {
                ChatToPlayerOrServer(pl, "", text);
            }
        }
    }

    void ShowWrongCommand(Player actingPlayer) {
        ChatToPlayerOrServer(actingPlayer, "", "WRONG COMMAND");
    }

    string GetDirection(string s) {
        if (s == "n") return "Northern";
        if (s == "s") return "Southern";
        if (s == "e") return "Eastern";
        if (s == "w") return "Western";
        else return "";
    }
}

/*
    +Examinar [sala/objeto]
    +Mover [N/S/L/O]
    +Pegar[objeto]
    +Largar [objeto]
    +Cochichar [jogador] [texto]
    +Falar [texto]
Usar [objeto] {alvo}
    +Inventário
    +Mapa
    +Ajuda
*/