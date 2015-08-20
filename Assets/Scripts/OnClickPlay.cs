using UnityEngine;
using System.Collections;

public class OnClickPlay : MonoBehaviour {

    AudioSource MyAudio;

	// Use this for initialization
	void Start () {
        MyAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            MyAudio.Play();
        }
	}
}
