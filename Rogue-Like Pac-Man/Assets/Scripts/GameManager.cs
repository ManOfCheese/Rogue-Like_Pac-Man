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

    public int score;
    public Text scoreText;
    public int ghostEatMultiplier;
    public Scene currentScene;
    public List<string> eatenPellets = new List<string>();
    public List<string> eatenPowerPellets = new List<string>();

    public AudioClip menuMusic;
    public AudioClip mainMusic;

    public int lives = 3;
    public GameObject lifeImage;
    public List<GameObject> lifeImages;
    private GameObject canvas;
    public Vector2 respawnPos;
    public int bossPelletsEaten;
    public bool reachedBoss = false;

    public GameObject gameOver;
    public GameObject victory;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(GameObject.Find("Canvas"));
        currentScene = SceneManager.GetActiveScene();
        canvas = GameObject.Find("Canvas");
        for (int i = 0; i < lives; i++) {
            GameObject life = GameObject.Instantiate(lifeImage);
            life.transform.SetParent(canvas.transform);
            life.transform.localPosition = new Vector3(-316 + (i * 30), 178, 0);
            lifeImages.Add(life);
        }
        SceneManager.LoadSceneAsync(1);
    }

    private void InstantiateLives() {
        foreach (GameObject life in lifeImages) {
            life.SetActive(true);
        }
        lives = 3;
    }

    private void OnEnable() {
        EventManager.PacManDeath += OnPacManDeath;
    }

    private void OnDisable() {
        EventManager.PacManDeath -= OnPacManDeath;
    }

    private void Update() {
        scoreText.text = "Score: " + score;
    }

    public void OnPacManDeath() {
        if (lives >= 1) {
            lives -= 1;
            lifeImages[lives].SetActive(false);
            StartCoroutine(Dead());
        }
        if (lives <= 0) {
            gameOver.SetActive(true);
            StartCoroutine(EndGame());
        }
    }

    public void Victory() {
        victory.SetActive(true);
        StartCoroutine(EndGame());
    }

    public IEnumerator Dead() {
        yield return new WaitForSeconds(1.2f);
        if (!reachedBoss) {
            SceneManager.LoadSceneAsync("Lvl.1");
        }
        else if (reachedBoss) {
            SceneManager.LoadSceneAsync("BossFight");
        }
    }

    public IEnumerator EndGame() {
        yield return new WaitForSeconds(5);
        victory.SetActive(false);
        gameOver.SetActive(false);
        GetComponent<AudioSource>().clip = menuMusic;
        GetComponent<AudioSource>().Play();
        eatenPellets.Clear();
        eatenPowerPellets.Clear();
        reachedBoss = false;
        score = 0;
        InstantiateLives();
        SceneManager.LoadSceneAsync("MainMenu");
        GetComponent<AudioSource>().clip = menuMusic;
    }
}
