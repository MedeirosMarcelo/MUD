using UnityEngine;
using System.Collections;

public class CommandReader : MonoBehaviour {

    string command;

    public void Read(string str) {
        CheckCommand(GetCommand(str));
        command = str;
    }

    string GetCommand(string str) {
        return "Test";
    }

    void CheckCommand(string command) {
        Debug.Log("");
        switch (command) {
            case "Test":
                Test();
                break;
            default:
                Debug.Log("WRONG COMMAND: " + command);
                break;
        }
    }

    void Test() {
        networkView.RPC("ApplyGlobalChatText", RPCMode.All, command);
    }
}
