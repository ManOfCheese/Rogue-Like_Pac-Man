using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraPellet : Consumable {

    private PlayerController player;

    private void Start() {
        itemType = "ultraPellet";
        pointValue = 1000;
    }

    public override void OnPelletEaten() {
        EventManager.Instance.OnUltraPelletEat();
        base.OnPelletEaten();
    }
}
