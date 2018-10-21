using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPellet : Consumable {

    private BossManager bossManager;

    private void Start() {
        itemType = "bossPellet";
        pointValue = 10;
        bossManager = GameObject.Find("ObjectPooler").GetComponent<BossManager>();
    }

    public override void OnPelletEaten() {
        base.OnPelletEaten();
        bossManager.bossPelletsEaten++;
    }
}
