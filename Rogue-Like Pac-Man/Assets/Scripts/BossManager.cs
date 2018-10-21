using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

    public OnUpdate updater;                   //Script that updates the OnUpdate function. Needs to be assigned in editor.
    public int BossPelletsEaten { get; set; }  //The amount of pellets that have been eaten.
    public GameObject ultraPowerPellet;        //Prefab of the ultra pellet. Needs to be assigned in editor.
    public GameObject inky;                    //Prefab of Inky. Needs to be assigned in editor.
    public GameObject blinky;                  //etc.
    public GameObject pinky;
    public GameObject clyde;

    //Pellet trail variables.
    private ObjectPooler objectPool;                         //Reference to the ObjectPooler script.
    private Vector2 startPos = new Vector2(0.5f, -6.5f);     //Starting position for pellet spawning.
    private Vector2 turnUp = new Vector2(13.5f, -6.5f);      //Position where the pellet trail turns upwards.
    private Vector2 turnLeft = new Vector2(13.5f, 10.5f);    //Poisiton where the pellet trail turns left.
    private Vector2 turnDown = new Vector2(-13.5f, 10.5f);   //You get the point.
    private Vector2 turnRight = new Vector2(-13.5f, -6.5f);
    private Vector2 currentAnchor;                           //Last turn passed so that the pellets can be spawned relative to that position.
    private Vector2 pelletScale = new Vector2(12, 12);       //Scale of the pelelts.
    private Vector2 dir = new Vector2(1, 0);                 //The direction of the pellet trail.

    private int stepsTaken;                   //Steps taken since last anchor.
    private int bossPelletReq = 100;          //Amount of pellets that need to be eaten for the ultra pellet to appear.
    private bool inkySpawned = false;         //Has Inky Spawned?
    private bool blinkySpawned = false;       //etc.
    private bool pinkySpawned = false;        
    private bool ultraPelletSpawned = false;  //Has the ultra pellet spawned?

    // Use this for initialization
    void Start () {
        GameManager.Instance.ReachedBoss = true;                            //Set the ReachedBoss bool in the GameManager to true.
        objectPool = GetComponent<ObjectPooler>();                          //Get the ObjectPooler script.
        currentAnchor = startPos;                                           //Set the startPos to be the currentAnchor.
        StartCoroutine(SpawnPellets());                                     //Start the coroutine that spawns the pellet trail.
        clyde = Instantiate(clyde, new Vector2(-4.5f, 6.5f), Quaternion.identity);  //Spawn Clyde.
        updater.units.Add(clyde.GetComponent<Unit>());
    }
	

    //Spawns the pellet trail.
	public IEnumerator SpawnPellets() {
        yield return new WaitForSeconds(0.15f);                                               //Wait for 0.15 seconds between spawning pellets.
        objectPool.SpawnFromPool("Pellet", currentAnchor + (dir * stepsTaken), pelletScale);  //Spawn a pellet from the object pool.
        stepsTaken++;                                                                         //Add one to stepsTaken.
        if (currentAnchor + (dir * stepsTaken) == turnUp) {    //If we have reached the turnUp position.
            dir = new Vector2(0, 1);  //Set the direction to upwards
            currentAnchor = turnUp;   //Set it to be the anchor.
            stepsTaken = 0;           //Reset Steps Taken.
        }
        if (currentAnchor + (dir * stepsTaken) == turnLeft) {  //If we have reached the turnLeft position.
            dir = new Vector2(-1, 0);  //etc.
            currentAnchor = turnLeft;
            stepsTaken = 0;          
        }
        if (currentAnchor + (dir * stepsTaken) == turnDown) {  //If we have reached the turnDown position.
            dir = new Vector2(0, -1);  //etc.
            currentAnchor = turnDown; 
            stepsTaken = 0;           
        }
        if (currentAnchor + (dir * stepsTaken) == turnRight) {  //If we have reached the turnRight position.
            dir = new Vector2(1, 0);  //etc.
            currentAnchor = turnRight;
            stepsTaken = 0;           
        }
        StartCoroutine(SpawnPellets());  //Run the coroutine again for the next pellet.
    }


    //Update Function.
    public void OnUpdate() {
        if (BossPelletsEaten >= bossPelletReq * 0.25f && inkySpawned == false) {     //If you've a quarter of the required pellets.
            inky = Instantiate(inky, new Vector2(4.5f, 6.5f), Quaternion.identity);  //Spawn Inky.
            inkySpawned = true;
            updater.units.Add(inky.GetComponent<Unit>());  //Pass him to the updater script so that he gets updated.
        }
        if (BossPelletsEaten >= bossPelletReq * 0.5f && blinkySpawned == false) {        //At half the required pellets.
            blinky = Instantiate(blinky, new Vector2(4.5f, 2.5f), Quaternion.identity);  //Spawn Blinky.
            blinkySpawned = true;
            updater.units.Add(blinky.GetComponent<Unit>()); //Pass him to the updater script so that he gets updated.
        }
        if (BossPelletsEaten >= bossPelletReq * 0.75f && pinkySpawned == false) {        //At three quarters.
            pinky = Instantiate(pinky, new Vector2(-4.5f, -2.5f), Quaternion.identity);  //Pinky
            pinkySpawned = true;
            updater.units.Add(pinky.GetComponent<Unit>());  //etc.
        }
        if (BossPelletsEaten >= bossPelletReq && ultraPelletSpawned == false) {          
            Instantiate(ultraPowerPellet, new Vector2(0.5f, -4.5f), Quaternion.identity); 
            ultraPelletSpawned = true;
        }
    }
}
