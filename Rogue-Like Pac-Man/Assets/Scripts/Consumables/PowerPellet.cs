using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Consumable {

    public delegate void PowerPelletAction();
    public static event PowerPelletAction OnPowerPelletEaten;

    private void Start() {
        itemType = "powerPellet";
        pointValue = 50;
    }

    public override void OnPelletEaten() {
        base.OnPelletEaten();
        if (OnPowerPelletEaten != null)
            OnPowerPelletEaten();
    }
}
