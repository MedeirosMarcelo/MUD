using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chat : MonoBehaviour {

    public bool usingChat = false;	//Can be used to determine if we need to stop player movement since we're chatting
    public GUISkin skin;						//Skin
    public bool showChat = false;			//Show/Hide the chat
    public string playerName;

    private string inputField = "";
    private Vector2 scrollPosition;
    private int width = 500;
    private int height = 500;
    private int maxLines = 15;
    private float lastUnfocusTime = 0;
    private Rect window;

    private ArrayList chatEntries = new ArrayList();
    class ChatEntry {
        public string name = "";
        public string text = "";
    }

    void Awake() {
        window = new Rect(Screen.width / 2 - width / 2, Screen.height - height + 5, width, height);
    }

    void OnDisconnectedFromServer() {
        CloseChatWindow();
    }

    void HitEnter(string msg) {
        msg = msg.Replace("\n", "");
        networkView.RPC("ApplyGlobalChatText", RPCMode.All, playerName, msg);
        inputField = ""; //Clear line
        //GUI.UnfocusWindow();//Deselect chat
        //lastUnfocusTime = Time.time;
        usingChat = false;
    }

    public void addGameChatMessage(string str) {
        ApplyGlobalChatText("", str);
        if (Network.connections.Length > 0) {
            networkView.RPC("ApplyGlobalChatText", RPCMode.Others, "", str);
        }
    }

    [RPC]
    void ApplyGlobalChatText(string name, string msg) {
        ChatEntry entry = new ChatEntry();
        entry.name = name;
        entry.text = msg;

        chatEntries.Add(entry);

        //Remove old entries
        if (chatEntries.Count > maxLines) {
            chatEntries.RemoveAt(0);
        }

        scrollPosition.y = 1000000;
    }

    public void CloseChatWindow() {
        showChat = false;
        inputField = "";
        chatEntries = new ArrayList();
    }

    public void ShowChatWindow() {
        showChat = true;
        inputField = "";
        chatEntries = new ArrayList();
    }

    void OnGUI() {
        if (!showChat) {
            return;
        }

        GUI.skin = skin;

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length <= 0) {
            if (lastUnfocusTime + 0.25 < Time.time) {
                usingChat = true;
                GUI.FocusWindow(5);
                GUI.FocusControl("Chat input field");
            }
        }
        window = GUI.Window(5, window, GlobalChatWindow, "");
    }

    void GlobalChatWindow(int id) {

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        // Begin a scroll view. All rects are calculated automatically - 
        // it will use up any available screen space and make sure contents flow correctly.
        // This is kept small with the last two parameters to force scrollbars to appear.
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (ChatEntry entry in chatEntries) {
            GUILayout.BeginHorizontal();
            if (entry.name == "") {//Game message
                GUILayout.Label(entry.text);
            }
            else {
                GUILayout.Label(entry.name + ": " + entry.text);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

        }
        // End the scrollview we began above.
        GUILayout.EndScrollView();

        if (Event.current.type == EventType.keyDown && Event.current.character == '\n' && inputField.Length > 0) {
            HitEnter(inputField);
        }
        GUI.SetNextControlName("Chat input field");
        inputField = GUILayout.TextField(inputField);


        if (Input.GetKeyDown("mouse 0")) {
            if (usingChat) {
                usingChat = false;
                GUI.UnfocusWindow();//Deselect chat
                lastUnfocusTime = Time.time;
            }
        }
    }
}
