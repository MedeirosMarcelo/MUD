using UnityEngine;
using System.Collections;

public static class ObjectAction {

    public static void Act(Action action, MudObject target) {
        switch (action){
            case Action.OpenDoor:
                OpenDoor(action, target);
                break;
            case Action.Shoot:
                Shoot(action, target);
                break;
        }
    }

    static void OpenDoor(Action action, MudObject target) {
        Door door = target as Door;
        door.Open();
    }

    static void Shoot(Action action, MudObject target) {
        
    }
}