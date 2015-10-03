using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    [RPC]
    void TellServerOurName(string name) {
    }

    [RPC]
    public void SendChatEntry(string msg) {
    }

    [RPC]
    void RectifyName(string name, NetworkMessageInfo info) {
    }
}