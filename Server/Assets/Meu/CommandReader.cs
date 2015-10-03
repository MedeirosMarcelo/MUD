using UnityEngine;
using System.Collections;
using System;

public class CommandReader : MonoBehaviour {

    Chat chat;
    string tempCommand;

    void Start() {
        chat = GameObject.FindWithTag("Chat").GetComponent<Chat>();
    }

    public void Read(string str) {
        tempCommand = str;
        CheckCommand(GetParams(str));
    }

    void CheckCommand(string[] command) {
        switch (command[0]) {
            case "whisper":
                Test(command);
                break;
            default:
                Debug.Log("WRONG COMMAND: " + command);
                break;
        }
    }

    string[] GetParams(string line) {
        string[] str = line.ToLower().Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < str.Length; i++) {
            Debug.Log("TEST " + str[i]);
        }
        return str;
    }

    void Test(string[] str) {
        chat.networkView.RPC("ApplyGlobalChatText", RPCMode.All, "CommandTest", tempCommand);
    }
}
