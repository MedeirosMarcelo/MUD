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
        RectifyUserName();
        chat.ShowChatWindow();
        networkManager.networkView.RPC("TellServerOurName", RPCMode.Server, userName, Network.player);
    }

    void RectifyUserName() {
        userName = PlayerPrefs.GetString("playerName", "");
        if (userName == "" || userName == "UserName") {
            userName = "RandomName" + Random.Range(1, 999);
        }
        VerifyNameUniqueness();
    }


    void VerifyNameUniqueness() {
        networkManager.networkView.RPC("CheckUniqueName", RPCMode.Server, userName, Network.player);
    }
}
