using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Consumable {

    private bool blueMode;
    private Unit unit;

    private void Start() {
        itemType = "ghost";
        pointValue = 200;
        unit = GetComponent<Unit>();
    }

    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;
        EventManager.EndBlueMode += BlueModeEnd;
    }

    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;
        EventManager.EndBlueMode -= BlueModeEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (blueMode == true) {
            GameManager.Instance.ghostEatMultiplier *= 2;
            GameManager.Instance.score += pointValue * GameManager.Instance.ghostEatMultiplier;
            unit.OnGhostEaten();
        }
    }

    public void BlueModeActive() {
        blueMode = true;
    }

    public void BlueModeEnd() {
        blueMode = false;
    }
}
