using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerManager : MonoBehaviour {

    public IList<Player> playerList = new List<Player>();
    Chat chat;
    string playerName;

	void Start () {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
	}

    void ReadCommand(string str) {
        CommandReader commandReader = new CommandReader();
        commandReader.CheckCommand(str);
    }

    //Server function
    void OnPlayerConnected(NetworkPlayer player) {
        chat.addGameChatMessage("Player connected from: " + player.ipAddress + ":" + player.port);
        chat.addGameChatMessage(GetComponent<GameManager>().map);
    }
    //Server function
    void OnPlayerDisconnected(NetworkPlayer player) {
        chat.addGameChatMessage("Player disconnected from: " + player.ipAddress + ":" + player.port);
        //Remove player from the server list
        playerList.Remove(GetPlayerNode(player));
    }

    //Server function
    void OnServerInitialized() {
        RectifyUserName();
        chat.ShowChatWindow();
        Player newEntry = new Player(playerName, Network.player);
        playerList.Add(newEntry);
        chat.addGameChatMessage(playerName + " joined the chat");
    }

    void RectifyUserName() {
        playerName = PlayerPrefs.GetString("playerName", "");
        if (playerName == "" || playerName == "UserName") {
            playerName = "Admin";
        }
        chat.playerName = playerName;
    }

    Player GetPlayerNode(NetworkPlayer networkPlayer) {
        foreach (Player entry in playerList) {
            if (entry.networkPlayer == networkPlayer) {
                return entry;
            }
        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }
}