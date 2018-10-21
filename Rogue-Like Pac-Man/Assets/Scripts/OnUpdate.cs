using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnUpdate : MonoBehaviour {

    public BossManager bossManager;              //Needs to be assigned in editor.
    private GameManager gameManager;             //Needs to be assigned in editor.
    public PlayerController playercontroller;    //Needs to be assigned in editor.
    public List<Unit> units = new List<Unit>();  //Needs to be assigned in editor.
    public OpenDoor openDoor;                    //Needs to be assigned in editor.


    private void Start() {
         gameManager = GameManager.Instance;  //Get the gameManager. 
    }


    // Update is called once per frame
    void Update () {
		if (bossManager != null) {   //Update all of our script that need to be updated.
            bossManager.OnUpdate();
        }
        if (gameManager != null) {
            gameManager.OnUpdate();
        }
        if (playercontroller != null) {
            playercontroller.OnUpdate();
        }
        if (openDoor != null) {
            openDoor.OnUpdate();
        }
        foreach (Unit unit in units) {
            if (unit != null) {
                unit.OnUpdate();
            }
        }
    }
}
