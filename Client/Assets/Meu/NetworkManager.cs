using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    [RPC]
    void TellServerOurName(string name, NetworkMessageInfo info) {
    }

    [RPC]
    public void SendChatEntry(string msg) {
    }

    [RPC]
    public void CheckUniqueName() {

    }
}