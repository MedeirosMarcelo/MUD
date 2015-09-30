using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    Chat chat;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    [RPC]
    void TellServerOurName(string name, NetworkMessageInfo info) {
    }
}