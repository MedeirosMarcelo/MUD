using UnityEngine;
using System.Collections;

public class CommandReader : MonoBehaviour {

    Chat chat;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    public void Read(string str) {
        CheckCommand(GetCommand(str));
    }

    void CheckCommand(string command) {
        switch (command) {
            case "Test":
                Test(command);
                break;
            default:
                Debug.Log("WRONG COMMAND: " + command);
                break;
        }
    }

    string GetCommand(string str) {
        return "Test";
    }

    void Test(string str) {
        Debug.Log(str);
        chat.networkView.RPC("ApplyGlobalChatText", RPCMode.All, "Server", str);
    }
}
