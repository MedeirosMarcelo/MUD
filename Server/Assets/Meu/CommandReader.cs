using UnityEngine;
using System.Collections;
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
            case "roomname":
                RoomExamine(command, player);
                break;
            case "ajuda":
                Help(player);
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
                ChatToPlayerOrServer(actingPlayer, "", GameManager.UpdateMapPosition(actingPlayer, 1));
            }
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
            ChatToPlayerOrServer(actingPlayer, "Server", "Invalid Command");
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

    void RoomExamine(string[] command, Player actingPlayer) {
        ChatToPlayerOrServer(actingPlayer, "", actingPlayer.room.name);
    }

    void Help(Player actingPlayer) {
        string helpText = "Examinar [sala/objeto] \nMover [N/S/L/O] \nPegar[objeto] \nLargar [objeto] \nInventário \nUsar [objeto] {alvo} \nFalar [texto] \nCochichar [jogador] [texto] \nAjuda";
        ChatToPlayerOrServer(actingPlayer, "", helpText);
    }

    void ChatToPlayerOrServer(Player player, string playerName, string text) {
        if (!player.isServer) {
            chat.networkView.RPC("ApplyGlobalChatText", player.networkPlayer, playerName, text);
        }
        else {
            chat.ApplyGlobalChatText(playerName, text);
        }
    }
}
