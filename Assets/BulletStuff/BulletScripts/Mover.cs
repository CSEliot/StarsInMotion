using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speed; // The speed of the bullet
	
	void Start () {

		GetComponent<Rigidbody>().velocity = GetComponent<Transform>().up * speed; //Give the bullet a straight line velocity. This is the basic forward shot
	}
}