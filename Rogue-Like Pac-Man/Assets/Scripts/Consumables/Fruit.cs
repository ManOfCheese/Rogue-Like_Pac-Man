using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Consumable {

    public int fruitPointValue;

    private void Start() {
        itemType = "fruit";
        pointValue = fruitPointValue;
    }

    public override void OnPelletEaten() {
        base.OnPelletEaten();
    }
}
