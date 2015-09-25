using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    Chat chat;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    [RPC]
    void AddPlayerToServerList(string name, NetworkMessageInfo info) {
        TellServerOurName(name, info);
    }

    void TellServerOurName(string name, NetworkMessageInfo info) {
        PlayerNode newEntry = new PlayerNode();
        newEntry.playerName = name;
        newEntry.networkPlayer = info.sender;
        serverManager.playerList.Add(newEntry);
        chat.addGameChatMessage(name + " joined the chat");
    }
}