using UnityEngine;
using System.Collections;

public static class GameManager {

    public static Room[,] dungeon = new Room[3, 3];
    public static Room startingRoom;
    public static string map = " [ ]---[ ]---[ ] \n  |     |     | \n  |     |     | \n [ ]---[ ]---[ ] \n              | \n              | \n             [ ]";

    public static void BuildLevel () {

        Room room00 = new Room("room 00", "It's clearly a room");
        Room room01 = new Room("room 01", "It's clearly a room");
        Room room02 = new Room("room 02", "It's clearly a room");
        Room room10 = new Room("room 10", "It's clearly a room");
        Room room11 = new Room("room 11", "It's clearly a room");
        Room room12 = new Room("room 12", "It's clearly a room");
        //Room room20 = new Room("room 20", "room description");
        //Room room21 = new Room("room 21", "room description");
        Room room22 = new Room("room 22", "It's clearly a room");

        Item key = new Item("Key", "A brass key", room11, null, Action.OpenDoor, room11);
        room11.AddItem(key);

        Door door = new Door("Door", "A wooden door.", room12, room22, key, null, Action.None);
        door.locked = true;
        room12.doorSouth = door;
        room12.AddObject(door);
        room22.doorNorth = door;
        room22.AddObject(door);

        dungeon[0, 0] = room00;
        dungeon[0, 1] = room01;
        dungeon[0, 2] = room02;
        dungeon[1, 0] = room10;
        dungeon[1, 1] = room11;
        dungeon[1, 2] = room12;
        //dungeon[2, 0] = room20;
        //dungeon[2, 1] = room21;
        dungeon[2, 2] = room22;

        startingRoom = room00;
    }

    public static int[] GetRoomPosition(Room room) {
        for (int i = 0; i < dungeon.GetLength(0); i++) {
            for (int j = 0; j < dungeon.GetLength(1); j++) {
                if (room == dungeon[i, j]) {
                    int[] position = new int[2] {i, j};
                    return position;
                }
            }
        }
        return null;
    }

    static int count = 0;
    public static string UpdateMapPosition(Player player) {
        int[] pos = GetRoomPosition(player.room);
        int index = 0;
        if (pos[0] == 0 && pos[1] == 0) index = 2;
        else if (pos[0] == 0 && pos[1] == 1) index = 8;
        else if (pos[0] == 0 && pos[1] == 2) index = 14;
        else if (pos[0] == 1 && pos[1] == 0) index = 54;
        else if (pos[0] == 1 && pos[1] == 1) index = 60;
        else if (pos[0] == 1 && pos[1] == 2) index = 66;
        else if (pos[0] == 2 && pos[1] == 0) index = 106;
        else if (pos[0] == 2 && pos[1] == 1) index = 112;
        else if (pos[0] == 2 && pos[1] == 2) index = 118;

        string newMap = map.Remove(index, 1);
        newMap = newMap.Insert(index, "x");
        count++;
        return newMap;
    }
}
