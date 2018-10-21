using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Grid grid;
    public float baseSpeed;
    public Vector2 dir;

    private AudioSource audioSource;

    private float speed;
    private Node nextNode;
    private Node currentNode;
    private Vector2 dest;
    private Rigidbody2D rb;
    private Animator animator;
    private bool dead;
    private bool blueMode;
    private bool ultraPelletActivated;

    private void Start() {
        transform.position = grid.NodeFromWorldPoint(transform.position).worldPos;
        speed = baseSpeed;
        rb = GetComponent<Rigidbody2D>();
        currentNode = grid.NodeFromWorldPoint(transform.position);
        dest = currentNode.worldPos;
        nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;
        EventManager.EndBlueMode += BlueModeEnd;
        EventManager.UltraPellet += OnUltraPelletEaten;
    }

    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;
        EventManager.EndBlueMode -= BlueModeEnd;
        EventManager.UltraPellet -= OnUltraPelletEaten;
    }

    private void Update() {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {
            //Going Right.
            dir = new Vector2(1, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {
            //Going Left.
            dir = new Vector2(-1, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0) {
            //Going Up.
            dir = new Vector2(0, 1);
            this.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0) {
            //Going Down.
            dir = new Vector2(0, -1);
            this.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }

        if (!dead) {
            Vector2 p = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
            rb.MovePosition(p);
            nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];
        }

        if (nextNode.walkable && (Vector2)nextNode.worldPos != dest) {
            dest = nextNode.worldPos;
        }

        if (ultraPelletActivated) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(55, 55), 2f * Time.deltaTime);
        }
    }

    public void BlueModeActive() {
        blueMode = true;
    }

    public void BlueModeEnd() {
        blueMode = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ghost" && dead == false && blueMode == false && ultraPelletActivated == false) {
            EventManager.Instance.OnPacManDeath();
            audioSource.Play();
            dead = true;
            animator.SetBool("Dead", dead);
        }
    }

    public void OnUltraPelletEaten() {
        EventManager.Instance.OnPowerPelletEaten();
        ultraPelletActivated = true;
        GameManager.Instance.Victory();
    }

    private void PacManDead() {
        if (GameManager.Instance.currentScene.name != "Lvl.1") {
            SceneManager.LoadScene("Lvl.1");
        }
        transform.position = GameManager.Instance.respawnPos;
        dir = new Vector2(-1, 0);
        dead = false;
        animator.SetBool("Dead", dead);
    }
}
