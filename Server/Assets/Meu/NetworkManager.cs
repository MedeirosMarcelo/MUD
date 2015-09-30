using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    Chat chat;
    ServerManager serverManager;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        serverManager = GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>();
    }

    [RPC]
    public void TellServerOurName(string name, NetworkMessageInfo info) {
        Player newEntry = new Player(name, info.sender);
        serverManager.playerList.Add(newEntry);
        chat.addGameChatMessage(name + " joined the chat");
    }
}