using UnityEngine;
using System.Collections;

public class MudObject : Entity {

    public Room room { get; set; }

    public MudObject(string name, string description, Room room)
        : base(name, description) {
        this.room = room;
    }
}
