using UnityEngine;
using System.Collections;

public enum Action {
    None,
    OpenDoor,
    Shoot
}

public class MudObject : Entity {

    public Room room { get; set; }
    public MudObject Usable { get; set; }
    public Action action { get; set; }

    public MudObject(string name, string description, Room room, MudObject usable, Action action)
        : base(name, description) {
        this.room = room;
        Usable = usable;
        this.action = action;
    }

    public void Use (MudObject target) {
        ObjectAction.Act(action, target);
    }
}
