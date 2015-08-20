using UnityEngine;
using System.Collections;

public class DisableFriend : MonoBehaviour {

    private GameObject[] DisableFriends;
    private bool active;

	// Use this for initialization
	void Start () {
        DisableFriends = GameObject.FindGameObjectsWithTag("DisableGroup");
        active = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisableMyFriends()
    {
        active = !active;
        for (int i = 0; i < DisableFriends.Length; i++)
        {
            DisableFriends[i].SetActive(active);
        }
    }
}
