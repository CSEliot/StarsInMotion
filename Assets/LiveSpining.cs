using UnityEngine;
using System.Collections;

public class LiveSpining : MonoBehaviour {

	public float tumble;
	
	void FixedUpdate ()
	{
		transform.Rotate(new Vector3(1f, 0, 0), tumble);

	}

}
