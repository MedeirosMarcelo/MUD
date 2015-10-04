using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

    public string userName;
    Chat chat;
    NetworkManager networkManager;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
        networkManager = chat.GetComponent<NetworkManager>();
    }

    void OnConnectedToServer() {
        userName = PlayerPrefs.GetString("playerName");        
        chat.ShowChatWindow();
        networkManager.networkView.RPC("InitializePlayer", RPCMode.Server, userName);
    }
}