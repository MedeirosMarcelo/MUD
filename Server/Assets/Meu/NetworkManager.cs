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
    public void TellServerOurName(string name, NetworkMessageInfo info) {
        Player newEntry = new Player(name, info.sender);
        serverManager.playerList.Add(newEntry);
        chat.addGameChatMessage(name + " joined the chat");
    }

    [RPC]
    public void SendChatEntry(string msg) {
        commandReader.Read(msg);
    }

    [RPC]
    public void CheckUniqueName(string name, NetworkPlayer networkPlayer) {
        foreach (Player player in serverManager.playerList) {
            if (player.name == name) {
                Network.CloseConnection(networkPlayer, true);
            }
        }
    }
}