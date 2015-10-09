using UnityEngine;
using System.Collections;

public class Item : MudObject {

    public Entity Owner { get; set; }
    public MudObject Usable { get; set; }

    public Item (Entity owner) {
        Owner = owner;
    }
}
