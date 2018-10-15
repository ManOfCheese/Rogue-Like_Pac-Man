﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : Consumable {

    private void Start() {
        itemType = "pellet";
        pointValue = 10;
    }

    public override void OnPelletEaten() {
        base.OnPelletEaten();
    }
}