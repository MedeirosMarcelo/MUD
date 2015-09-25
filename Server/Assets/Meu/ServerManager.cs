using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNode {
    public string playerName;
    public NetworkPlayer networkPlayer;
}

public class ServerManager : MonoBehaviour {

    //Server-only playerlist
    public IList<PlayerNode> playerList = new List<PlayerNode>();
    Chat chat;
    string playerName;

	void Start () {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ReadCommand(string str) {
        CommandReader commandReader = new CommandReader();
        commandReader.CheckCommand(str);
    }

    //Server function
    void OnPlayerConnected(NetworkPlayer player) {
        chat.addGameChatMessage("Player connected from: " + player.ipAddress + ":" + player.port);
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
        PlayerNode newEntry = new PlayerNode();
        newEntry.playerName = playerName;
        newEntry.networkPlayer = Network.player;
        playerList.Add(newEntry);
        chat.addGameChatMessage(playerName + " joined the chat");
    }

    void RectifyUserName() {
        playerName = PlayerPrefs.GetString("playerName", "");
        if (playerName == "" || playerName == "UserName") {
            playerName = "Admin";
        }
    }

    PlayerNode GetPlayerNode(NetworkPlayer networkPlayer) {
        foreach (PlayerNode entry in playerList) {
            if (entry.networkPlayer == networkPlayer) {
                return entry;
            }
        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }
}