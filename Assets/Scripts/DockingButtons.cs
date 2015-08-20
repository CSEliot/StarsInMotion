using UnityEngine;
using System.Collections;

public class DockingButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReturnToGame()
    {
        if (GameObject.Find("Player").GetComponent<Player>().GetRentedStatus())
        {
		    Debug.Log("Returning to Game . . .");
            Application.LoadLevel("AdventureScene");
        }
    }
    
    public void SaveAndQuit()
    {
		Debug.LogError("Saving and Quitting// FUNCTION DISABLED");
		//GameObject.Find("Player").GetComponent<Player>().SaveGame();
    }
    

}
