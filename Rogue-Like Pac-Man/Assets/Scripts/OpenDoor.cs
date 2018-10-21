using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public GameObject BossTransition;

    private void Update() {
        if (GameManager.Instance.score >= 2500) {
            BossTransition.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
