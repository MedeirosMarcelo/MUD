using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public IList<Player> playerList = new List<Player>();
    Chat chat;
    string userName;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    void ReadCommand(string str) {
        CommandReader commandReader = new CommandReader();
        commandReader.Read(str);
    }

    void OnPlayerConnected(NetworkPlayer player) {
        chat.addGameChatMessage("Player connected from: " + player.ipAddress + ":" + player.port);
        chat.addGameChatMessage(GetComponent<GameManager>().map);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        chat.addGameChatMessage("Player disconnected from: " + player.ipAddress + ":" + player.port);
        playerList.Remove(GetPlayerNode(player));
    }

    void OnServerInitialized() {
        userName = PlayerPrefs.GetString("playerName");
        Player newPlayer = new Player(userName, Network.player);
        RectifyUserName(newPlayer);
        chat.ShowChatWindow();
        playerList.Add(newPlayer);
        chat.addGameChatMessage(userName + " joined the chat");
    }

    Player GetPlayerNode(NetworkPlayer networkPlayer) {
        foreach (Player player in playerList) {
            if (player.networkPlayer == networkPlayer) {
                return player;
            }
        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public void RectifyUserName(Player player) {
        bool hasSpace = player.name.Contains(" ");
        if (hasSpace) {
            Network.CloseConnection(player.networkPlayer, true);
        }
        else if (Network.peerType == NetworkPeerType.Client) {
            if (player.name == "" || player.name == "UserName") {
                player.name = "RandomName" + Random.Range(1, 999);
            }
            else if (CheckUniqueName(player)) {
                player.name = PlayerPrefs.GetString("playerName");
            }
        }
        else if (Network.peerType == NetworkPeerType.Server) {
            if (player.name == "" || player.name == "UserName") {
                player.name = "Server";
            }
        }
    }

    bool CheckUniqueName(Player player) {
        foreach (Player pl in playerList) {
            if (pl.name == player.name && pl != player) {
                Network.CloseConnection(player.networkPlayer, true);
                return false;
            }
        }
        return true;
    }
}