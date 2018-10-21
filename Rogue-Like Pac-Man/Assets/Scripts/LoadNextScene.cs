using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour {

    public void LoadFirstLevel() {
        GameManager.Instance.GetComponent<AudioSource>().clip = GameManager.Instance.mainMusic;
        GameManager.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync("Lvl.1");
    }
}
