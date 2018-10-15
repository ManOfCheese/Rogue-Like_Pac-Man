using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    public int score;
    public Text scoreText;

    private void Update() {
        scoreText.text = "Score: " + score;
    }
}
