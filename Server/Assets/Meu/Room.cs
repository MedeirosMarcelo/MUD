using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : Entity {

    public IList<Player> playerList = new List<Player>();
    public IList<Item> itemList = new List<Item>();
    public IList<MudObject> mudObjectList = new List<MudObject>();
    public Door doorNorth;
    public Door doorSouth;
    public Door doorEast;
    public Door doorWest;

    public Room(string name, string description)
        : base(name, description) {
    }

    public Room(string name, string description, IList<Item> itemList)
        : base(name, description) {
        foreach (Item item in itemList) {
            this.itemList.Add(item);
        }
    }

    public void Enter(Player player) {
        playerList.Add(player);
        mudObjectList.Add(player);
        player.room = this;
    }

    public void Exit(Player player) {
        playerList.Remove(player);
        mudObjectList.Remove(player);
    }

    public void AddItem(Item item) {
        itemList.Add(item);
        mudObjectList.Add(item);
    }

    public void AddObject(MudObject obj) {
        mudObjectList.Add(obj);
    }

    public void RemoveItem(Item item) {
        itemList.Remove(item);
        mudObjectList.Remove(item);
    }

    public void RemoveObject(MudObject obj) {
        mudObjectList.Remove(obj);
    }

    public IList<Item> Search() {
        if (itemList.Count > 0) {
            return itemList;
        }
        else {
            return null;
        }
    }

    public Item GetItem(string name) {
        foreach (Item item in itemList) {
            if (item.name.ToLower() == name.ToLower()) {
                return item;
            }
        }
        return null;
    }

    public MudObject GetMudObject(string name) {
        foreach (MudObject obj in mudObjectList) {
            if (obj.name.ToLower() == name.ToLower()) {
                return obj;
            }
        }
        return null;
    }
}