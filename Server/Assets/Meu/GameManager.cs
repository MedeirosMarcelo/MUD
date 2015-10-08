using UnityEngine;
using System.Collections;

public static class GameManager {

    public static Room[,] dungeon = new Room[3, 3];
    public static Room startingRoom;
    public static string map = " [ ]---[ ]---[ ] \n  |     |     | \n  |     |     | \n [ ]---[ ]---[ ] \n  |     |     | \n  |     |     | \n [ ]---[ ]---[ ]";

    public static void BuildLevel () {

        Room room00 = new Room("room 00", "room description");
        Room room01 = new Room("room 01", "room description");
        Room room02 = new Room("room 02", "room description");
        Room room10 = new Room("room 10", "room description");
        Room room11 = new Room("room 11", "room description");
        Room room12 = new Room("room 12", "room description");
        Room room20 = new Room("room 20", "room description");
        Room room21 = new Room("room 21", "room description");
        Room room22 = new Room("room 22", "room description");

        for (int x = 0; x < dungeon.GetLength(0); x++) {
            for (int y = 0; y < dungeon.GetLength(1); y++) {

            }
        }
        dungeon[0, 0] = room00;
        dungeon[0, 1] = room01;
        dungeon[0, 2] = room02;
        dungeon[1, 0] = room10;
        dungeon[1, 1] = room11;
        dungeon[1, 2] = room12;
        dungeon[2, 0] = room20;
        dungeon[2, 1] = room21;
        dungeon[2, 2] = room22;

        startingRoom = room00;

        Item key = new Item("Key", "A brass key", room00, null, Action.OpenDoor, room00);
        room00.AddItem(key);
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

    public static void GetRoom(int x, int y) {
        
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

      //  int offsetX = 6;
      //  int offsetY = 40;
      //  string newMap = map.Remove((pos[0] * offsetX) * (pos[1] * offsetY) + 1, 1);
      //  string newMap = map.Insert((pos[0] * offsetX) * (pos[1] * offsetY) + 1, "x");
        string newMap = map.Remove(index, 1);
        newMap = newMap.Insert(index, "x");
        count++;
        return newMap;
    }

    /*
void CalcMap() {
    string newMap = "";
    bool breakLine = false;
    int count = 0;
    for (int i = 0; i < dungeon.GetLength(0); i++){
        for (int j = 0; j < dungeon.GetLength(1); j++) {
            if (dungeon[i, j] != null) {
                newMap += "[ ]---";
            }
            else {
                if (breakLine){
                    newMap += "\n  |     |     |";
                }
                else {
                        
                }
            }
            if (j == 2) {
                newMap = "\n  |     |     |";
            }
        }
    }
}*/
}
