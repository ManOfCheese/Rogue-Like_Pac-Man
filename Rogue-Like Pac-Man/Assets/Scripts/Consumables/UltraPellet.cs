using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltraPellet : Consumable {

    private PlayerController player;  //Reference to the player.

    private void Start() {
        pointValue = 1000;  //Set pointValue to 1000.
    }


    public override void OnPelletEaten() {
        EventManager.Instance.OnUltraPelletEat();  //Call the UltraPellet event.
        base.OnPelletEaten();                      //Execute the base funcionality from the Consumable class.
    }
}
