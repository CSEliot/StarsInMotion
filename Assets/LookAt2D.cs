using UnityEngine;
using System.Collections;

public class LookAt2D : MonoBehaviour {

	public GameObject Target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion rotation = Quaternion.LookRotation
			(Target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
	
	}
}
