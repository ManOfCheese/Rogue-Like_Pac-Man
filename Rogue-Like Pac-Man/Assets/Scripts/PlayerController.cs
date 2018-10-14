using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Grid grid;
    public float baseSpeed;

    private Vector2 dir = new Vector2(-1, 0);
    private float speed;
    private Node nextNode;
    private Node currentNode;
    private Vector2 dest;
    private Rigidbody2D rb;

    private void Start() {
        transform.position = grid.NodeFromWorldPoint(transform.position).worldPos;
        speed = baseSpeed;
        rb = GetComponent<Rigidbody2D>();
        currentNode = grid.NodeFromWorldPoint(transform.position);
        dest = currentNode.worldPos;
        nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];
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

        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed * Time.deltaTime);
        rb.MovePosition(p);
        nextNode = grid.grid[grid.NodeFromWorldPoint(transform.position).gridX + Mathf.RoundToInt(dir.x), grid.NodeFromWorldPoint(transform.position).gridY + Mathf.RoundToInt(dir.y)];

        if (nextNode.walkable && (Vector2)nextNode.worldPos != dest) {
            dest = nextNode.worldPos;
        }
    }
}
