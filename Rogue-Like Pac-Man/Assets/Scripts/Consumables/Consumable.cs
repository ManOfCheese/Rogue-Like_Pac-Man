using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour {

    protected string itemType;
    protected int pointValue;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnPelletEaten();
        }
    }

    public virtual void OnPelletEaten() {
        GameManager.Instance.score += pointValue;
        Destroy(this.gameObject);
    }
}
