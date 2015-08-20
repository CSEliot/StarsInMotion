using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetCreditsInDock : MonoBehaviour {

    private Player Client;

	// Use this for initialization
	void Start () {
        Client = GameObject.Find("Player").GetComponent<Player>();
        GetComponent<Text>().text = "Credits : " + Client.GetCredits();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
