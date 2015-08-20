using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class GameplayGUI : MonoBehaviour {

	public Text Points;
	public Text Function;
	public Text TimeRemaining;
	private Player Player;
    private float MinutesRemaining;
	private GameObject Ship;

	private bool DiedFunctionCalled;
	// Use this for initialization
	void Start () {
		DiedFunctionCalled = false;
		//TimeRemaining = GameObject.Find ("TopPanel/Time Remaining").GetComponent<Text> ();
		Player = GameObject.Find ("Player").GetComponent<Player>();
		Ship = GameObject.FindGameObjectWithTag("Ship");
        //Points = GameObject.Find("Points").GetComponent<Text>();
        //get seconds remaining and convert to string
        MinutesRemaining = 60f * (float)Player.GetTimeToReturnByObject().Subtract(DateTime.Now).TotalHours;
        TimeRemaining.text = "" + Mathf.RoundToInt(MinutesRemaining);
		//Function = GameObject.Find ("Function").GetComponent<Text> ();
		Points.text = Player.GetCredits ().ToString ();

	}
	
	// Update is called once per frame
	void Update () {
		Points.text = Player.GetCredits ().ToString ();
        TimeRemaining.text = "" + Mathf.RoundToInt(MinutesRemaining - Time.timeSinceLevelLoad/60);
        if (!DiedFunctionCalled && (MinutesRemaining - Time.timeSinceLevelLoad/60) <= 0)
        {
			DiedFunctionCalled = true;
			Ship.GetComponent<ShipMove>().Died();
        }
	}

    public void SetFunctionName(string FunctionName)
    {
        Function.text = FunctionName;
    }
}
