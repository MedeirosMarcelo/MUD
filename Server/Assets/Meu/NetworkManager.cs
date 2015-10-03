using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    Chat chat;
    ServerManager serverManager;
    CommandReader commandReader;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        serverManager = GameObject.FindWithTag("ServerManager").GetComponent<ServerManager>();
        commandReader = serverManager.GetComponent<CommandReader>();
    }

    [RPC]
    void TellServerOurName(string name, NetworkMessageInfo info) {
        Player player = new Player(name, info.sender);
        serverManager.playerList.Add(player);
        chat.addGameChatMessage(name + " joined the chat");
    }

    [RPC]
    public void SendChatEntry(string msg) {
        commandReader.Read(msg);
    }

    [RPC]
    void RectifyName(string name, NetworkMessageInfo info) {
        Player player = new Player(name, info.sender);
        serverManager.RectifyUserName(player);
    }
}