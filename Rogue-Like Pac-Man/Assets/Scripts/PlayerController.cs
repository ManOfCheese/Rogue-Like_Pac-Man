using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private float baseSpeed = 10;  //Base speed.
    private float speed;           //Current speed.
     
    private Vector2 dest;                      //Current destination.
    public Vector2 dir;  //Current direction.

    private Node nextNode;            //The node directly in front of us.
    private Node currentNode;         //The node we are currently on.
    private Grid grid;                //The grid of nodes.
    private AudioSource audioSource;  //Audio source.
    private Rigidbody2D rb;           //2D Rigidbody.
    private Animator animator;        //Animator.

    private bool dead;                  //Is the player dead?
    private bool blueMode;              //Is blue mode active?
    private bool ultraPelletActivated;  //Is the ultra pellet active?


    //Initialize varaibles.
    private void Start() {
        grid = GameObject.Find("A*").GetComponent<Grid>();                          //Get the grid.
        transform.position = grid.NodeFromWorldPoint(transform.position).worldPos;  //Make sure we start on a node.
        speed = baseSpeed;                                                          //Set out speed equal to our base speed.
        rb = GetComponent<Rigidbody2D>();                                           //Get the rigidbody.
        currentNode = grid.NodeFromWorldPoint(transform.position);                  //Get our currentNode.
        dest = currentNode.worldPos;                                                //Set our destination to our currentNode.
        nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];  //Get the next node using currentNode and direction.
        animator = GetComponent<Animator>();                                        //Get animator.
        audioSource = GetComponent<AudioSource>();                                  //Get audio source.
    }


    //When this script is enabled.
    private void OnEnable() {
        EventManager.BlueMode += BlueModeActive;         //Subscribe BlueModeActive to the BlueMode event.
        EventManager.EndBlueMode += BlueModeEnd;         //Subscribe BlueModeEnd to the EndBlueMode event.
        EventManager.UltraPellet += OnUltraPelletEaten;  //Subscribe OnUltraPelletEaten to the UltraPellet event.
    }


    //When this script is disabled.
    private void OnDisable() {
        EventManager.BlueMode -= BlueModeActive;         //Unsubscribe BlueModeActive from the BlueMode event.
        EventManager.EndBlueMode -= BlueModeEnd;         //Unsubscribe BlueModeEnd from the EndBlueMode event.
        EventManager.UltraPellet -= OnUltraPelletEaten;  //Unsubscribe OnUltraPelletEaten from the UltraPellet event.
    }


    //Update Function.
    public void OnUpdate() {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0) {  //If we press the button to go right.
            dir = new Vector2(1, 0);                                   //Change dir to right.
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);  //Change rotation.
        }
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0) {  //Left.
            dir = new Vector2(-1, 0);
            this.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") > 0) {  //Up.
            dir = new Vector2(0, 1);
            this.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetButtonDown("Vertical") && Input.GetAxisRaw("Vertical") < 0) {  //Down.
            dir = new Vector2(0, -1);
            this.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }

        if (!dead) {                                                                            //If we're not dead.
            Vector2 p = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);  //Calculate a moveTowards to our destination.
            rb.MovePosition(p);                                                                 //Use that moveToward to move our rigidbody making it smooth.
            nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];  //Recalcute nextNode.
        }
        if (nextNode.walkable && (Vector2)nextNode.worldPos != dest) { dest = nextNode.worldPos; }  //If the next node is walkable and not our desination, make it our destination.
        if (ultraPelletActivated) { transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(55, 55), 2f * Time.deltaTime); }  //If the ultra pellet is active lerp out scale to 55.
    }


    //If blue mode is active.
    public void BlueModeActive() {
        blueMode = true;  //Set blueMode to true.
    }


    //When blue mode ends.
    public void BlueModeEnd() {
        blueMode = false;  //Set blueMode to false.
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ghost" && dead == false && blueMode == false && ultraPelletActivated == false) {  //When we collide with a ghost, we are not dead, blueMode is not active and the ultra pellet is not active.
            EventManager.Instance.OnPacManDeath();  //Execute the OnPacManDeath function of the gameManager.
            audioSource.Play();                     //Play the death sound.
            dead = true;                            //Set dead to true.
            animator.SetBool("Dead", dead);         //Tell the animator we are dead.
        }
    }


    //When the ultra pellet is eaten.
    public void OnUltraPelletEaten() {
        EventManager.Instance.OnPowerPelletEaten();  //Call the OnPowerPelletEaten function on the GameManager.
        ultraPelletActivated = true;                 //Set ultraPelletActive to true.
        GameManager.Instance.Victory();              //Call the Victory function on the GameManager.
    }


    //When PacMan is dead.
    private void PacManDead() {
        transform.position = GameManager.Instance.respawnPos;     //Set our position equal to the respawnPos from the GameManager.
        dir = new Vector2(-1, 0);                                 //Reset our direction.
        dead = false;                                             //Set dead to false.
        animator.SetBool("Dead", dead);                           //Tell the animator we are no longer dead.
    }
}
