using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MudObject {

    public NetworkPlayer networkPlayer;
    public bool isServer;
    IList<Item> inventory = new List<Item>();

    public Player(string name, string description, Room room, NetworkPlayer networkPlayer, MudObject usable, Action action)
        : base(name, description, room, usable, action) {
        this.name = name;
        this.room = room;
        this.networkPlayer = networkPlayer;
    }

    public bool Move(string direction) {
        int[] currentPos = GameManager.GetRoomPosition(room);
        Debug.Log(room.name + " Old Position " + currentPos[0] + " " + currentPos[1]);
        Room newRoom = null;
        int lengthI = GameManager.dungeon.GetLength(0);
        int lengthJ = GameManager.dungeon.GetLength(1);
        switch (direction) {
            case "north":
                if (currentPos[0] - 1 >= 0) newRoom = GameManager.dungeon[currentPos[0] - 1, currentPos[1]];
                break;
            case "south":
                if (currentPos[0] + 1 < lengthI) newRoom = GameManager.dungeon[currentPos[0] + 1, currentPos[1]];
                break;
            case "east":
                if (currentPos[1] + 1 < lengthJ) newRoom = GameManager.dungeon[currentPos[0], currentPos[1] + 1];
                break;
            case "west":
                if (currentPos[1] - 1 >= 0) newRoom = GameManager.dungeon[currentPos[0], currentPos[1] - 1];
                break;
            default:
                Debug.Log("Can't go to a gibberish direction");
                return false;
        }
        if (newRoom != null) {
            EnterRoom(newRoom);
        }
        else {
            return false;
        }
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

    public bool Pickup(string itemName) {
        Item item = room.GetItem(itemName);
        if (item != null) {
            item.Owner = this;
            inventory.Add(item);
            return true;
        }
        else {
            return false;
        }
    }

    public bool Drop(string itemName) {
        Item item = room.GetItem(itemName);
        if (item != null) {
            inventory.Remove(item);
            item.Owner = this.room;
            return true;
        }
        else {
            return false;
        }
    }

    public bool Use(Item item, MudObject target) {
        if (item.Usable == target) {
            return true;
        }
        return false;
    }

    public string GetInventoryList() {
        if (inventory.Count > 0) {
            string itemList = "";
            bool first = true;
            foreach (Item item in inventory) {
                if (first) {
                    itemList += item.name;
                }
                else {
                    itemList += ", " + item.name;
                    first = false;
                }
            }
            return itemList + ".";
        }
        return "You have nothing with you.";
    }

    void Dead() {
        
    }
}
