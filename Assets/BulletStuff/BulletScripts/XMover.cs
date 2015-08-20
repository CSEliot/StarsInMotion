using UnityEngine;
using System.Collections;

public class XMover : MonoBehaviour {

	public float moveSpeed; // Speed of the shot
    private float X;
    private float Y;
    private Rigidbody r;
    private float spawnTime;
    public float modTime;
    private Vector3 moveDirection;
    public float amplitude;

    void Start()
    {
        X = 0f;
        Y = 0f;
        r = GetComponent<Rigidbody>();
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        X = (Time.time - spawnTime) * modTime;

        Y = X;
        moveDirection.Set((transform.up * moveSpeed).x, (transform.up * moveSpeed).y, 0.0f); // Set the move direction vector
        moveDirection += transform.right * Y; // Adding the current direction it is heading in
    }

    void FixedUpdate()
    {
        r.MovePosition(r.position + (transform.right * amplitude) + (transform.up * moveSpeed)); // Moves the bullet to the vector we want
    }
}