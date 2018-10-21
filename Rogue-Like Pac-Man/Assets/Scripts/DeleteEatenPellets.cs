using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEatenPellets : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (string pellet in GameManager.Instance.eatenPellets) {
            Destroy(GameObject.Find(pellet));
        }
        foreach (string powerPellet in GameManager.Instance.eatenPowerPellets) {
            Destroy(GameObject.Find(powerPellet));
        }
    }
}
