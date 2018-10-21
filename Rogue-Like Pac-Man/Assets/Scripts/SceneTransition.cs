using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public int sceneNumber; //Needs to be assigned in editor.

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {    //If we collide with the player.
            SceneManager.LoadSceneAsync(sceneNumber);  //Load the scene assigned in the editor.
        }
    }
}
