using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public IList<Player> playerList = new List<Player>();
    public Player playerServer;
    Chat chat;
    CommandReader commandReader;
    public string userName;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        commandReader = GetComponent<CommandReader>();
        GameManager.BuildLevel();
    }

    public void ReadCommand(string str, Player player) {
        commandReader.Read(str, player);
    }

    void OnPlayerConnected(NetworkPlayer player) {
   //     chat.addGameChatMessage("Player connected from: " + player.ipAddress + ":" + player.port);
   //     chat.addGameChatMessage(GetComponent<GameManager>().map);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        chat.addGameChatMessage(GetPlayerNode(player).name + " disconnected from: " + player.ipAddress + ":" + player.port);
        playerList.Remove(GetPlayerNode(player));
    }

    void OnServerInitialized() {
        userName = PlayerPrefs.GetString("playerServerName");
        userName = RectifyUserName(userName, Network.player);
        Player newPlayer = new Player(userName, "A douchy server admin.", GameManager.startingRoom, Network.player, null, Action.None);
        newPlayer.isServer = true;
        playerServer = newPlayer;
        chat.ShowChatWindow();
        playerList.Add(newPlayer);
        chat.addGameChatMessage(userName + " joined the chat.");
    }

    public void InitializePlayer(string userName, NetworkPlayer networkPlayer) {
        string newName = RectifyUserName(userName, networkPlayer);
        Player player = new Player(newName, "A wild nerd appears.", GameManager.startingRoom, networkPlayer, null, Action.None);
        playerList.Add(player);
        chat.addGameChatMessage(player.name + " joined the chat.");
    }

    public string RectifyUserName(string playerName, NetworkPlayer networkPlayer) {
        bool hasSpace = playerName.Contains(" ");
        if (hasSpace) {
            Network.CloseConnection(networkPlayer, true);
            return "";
        }
        if (playerName == "" || playerName == "UserName") {
            return "RandomName" + Random.Range(1, 999);
        }
        if (CheckUniqueName(playerName, networkPlayer)) {
            return playerName;
        }
        return "";
    }

    bool CheckUniqueName(string playerName, NetworkPlayer networkPlayer) {
        foreach (Player pl in playerList) {
            if (pl.name == playerName && pl.networkPlayer != networkPlayer) {
                Network.CloseConnection(networkPlayer, true);
                return false;
            }
        }
        return true;
    }

    public Player GetPlayerNode(NetworkPlayer networkPlayer) {
        foreach (Player player in playerList) {
            if (player.networkPlayer == networkPlayer) {
                return player;
            }
        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public Player GetPlayerNode(string name) {
        foreach (Player player in playerList) {
            if (player.name == name) {
                return player;
            }
        }
        Debug.Log("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    public bool CheckGameOver(out Player player){
        foreach (Player pl in playerList) {
            if (pl.room.name == "room 22") {
                player = pl;
                return true;
            }
        }
        player = null;
        return false;
    }
}