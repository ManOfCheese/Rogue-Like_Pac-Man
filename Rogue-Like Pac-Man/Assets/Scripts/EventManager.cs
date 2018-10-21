using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    //Singleton
    private static EventManager _instance;

    public static EventManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<EventManager>();
            }

            return _instance;
        }
    }

    public delegate void PowerPelletAction();
    public static event PowerPelletAction BlueMode;  //Delegate and event for entering blue mode.

    public delegate void EndOfBlueModeAction();
    public static event EndOfBlueModeAction EndBlueMode;  //Delegate and evetn for the end of blue mode.

    public delegate void PacManDeathAction();
    public static event PacManDeathAction PacManDeath;  //Delegate and event for when pacman dies.

    public delegate void UltraPowerPelletAction();
    public static event UltraPowerPelletAction UltraPellet;  //Delegate and event for when the ultra pellet is eaten.


    public void OnPowerPelletEaten() {
        if (BlueMode != null)
            BlueMode();  //If the blue mode event exist call it.
    }


    public void OnBlueModeEnd() {
        if (EndBlueMode != null)
            EndBlueMode(); //If the end blue mode event exist call it.
    }


    public void OnPacManDeath() {
        if (PacManDeath != null) {
            PacManDeath();  //etc.
        }
    }


    public void OnUltraPelletEat() {
        if (UltraPellet != null) {
            UltraPellet();
        }
    }
}
