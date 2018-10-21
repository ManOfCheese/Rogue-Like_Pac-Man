using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public Sprite shield; //sprite for shield pick-up
    public Sprite slowMotion; //sprite for slow-motion pick-up

    public void OnPickUp() {
        //Pick ups item when the player collides with it.
    }
}

public class PlayerController : MonoBehaviour {

    public GameObject player; //The gameobject that is the player

    //Shouldn't the be controls here maybe?
}

public class Enemy : MonoBehaviour {

    public float Speed { get; set; } //speed at which the enemy moves
    public GameObject Target { get; set; } //target the enemy moves towards
    private float retreatDistance; //distane between target and enemy at which it retreats

    public void PlayerDeath(Enemy enemy) {
        //Kills the player
    }

}

public class Car : Enemy {

    public Color CarColor { get; set; } //Determines color of the car.

}

public class Crocodile : Enemy {

    public float Size { get; set; } //Determines the size of the crocodile.

}

public class Snake : Enemy () {

    public Color SnakeColor { get; set; } //color of snek
    public GameObject Projectile { get; set; } //snek spit

    private void ShootAtPlayer() {
        //snek spits saliva super satisfyingly
    }

}


