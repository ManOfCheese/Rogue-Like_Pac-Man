using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

    private ObjectPooler objectPool;
    private Vector2 startPos = new Vector2(0.5f, -6.5f);
    private Vector2 turnUp = new Vector2(13.5f, -6.5f);
    private Vector2 turnLeft = new Vector2(13.5f, 10.5f); 
    private Vector2 turnDown = new Vector2(-13.5f, 10.5f);
    private Vector2 turnRight = new Vector2(-13.5f, -6.5f);
    private Vector2 currentAnchor;
    private Vector2 pelletScale = new Vector2(12, 12);
    private Vector2 dir = new Vector2(1, 0);
    private int stepsTaken;
    public int bossPelletsEaten;
    private int bossPelletReq = 100;

    public GameObject ultraPowerPellet;
    public GameObject inky;
    public GameObject blinky;
    public GameObject pinky;
    public GameObject clyde;
    private bool inkySpawned = false;
    private bool blinkySpawned = false;
    private bool pinkySpawned = false;
    private bool ultraPelletSpawned = false;

    // Use this for initialization
    void Start () {
        GameManager.Instance.reachedBoss = true;
        objectPool = GetComponent<ObjectPooler>();
        currentAnchor = startPos;
        StartCoroutine(SpawnPellets());
        Instantiate(clyde, new Vector2(-4.5f, 6.5f), Quaternion.identity);
    }
	
	public IEnumerator SpawnPellets() {
        yield return new WaitForSeconds(0.15f);
        objectPool.SpawnFromPool("Pellet", currentAnchor + (dir * stepsTaken), pelletScale);
        stepsTaken++;
        if (currentAnchor + (dir * stepsTaken) == turnUp) {
            dir = new Vector2(0, 1);
            currentAnchor = turnUp;
            stepsTaken = 0;
        }
        if (currentAnchor + (dir * stepsTaken) == turnLeft) {
            dir = new Vector2(-1, 0);
            currentAnchor = turnLeft;
            stepsTaken = 0;
        }
        if (currentAnchor + (dir * stepsTaken) == turnDown) {
            dir = new Vector2(0, -1);
            currentAnchor = turnDown;
            stepsTaken = 0;
        }
        if (currentAnchor + (dir * stepsTaken) == turnRight) {
            dir = new Vector2(1, 0);
            currentAnchor = turnRight;
            stepsTaken = 0;
        }
        StartCoroutine(SpawnPellets());
    }

    private void Update() {
        if (bossPelletsEaten >= bossPelletReq * 0.25f && inkySpawned == false) {
            Instantiate(inky, new Vector2(4.5f, 6.5f), Quaternion.identity);
            inkySpawned = true;
        }
        if (bossPelletsEaten >= bossPelletReq * 0.5f && blinkySpawned == false) {
            Instantiate(blinky, new Vector2(4.5f, 2.5f), Quaternion.identity);
            blinkySpawned = true;
        }
        if (bossPelletsEaten >= bossPelletReq * 0.75f && pinkySpawned == false) {
            Instantiate(pinky, new Vector2(-4.5f, -2.5f), Quaternion.identity);
            pinkySpawned = true;
        }
        if (bossPelletsEaten >= bossPelletReq && ultraPelletSpawned == false) {
            Instantiate(ultraPowerPellet, new Vector2(0.5f, -4.5f), Quaternion.identity);
            ultraPelletSpawned = true;
        }
    }
}
