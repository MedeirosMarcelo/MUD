using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : Entity {

    IList<Player> playerList = new List<Player>();
    IList<Item> itemList = new List<Item>();
    IList<MudObject> mudObjectList = new List<MudObject>();

    public Room(string name, string description)
        : base(name, description) {
    }

    public Room(string name, string description, IList<Item> itemList)
        : base(name, description) {
        foreach (Item item in itemList) {
            this.itemList.Add(item);
        }
    }

    public void Enter (Player player) {
        playerList.Add(player);
    }

    public void Exit(Player player) {
        playerList.Remove(player);
    }

    public void AddItem(Item item) {
        itemList.Add(item);
    }

    public void AddObject(MudObject obj) {
        mudObjectList.Add(obj);
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