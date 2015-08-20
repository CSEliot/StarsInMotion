using UnityEngine;
using System.Collections;

public class SinMover : MonoBehaviour {

	public float amplitude;
	public float moveSpeed;
	public float modTime;

	private float Y; // The X position of the laser
	private float X; // The Z position of the laser since our game is in the X, Z plane
	private Vector3 moveDirection; // Direction the bullet needs to move in
	private Vector3 spawnLocation; // Reference to where the laser is spawned
	private Rigidbody r; // Reference to the rigidbody of the bullet. used in calculations
	private float spawnTime; // Time the bullet is spawned to be used in calculations


	void Start () {
		Y = 0f;
		r = GetComponent<Rigidbody>();
		spawnTime = Time.time;
		spawnLocation.Set(GetComponent<Transform>().position.x, GetComponent<Transform>().position.y, 0.0f);
	}

	// Update is called once per frame
	void Update () {
		Y = (Time.time - spawnTime) * modTime; // Calculating the new X position of the laser based on how much time has passed

		X = Sin(Y); // Determine where the shot is on the Z axis 
		
		moveDirection.Set ((GetComponent<Transform> ().up * moveSpeed).x, (GetComponent<Transform> ().up * moveSpeed).y, 0.0f); // Set the move direction vector
		moveDirection += transform.right*X; // Adding the current direction it is heading in
		r.MovePosition(r.position + moveDirection * Time.deltaTime); // Moves the bullet to the vector we want
	}

	private float Sin(float x){
		return amplitude * Mathf.Sin(x + Mathf.PI / 2f) ;
	}
}
