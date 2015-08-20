using UnityEngine;
using System.Collections;

public class XSquaredMover : MonoBehaviour {
	
	public float amplitude;
	public float moveSpeed;
	public float modTime;
	
	private float X; // The X position of the laser
	private float Y; // The Z position of the laser since our game is in the X, Z plane
	private Vector3 moveDirection; // Direction the bullet needs to move in
	private Vector3 spawnLocation; // Reference to where the laser is spawned
	private Rigidbody r; // Reference to the rigidbody of the bullet. used in calculations
	private float spawnTime; // Time the bullet is spawned to be used in calculations
	
	
	void Start () {
		X = 0f;
        Y = 0f;
		r = GetComponent<Rigidbody>();
		spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		X = (Time.time - spawnTime) * modTime;

        Y = amplitude * Mathf.Sqrt((1 / X));
		moveDirection.Set ((transform.up * moveSpeed).x,  (transform.up * moveSpeed).y ,0.0f); // Set the move direction vector
		moveDirection += transform.right*Y; // Adding the current direction it is heading in
		r.MovePosition(r.position + moveDirection * Time.deltaTime); // Moves the bullet to the vector we want
	}
}
