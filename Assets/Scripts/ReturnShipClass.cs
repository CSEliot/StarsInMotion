using UnityEngine;
using System.Collections;

public class ReturnShipClass : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ReturnShipButton(bool FromGUI)
    {
        ShipTracking ShipTracker = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>();
        Player Player = GameObject.Find("Player").GetComponent<Player>();
        ServerPretend Server = GameObject.Find("Server").GetComponent<ServerPretend>();
        if (Player.GetRentedStatus())
        {   

            //Problem: Server needs to GET immediately before POST is called
            //         BUT, That just resets any changes made to the server.
            //Solution:Enforce GET calls to be made immediately, put post in server methods.
            Server.ReturnShip(ref Player);
            if (Player.GetWaitListStatus())
            {
                //Server will automatically handle renting a waitlisted ship.
                //So all we have to do is tell the player it's going from waitlist to
                //Rental . . . probably.
            }
            //Update the GUI
            Debug.Log("Telling ShipTracker to Return");
            ShipTracker.ShipReturned(Player.GetRentedShipName()); //just visuals happen here.
            //Updating the GUI Timestamp.
            GameObject.Find("Rental Time Remaining")
                .GetComponentInChildren<GetRentalTimeRemaining>().NewTime();

        }
        else if (FromGUI == false)
        {
            //In case user can't rent a ship, because someone else did first,
            // we call this method in order to remove it's icon from rented space.
            // Icon gets added before we actually know if we can rent.
            ShipTracker.ShipReturned(Player.GetRentedShipName());
        }
    }
}
