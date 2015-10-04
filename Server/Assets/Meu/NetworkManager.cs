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
    void InitializePlayer(string userName, NetworkMessageInfo info) {
        serverManager.InitializePlayer(userName, info.sender);
    }

    [RPC]
    public void SendChatEntry(string msg, NetworkMessageInfo info) {
        serverManager.ReadCommand(msg, serverManager.GetPlayerNode(info.sender));
    }

    public void ServerSendChatEntry(string msg) {
        Debug.Log("ServerSendChatEntry");
        serverManager.ReadCommand(msg, serverManager.playerServer);
    }
}