using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPellet : Consumable {

    private BossManager bossManager;  //Reference to the BossManager.


    //Initialize Variables.
    private void Start() {
        pointValue = 10;                                                            //Set the pointValue of the pellet.
        bossManager = GameObject.Find("ObjectPooler").GetComponent<BossManager>();  //Get the BossManager.
    }


    //When this pellet is eaten.
    public override void OnPelletEaten() {
        base.OnPelletEaten();            //Excute the base from the Consumable Class.
        bossManager.BossPelletsEaten++;  //Add one to BossPelletsEaten in the BossManager.
    }
}
