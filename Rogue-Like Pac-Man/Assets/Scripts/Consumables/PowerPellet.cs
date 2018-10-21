using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Consumable {

    private void Start() {
        itemType = "powerPellet";
        pointValue = 50;
    }

    public override void OnPelletEaten() {
        GameManager.Instance.eatenPowerPellets.Add(this.name);
        base.OnPelletEaten();
        EventManager.Instance.OnPowerPelletEaten();
    }
}
