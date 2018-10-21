using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public GameObject BossTransition;  //Reference to the object that loads the BossFight scene. Needs to be assigned in editor.


    //Update Function.
    public void OnUpdate() {
        if (GameManager.Instance.Score >= 2500) {  //If our score is more than 2500.
            BossTransition.SetActive(true);        //Turn on the BossTranstion.
            this.gameObject.SetActive(false);      //Turn of the door.
        }
    }
}
