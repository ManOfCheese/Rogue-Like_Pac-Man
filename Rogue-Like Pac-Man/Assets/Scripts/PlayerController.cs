using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Grid grid;
    public float baseSpeed;

    private Vector2 dir = new Vector2(1, 0);
    private Node currentNode;
    private float speed;

    private void Awake() {
        speed = baseSpeed;
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

        transform.position += new Vector3(dir.x * (speed * Time.deltaTime), dir.y * (speed * Time.deltaTime), 0);

        currentNode = grid.NodeFromWorldPoint(transform.position);
        foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
            if (new Vector2(currentNode.gridX + dir.x, currentNode.gridY + dir.y) == new Vector2(neighbor.gridX, neighbor.gridY) && !neighbor.walkable) {
                speed = 0;
            }
            else {
                speed = baseSpeed;
            }
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision) {
        speed = 0;
    }
    private void OnTriggerExit2D(Collider2D collision) {
        speed = baseSpeed;
    }*/
}
