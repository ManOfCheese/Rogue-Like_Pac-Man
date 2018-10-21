using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Consumable {

    private bool blueMode;
    private Unit unit;
    private AudioSource audioSource;

    private void Start() {
        itemType = "ghost";
        pointValue = 200;
        unit = GetComponent<Unit>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        EventManager.EndBlueMode += BlueModeEnd;
    }

    private void OnDisable() {
        EventManager.EndBlueMode -= BlueModeEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (enabled && collision.gameObject.tag == "Player") {
            unit = this.GetComponent<Unit>();
            audioSource.Play();
            GameManager.Instance.ghostEatMultiplier *= 2;
            GameManager.Instance.score += pointValue * GameManager.Instance.ghostEatMultiplier;
            unit.OnGhostEaten();
        }   
    }

    public void BlueModeEnd() {
        GameManager.Instance.ghostEatMultiplier = 1;
    }
}
