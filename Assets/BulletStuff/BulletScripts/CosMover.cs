using UnityEngine;
using System.Collections;

public class CosMover : MonoBehaviour {

	public float amplitude;
	public float moveSpeed;
	public float modTime; // makes the bullet go faster
	public float offset;

	private float X; // The X position of the laser
	private float Y; // The Z position of the laser since our game is in the X, Z plane
	private Vector3 moveDirection; // Direction the bullet needs to move in
	private Vector3 spawnLocation; // Reference to where the laser is spawned
	private Rigidbody r; // Reference to the rigidbody of the bullet. used in calculations
	private float spawnTime; // Time the bullet is spawned to be used in calculations

	//Called when an object is instantiated

	void Start () {
		X = 0f;
		r = GetComponent<Rigidbody>();
		spawnTime = Time.time;
        transform.localPosition += transform.right * offset;//new Vector3(transform.localPosition.x+offset, transform.localPosition.y, transform.localPosition.z);
	}

	// Update is called once per frame
	void Update () {
		X = (Time.time - spawnTime) * modTime; // Calculating the new X position of the laser based on how much time has passed

		Y = Cos(X); // Determine where the shot is on the Z axis 
		
		moveDirection.Set ((GetComponent<Transform> ().up * moveSpeed).x, (GetComponent<Transform> ().up * moveSpeed).y, 0.0f); // Set the move direction vector
		moveDirection += transform.right*Y; // Adding the current direction it is heading in
		r.MovePosition(r.position + moveDirection * Time.deltaTime); // Moves the bullet to the vector we want
		
	}
	
	//Calculates cosine for the pattern we want the shot to move in
	private float Cos(float x){
		return amplitude * Mathf.Cos(x + Mathf.PI / 2);
	}
}
