using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MudObject {

    public string playerName;
    public NetworkPlayer networkPlayer;
    IList<Item> inventory = new List<Item>();

    public Player(string name, NetworkPlayer networkPlayer) {
        this.playerName = name;
        this.networkPlayer = networkPlayer;
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
