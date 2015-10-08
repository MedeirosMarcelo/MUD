using UnityEngine;
using System.Collections;

public class Door : MudObject {

    public bool opened { get; set; }

    public Door (string name, string description, Room room, MudObject usable, Entity owner, Action action)
        : base (name, description, room, usable, action) {
    }

    public void Use() {
        Open();
    }

    public void Open() {
        if (!opened) {
            opened = true;
        }
    }

    public void Close() {
        if (opened) {
            opened = false;
        }
    }
}