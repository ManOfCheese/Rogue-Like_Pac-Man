using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

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
    public static event PowerPelletAction BlueMode;

    public delegate void EndOfBlueModeAction();
    public static event EndOfBlueModeAction EndBlueMode;

    public delegate void PacManDeathAction();
    public static event PacManDeathAction PacManDeath;

    public delegate void UltraPowerPelletAction();
    public static event UltraPowerPelletAction UltraPellet;

    public void OnPowerPelletEaten() {
        if (BlueMode != null)
            BlueMode();
    }

    public void OnBlueModeEnd() {
        if (EndBlueMode != null)
            EndBlueMode();
    }

    public void OnPacManDeath() {
        if (PacManDeath != null) {
            PacManDeath();
        }
    }

    public void OnUltraPelletEat() {
        if (UltraPellet != null) {
            UltraPellet();
        }
    }
}
