using UnityEngine;
using System.Collections;

public class Item : MudObject {

    public Entity Owner { get; set; }

    public Item (string name, string description, Room room, MudObject usable, Action action, Entity owner)
        : base (name, description, room, usable, action) {
        Owner = owner;
    }
}
