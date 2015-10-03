using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

    string userName;
    Chat chat;
    NetworkManager networkManager;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        networkManager = chat.GetComponent<NetworkManager>();
    }

    void OnConnectedToServer() {
        userName = PlayerPrefs.GetString("playerName");
        networkManager.networkView.RPC("RectifyName", RPCMode.Server, userName);
        chat.ShowChatWindow();
        networkManager.networkView.RPC("TellServerOurName", RPCMode.Server, userName);
    }
}