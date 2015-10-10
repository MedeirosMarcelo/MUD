using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MudObject {

    public NetworkPlayer networkPlayer;
    public bool isServer;
    IList<Item> inventory = new List<Item>();

    public Player(string name, string description, Room room, NetworkPlayer networkPlayer, MudObject usable, Action action)
        : base(name, description, room, usable, action) {
        this.networkPlayer = networkPlayer;
        room.Enter(this);
    }

    public string Move(string direction, out bool canMove, out Room oldRoom) {
        int[] currentPos = GameManager.GetRoomPosition(room);
        Debug.Log(room.name + " Old Position " + currentPos[0] + " " + currentPos[1]);
        Room newRoom = null;
        int lengthI = GameManager.dungeon.GetLength(0);
        int lengthJ = GameManager.dungeon.GetLength(1);
        Door doorChosen = null;
        switch (direction) {
            case "n":
            case "north":
                if (currentPos[0] - 1 >= 0) {
                    newRoom = GameManager.dungeon[currentPos[0] - 1, currentPos[1]];
                    doorChosen = room.doorNorth;
                }
                break;
            case "s":
            case "south":
                if (currentPos[0] + 1 < lengthI) {
                    newRoom = GameManager.dungeon[currentPos[0] + 1, currentPos[1]];
                    doorChosen = room.doorSouth;
                }
                break;
            case "e":
            case "east":
                if (currentPos[1] + 1 < lengthJ) {
                    newRoom = GameManager.dungeon[currentPos[0], currentPos[1] + 1];
                    doorChosen = room.doorEast;
                }
                break;
            case "w":
            case "west":
                if (currentPos[1] - 1 >= 0) {
                    newRoom = GameManager.dungeon[currentPos[0], currentPos[1] - 1];
                    doorChosen = room.doorWest;
                }
                break;
            default:
                Debug.Log("Can't go to a gibberish direction");
                canMove = false;
                oldRoom = null;
                return "You can't go to a gibberish direction";
        }
        if (newRoom != null) {
            if (doorChosen != null) {
                if (doorChosen.locked == false) {
                    oldRoom = room;
                    EnterRoom(newRoom);
                    currentPos = GameManager.GetRoomPosition(room);
                    Debug.Log(newRoom.name + " New Position " + currentPos[0] + " " + currentPos[1]);
                    canMove = true;
                    return "You go through the door.";
                }
                else {
                    canMove = false;
                    oldRoom = null;
                    return "The door is locked";
                }
            }
            else {
                oldRoom = room;
                EnterRoom(newRoom);
                currentPos = GameManager.GetRoomPosition(room);
                canMove = true;
                return "You go through the door.";
            }
        }
        else {
            canMove = false;
            oldRoom = null;
            return "Yeah... you can't go that way.";
        }
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
    }

    public bool Pickup(string itemName) {
        Item item = room.GetItem(itemName);
        if (item != null) {
            item.Owner = this;
            inventory.Add(item);
            room.RemoveItem(item);
            room.RemoveObject(item);
            return true;
        }
        else {
            return false;
        }
    }

    public bool Drop(string itemName) {
        Item item = GetItem(itemName);
        if (item != null) {
            inventory.Remove(item);
            item.Owner = this.room;
            room.AddItem(item);
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

    public Item GetItem(string name) {
        foreach (Item item in inventory) {
            if (item.name.ToLower() == name.ToLower()) {
                return item;
            }
        }
        return null;
    }

    void Dead() {

    }
}
