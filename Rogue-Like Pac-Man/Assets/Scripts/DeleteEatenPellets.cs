using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEatenPellets : MonoBehaviour {

	void Start () {
        foreach (string pellet in GameManager.Instance.EatenPellets) {  
            Destroy(GameObject.Find(pellet));  //Delete every pellet in the list of eaten pellets in the GameManager.
        }
        foreach (string powerPellet in GameManager.Instance.EatenPowerPellets) {
            Destroy(GameObject.Find(powerPellet));  //Do the same for the power pellets.
        }
    }
}
