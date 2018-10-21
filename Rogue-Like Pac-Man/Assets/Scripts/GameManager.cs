using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _instance;

    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    public int Score { get; set; }               //Score.
    public int Lives { get; set; }               //Amount of lives.
    public int BossPelletsEaten { get; set; }    //Amount of bossPellets eaten.
    public int GhostEatMultiplier { get; set; }  //Multiplier that increases with each ghost you eat.

    public Vector2 respawnPos;                           //Needs to be assigned in editor.
    public List<string> EatenPellets { get; set; }       //List of pellets we have eaten.
    public List<string> EatenPowerPellets { get; set; }  //List of power pellets we have eaten.

    public Text scoreText;                   //Needs to be assigned in editor.
    public Scene CurrentScene { get; set; }  //Scene we are currently in.
    public AudioClip menuMusic;              //Needs to be assigned in editor.
    public AudioClip mainMusic;              //Needs to be assigned in editor.

    public bool ReachedBoss { get; set; }
    public GameObject gameOver;          //Needs to be assigned in editor.
    public GameObject victory;           //Needs to be assigned in editor.
    public GameObject lifeImage;         //Needs to be assigned in editor.
    public List<GameObject> LifeImages { get; set; }  //List of the sprites that represent our lives.

    private GameObject canvas;  //Reference to our canvas.

    private void Awake() {
        EatenPellets = new List<string>();             //Instiate list.
        EatenPowerPellets = new List<string>();        //Instiate list.
        ReachedBoss = false;                           //Set ReachedBoss
        LifeImages = new List<GameObject>();           //Instantiate LifeImages.
        Lives = 3;                                     //Set Lives.
        DontDestroyOnLoad(gameObject);                 //Dont destroy this game object.
        DontDestroyOnLoad(GameObject.Find("Canvas"));  //Dont destroy the canvas either.
        CurrentScene = SceneManager.GetActiveScene();  //Set the CurrentScene to be our active scene.
        canvas = GameObject.Find("Canvas");            //Get the canvas.
        for (int i = 0; i < Lives; i++) {              //Instantiate an amount of life sprites equal to our lives.
            GameObject life = GameObject.Instantiate(lifeImage);
            life.transform.SetParent(canvas.transform);
            life.transform.localPosition = new Vector3(-316 + (i * 30), 178, 0);
            LifeImages.Add(life);
        }
        SceneManager.LoadSceneAsync(1);  //Load the first scene.
    }


    //Reinstantiate our lives.
    private void InstantiateLives() {
        foreach (GameObject life in LifeImages) {
            life.SetActive(true);
        }
        Lives = 3;
    }


    //When this script is enabled.
    private void OnEnable() {
        EventManager.PacManDeath += OnPacManDeath;  //Subscribe OnPacManDeath to the PacManDeath event.
    }


    //When this script is disabled.
    private void OnDisable() {
        EventManager.PacManDeath -= OnPacManDeath;  //Unsubscribe OnPacManDeath from the PacManDeath event.
    }


    //Update Function.
    public void OnUpdate() {
        scoreText.text = "Score: " + Score;  //Display our score in the ui.
    }


    //When PacMan Dies.
    public void OnPacManDeath() {
        if (Lives >= 1) {                        //If this is not our last life.
            Lives -= 1;                          //Subtract one life.
            LifeImages[Lives].SetActive(false);  //Disable one of the sprites.
            StartCoroutine(Dead());              //Start the Dead coroutine.
        }
        if (Lives <= 0) {               //If this is our last life.
            gameOver.SetActive(true);   //Set the gameOver boolean to true.
            StartCoroutine(EndGame());  //Start the EndGame Coroutine.
        }
    }


    //When we finish the game.
    public void Victory() {
        victory.SetActive(true);
        StartCoroutine(EndGame());
    }


    //When we die.
    public IEnumerator Dead() {
        yield return new WaitForSeconds(1.2f);  //Wait
        if (!ReachedBoss) {                        //If we have not reached the boss.
            SceneManager.LoadSceneAsync("Lvl.1");  //Reload lvl. 1
        }
        else if (ReachedBoss) {                        //If we have reached the boss.
            SceneManager.LoadSceneAsync("BossFight");  //Reload the BossFight.
        }
    }


    //When we run out of lives.
    public IEnumerator EndGame() {
        yield return new WaitForSeconds(5);  //Wait.
        victory.SetActive(false);                      //Set on screen text to inactive.
        gameOver.SetActive(false);
        GetComponent<AudioSource>().clip = menuMusic;  //Switch to menu music.
        GetComponent<AudioSource>().Play();
        EatenPellets.Clear();                          //Clear our lists of pellets.
        EatenPowerPellets.Clear();  
        ReachedBoss = false;                           //Reset ReahedBoss bool.
        Score = 0;                                     //Reset Score.
        InstantiateLives();                            //Reinstantiate our lives.
        SceneManager.LoadSceneAsync("MainMenu");       //Load the main menu.
    }
}
