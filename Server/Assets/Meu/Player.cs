using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MudObject {

    public NetworkPlayer networkPlayer;
    public bool isServer;
    IList<Item> inventory = new List<Item>();

    public Player(string name, string description, Room room, NetworkPlayer networkPlayer)
        : base(name, description, room) {
        this.name = name;
        this.room = room;
        this.networkPlayer = networkPlayer;
    }

    public bool Move(string direction) {
        int[] currentPos = GameManager.GetRoomPosition(room);
        Debug.Log(room.name + " Old Position " + currentPos[0] + " " + currentPos[1]);
        Room newRoom = null;
        switch (direction) {
            case "north":
                newRoom = GameManager.dungeon[currentPos[0] + 1, currentPos[1]];
                break;
            case "south":
                newRoom = GameManager.dungeon[currentPos[0] - 1, currentPos[1]];
                break;
            case "east":
                newRoom = GameManager.dungeon[currentPos[0], currentPos[1] + 1];
                break;
            case "west":
                newRoom = GameManager.dungeon[currentPos[0], currentPos[1] - 1];
                break;
            default:
                Debug.Log("Can't go to a gibberish direction");
                return false;
        }
        EnterRoom(newRoom);
        currentPos = GameManager.GetRoomPosition(room);
        Debug.Log(newRoom.name + " New Position " + currentPos[0] + " " + currentPos[1]);
        return true;
    }

    void Speak(string text) {
    
    }

    void Whisper(string text) {

    }

    void Examine(Entity entity) {
        
    }

    void EnterRoom(Room newRoom) {
        room.Exit(this);
        newRoom.Enter(this);
        this.room = newRoom;
    }

    void Pickup(Item item) {
        item.Owner = this;
        inventory.Add(item);
    }

    void Drop(Item item) {
        inventory.Remove(item);
        item.Owner = this.room;
    }

    bool Use(Item item, MudObject target) {
        if (item.Usable == target) {
            return true;
        }
        return false;
    }

    void ShowInventory() {
        
    }

    void Dead() {
        
    }
}
