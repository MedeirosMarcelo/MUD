using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : Entity {

    IList<Player> playerList = new List<Player>();
    IList<Item> objectList = new List<Item>();

    public Room() {
    }
    
    public Room (IList<Item> objectList) {
        foreach (Item obj in objectList) {
            this.objectList.Add(obj);
        }
    }

    public void Enter (Player player) {
        playerList.Add(player);
    }

    public void Exit(Player player) {
        playerList.Remove(player);
    }
}