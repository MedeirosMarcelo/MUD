using UnityEngine;
using System.Collections;

public class Door : MudObject {

    public bool locked { get; set; }
    public Room room2;

    public Door(string name, string description, Room room, Room room2, MudObject usable, Entity owner, Action action)
        : base (name, description, room, usable, action) {
    }

    public void Use() {
        Open();
    }

    public void Open() {
        if (locked) {
            locked = false;
        }
    }

    public void Close() {
        if (!locked) {
            locked = true;
        }
    }

    public Room GetNextRoom(Room currentRoom) {
        if (currentRoom == room) {
            return room2;
        }
        else if (currentRoom == room2) {
            return room;
        }
        else {
            return null;
        }
    }
}