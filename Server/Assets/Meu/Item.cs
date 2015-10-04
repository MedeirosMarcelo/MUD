using UnityEngine;
using System.Collections;

public class Item : MudObject {

    public Entity Owner { get; set; }
    public MudObject Usable { get; set; }

    public Item (string name, string description, Room room, Entity owner)
        : base (name, description, room) {
        Owner = owner;
    }
}
