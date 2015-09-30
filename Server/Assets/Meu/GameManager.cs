using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public Room[,] dungeon = new Room[2, 2];
    public string map = @"[ ]---[ ]---[ ]
                           |     |     |
                           |     |     |
                          [ ]-- [ ]   [ ]
                                 |     |
                                 |     |
                                [ ]---[ ]";

    void Start() {
        BuildLevel ();
        Debug.Log(map);
    }

    void BuildLevel () {

        Room room1 = new Room();
        Room room2 = new Room();
        Room room3 = new Room();
        Room room4 = new Room();

        for (int x = 0; x < dungeon.GetLength(0); x++) {
            for (int y = 0; y < dungeon.GetLength(1); y++) {

            }
        }
        dungeon[0, 0] = room1;
        dungeon[0, 1] = room2;
        dungeon[1, 0] = room3;
        dungeon[1, 1] = room4;
    }
}
