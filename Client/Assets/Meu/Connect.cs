using UnityEngine;
using System.Collections;

public class Connect : MonoBehaviour {

    public string connectToIP = "127.0.0.1";
    public int connectPort = 25001;
    string userName = "UserName";
    bool serverRights = false;

    void OnGUI() {

        if (Network.peerType == NetworkPeerType.Disconnected) {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");
            userName = GUILayout.TextField(userName, GUILayout.MinWidth(100));
            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));
            connectPort = int.Parse(GUILayout.TextField(connectPort.ToString()));

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client")) {
                if (!userName.Contains(" ")) {
                    PlayerPrefs.SetString("playerName", userName);
                    //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                    //Ignore the NAT for now
                    //        Network.useNat = false;
                    Network.Connect(connectToIP, connectPort);
                }
                else {
                    GUILayout.Label("Spacing not alowed!");
                    Debug.Log("Spacing not alowed!");
                }
            }
            if (serverRights) {
                if (GUILayout.Button("Start Server")) {
                    if (!userName.Contains(" ")) {
                        PlayerPrefs.SetString("playerServerName", userName);
                        //Start a server for 32 clients using the "connectPort" given via the GUI
                        //Ignore the nat for now
                        Network.InitializeServer(32, connectPort, false);
                    }
                    else {
                        GUILayout.Label("Spacing not alowed!");
                        Debug.Log("Spacing not alowed!");
                    }
                }
            }
            GUILayout.EndVertical();
        }
        else {
            //We've got a connection!
            if (Network.peerType == NetworkPeerType.Connecting) {
                GUILayout.Label("Connection status: Connecting");

            }
            else if (Network.peerType == NetworkPeerType.Client) {

                GUILayout.Label("Connection status: Client!");
                GUILayout.Label("Ping to server: " + Network.GetAveragePing(Network.connections[0]));

            }
            else if (Network.peerType == NetworkPeerType.Server) {

                GUILayout.Label("Connection status: Server!");
                GUILayout.Label("Connections: " + Network.connections.Length);
                if (Network.connections.Length >= 1) {
                    GUILayout.Label("Ping to first player: " + Network.GetAveragePing(Network.connections[0]));
                }
            }

            if (GUILayout.Button("Disconnect")) {
                Network.Disconnect(200);
            }
        }
    }

    // NONE of the functions below is of any use in this demo, the code below is only used for demonstration.
    // First ensure you understand the code in the OnGUI() function above.

    //Client functions called by Unity
    void OnConnectedToServer() {
        Debug.Log("This CLIENT has connected to a server");
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("This SERVER OR CLIENT has disconnected from a server");
    }

    void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Could not connect to server: " + error);
    }


    //Server functions called by Unity
    void OnPlayerConnected(NetworkPlayer player) {
        PlayerPrefs.SetString("playerName", userName);
        Debug.Log("Player connected from: " + player.ipAddress + ":" + player.port);
    }

    void OnServerInitialized() {
        Debug.Log("Server initialized and ready");
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Debug.Log("Player disconnected from: " + player.ipAddress + ":" + player.port);
    }


    // OTHERS:
    // To have a full overview of all network functions called by unity
    // the next four have been added here too, but they can be ignored for now

    void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
        Debug.Log("Could not connect to master server: " + info);
    }

    void OnNetworkInstantiate(NetworkMessageInfo info) {
        Debug.Log("New object instantiated by " + info.sender);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        //Custom code here (your code!)
    }

    /* 
     The last networking functions that unity calls are the RPC functions.
     As we've added "OnSerializeNetworkView", you can't forget the RPC functions 
     that unity calls..however; those are up to you to implement.
 
     @RPC
     function MyRPCKillMessage(){
        //Looks like I have been killed!
        //Someone send an RPC resulting in this function call
     }
    */
}
