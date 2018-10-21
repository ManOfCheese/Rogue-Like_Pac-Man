using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : Consumable {

    private void Start() {
        pointValue = 10;  //Set the pointValue.
    }


    //When this pellet is eaten.
    public override void OnPelletEaten() {
        GameManager.Instance.EatenPellets.Add(this.name);  //Add this pellet to the list of eaten pellets.
        base.OnPelletEaten();                              //Execute the base of this function in the Consumable class.
    }
}
