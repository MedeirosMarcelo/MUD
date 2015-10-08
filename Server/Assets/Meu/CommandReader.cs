using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CommandReader : MonoBehaviour {

    Chat chat;
    ServerManager serverManager;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        serverManager = GetComponent<ServerManager>();
    }

    public void Read(string text, Player player) {
        CheckCommand(GetParams(text), player);
    }

    void CheckCommand(string[] command, Player player) {
        string commandName = command[0].ToLower();
        switch (commandName) {
            case "m":
            case "move":
                Move(command, player);
                break;
            case "s":
            case "say":
                Say(command, player);
                break;
            case "w":
            case "whisper":
                Whisper(command, player);
                break;
            case "ex":
            case "examine":
                Examine(command, player);
                break;
            case "pickup":
                PickUp(command, player);
                break;
            case "drop":
                Drop(command, player);
                break;
            case "search":
                Search(command, player);
                break;
            case "map":
                Map(command, player);
                break;
            case "inventory":
                Inventory(command, player);
                break;
            case "help":
                Help(command, player);
                break;
            default:
                Debug.Log("WRONG COMMAND: " + command);
                string name = "Server: ";
                string text = "Invalid Command";
                ChatToPlayerOrServer(player, name, text);
                break;
        }
    }

    string[] GetParams(string text) {
        string[] str = text.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < str.Length; i++) {
            Debug.Log("GetParams " + str[i]);
        }
        return str;
    }

    void Move(string[] command, Player actingPlayer) {
        if (command.Length == 2) {
            string direction = command[1].ToLower();
            if (actingPlayer.Move(direction)) {
                ChatToPlayerOrServer(actingPlayer, "", command[0] + " " + command[1]);
                ChatToPlayerOrServer(actingPlayer, "", GameManager.UpdateMapPosition(actingPlayer));
            }
            else {
                ChatToPlayerOrServer(actingPlayer, "", "You can't move that way.");
                ChatToPlayerOrServer(actingPlayer, "", GameManager.UpdateMapPosition(actingPlayer));
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
                playerName = "whisper to [" + command[1] + "]";
                ChatToPlayerOrServer(actingPlayer, playerName, text);
                playerName = "[" + actingPlayer.name + "] whispers";
                ChatToPlayerOrServer(target, playerName, text);
            }
            else {
                playerName = "Server: ";
                text = "Player " + command[1] + " Doesn't Exist!";
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
                ChatToPlayerOrServer(actingPlayer, "", actingPlayer.room.name);
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
                ChatToPlayerOrServer(actingPlayer, "", "You put " + command[1] + " in your inventory");
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
                ChatToPlayerOrServer(actingPlayer, "", "You dropped " + command[1] + " on the floor");
                chat.networkView.RPC("ApplyGlobalChatText", RPCMode.Others, actingPlayer.name + " dropped " + command[1] + " on the floor");
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
            if (command[1].ToLower() == "room") {
                IList<Item> itemList = actingPlayer.room.Search();
                string listItems = "";
                if (itemList != null) {
                    foreach (Item item in itemList) {
                        listItems += "You find " + item.name + " ";
                    }
                }
                ChatToPlayerOrServer(actingPlayer, "", listItems);
            }
            else {
                ChatToPlayerOrServer(actingPlayer, "", "You find nothing.");
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
    }

    void Use(string[] command, Player actingPlayer) {
        if (command.Length == 3) {
            if (actingPlayer.room.GetItem(command[1]) != null) {
                string objName = command[2];
                MudObject item = actingPlayer.room.GetItem(objName);
            }
        }
        else {
            ShowWrongCommand(actingPlayer);
        }
        if (command.Length == 3) {
            if (command[1].ToLower() == "room") {
                ChatToPlayerOrServer(actingPlayer, "", actingPlayer.room.name);
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

    void ShowWrongCommand(Player actingPlayer) {
        ChatToPlayerOrServer(actingPlayer, "", "WRONG COMMAND");
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