using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour {

    public string playerName;
    Chat chat;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    //Client function
    void OnConnectedToServer() {
        RectifyUserName();
        chat.ShowChatWindow();
        networkView.RPC("TellServerOurName", RPCMode.Server, playerName);
    }

    void RectifyUserName() {
        playerName = PlayerPrefs.GetString("playerName", "");
        if (playerName == "" || playerName == "UserName") {
            playerName = "RandomName" + Random.Range(1, 999);
        }
    }
}
