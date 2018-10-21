using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Consumable {

    private void Start() {
        pointValue = 50;  //Set pointValue to 50.
    }


    //When this pellet is eaten.
    public override void OnPelletEaten() {
        GameManager.Instance.EatenPowerPellets.Add(this.name);  //Add this pellet to the EatenPowerPelletsList.
        base.OnPelletEaten();      
        EventManager.Instance.OnPowerPelletEaten();             //Call the BlueMode event.
    }
}
