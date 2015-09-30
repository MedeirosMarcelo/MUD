using UnityEngine;
using System.Collections;

public class CommandReader {

    public void ReadScript(string str) {
        CheckCommand(str);
    }

    string GetCommand(string str) {
        return "";
    }

    public void CheckCommand(string str) {

        string command = GetCommand(str);
        Debug.Log("@" + command + ": ");

        switch (command) {
            case "wait":
                break;
            default:
                Debug.Log("WRONG COMMAND: " + command);
                break;
        }
    }
}
