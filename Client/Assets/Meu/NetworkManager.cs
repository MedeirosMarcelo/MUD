using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    [RPC]
    void InitializePlayer(string userName, NetworkMessageInfo info) {
    }

    [RPC]
    public void SendChatEntry(string msg) {
    }

    [RPC]
    void RectifyName(string name, NetworkMessageInfo info) {
    }
}