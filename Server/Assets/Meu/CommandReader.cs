using UnityEngine;
using System.Collections;

public class CommandReader : MonoBehaviour {

    public void ReadScript(string str) {

        CheckCommand(str);
    }

    string GetCommand(string str) {
        return "";
    }

    void CheckCommand(string str) {

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
