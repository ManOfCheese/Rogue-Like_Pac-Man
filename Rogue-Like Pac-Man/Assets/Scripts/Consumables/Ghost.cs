using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Consumable {

    private Unit unit;                //Reference to unit.
    private AudioSource audioSource;  //Reference to audioSource.


    //Initialize variables.
    private void Start() {
        pointValue = 200;                           //Set the pointValue to 200.
        unit = GetComponent<Unit>();                //Get Unit.
        audioSource = GetComponent<AudioSource>();  //Get AudioSource.
    }


    //When this script is enabled.
    private void OnEnable() {
        EventManager.EndBlueMode += BlueModeEnd;  //Subscribe BlueModeEnd to the EndBlueMode event.
    }


    //When this script is disabled.
    private void OnDisable() {
        EventManager.EndBlueMode -= BlueModeEnd;  //Unsubscribe BlueModeEnd from the EndBlueMode event.
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (enabled && collision.gameObject.tag == "Player") {  //If we collide with the player and this script is enabled.
            unit = this.GetComponent<Unit>();                                                    //Get unit.
            audioSource.Play();                                                                  //Play ghostChomp sound.
            GameManager.Instance.GhostEatMultiplier *= 2;                                        //Double the ghost multiplier.
            GameManager.Instance.Score += pointValue * GameManager.Instance.GhostEatMultiplier;  //Add the pointvalue x the ghost multiplier to our score.
            unit.OnGhostEaten();                                                                 //Call the OnGhostEaten function on our unit.
        }   
    }


    //When blue mode is over.
    public void BlueModeEnd() {
        GameManager.Instance.GhostEatMultiplier = 1;  //Reset the ghost multiplier.
    }
}
