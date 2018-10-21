using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour {

    protected int pointValue;  //pointValue of consumable.


    private void OnTriggerEnter2D(Collider2D collision) {  
        if (collision.gameObject.tag == "Player") {  //When colliding with pacman.
            OnPelletEaten();                         //Excute this functiion.
        }
    }


    //Happens when we are eaten.
    public virtual void OnPelletEaten() {
        GameManager.Instance.Score += pointValue;  //Add the pointValue to the score.
        Destroy(this.gameObject);                  //Kill yourself.
    }
}
