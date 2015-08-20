using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GetRentalTimeRemaining : MonoBehaviour {

    private ShipTracking TimeTracking;
    private  Text TimeRemainingText;
    private string OGText;
    private Player Client;
    private int TimeRemaining;
    public bool DisplayWaitTime; //referenced in Unity engine.

	// Use this for initialization
	void Start () {
        Client = GameObject.Find("Player").GetComponent<Player>();
        TimeRemainingText = GetComponent<Text>();
        OGText = TimeRemainingText.text;
        TimeRemaining = 0;
        NewTime();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
	}

    public void NewTime()
    {
        if (DisplayWaitTime == false)
        {
            if (Client.GetRentedStatus())
            {
                TimeRemainingText.text = OGText + (Client.GetRentalDueDate());
            }
            else
            {
                TimeRemainingText.text = OGText + "Please Rent A Ship.";
            }
        }
        else
        {
            if (Client.GetWaitListStatus())
            {
                TimeRemainingText.text = OGText + (Client.GetWaitTimeRemaining());
            }
            else
            {
                TimeRemainingText.text = OGText + "Please WaitList A Ship.";
            }
        }

    }
}
