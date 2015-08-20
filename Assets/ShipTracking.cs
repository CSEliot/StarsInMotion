using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


public class ShipTracking : MonoBehaviour {

    private string RentedShip;
    private string RentedShipGuiBox;
    
    private string WaitListedShip;
    private string WaitListedShipGuiBox;
    
    private DropMe CurrentShipGUI;
    
    private DropMe WaitListedShipGUI;

    private GameObject[] AllShipsInGUI;
    public GameObject[] ShipPrefabs;
    private ServerPretend Server;
    private Player Client;
    private List<string> RentedShips;


    /*
	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		if (FindObjectsOfType(GetType()).Length > 1){
			Destroy(gameObject);
		}
	}*/

	// Use this for initialization
	void Start () {
        Server = GameObject.Find("Server").GetComponent<ServerPretend>();
        Client = GameObject.Find("Player").GetComponent<Player>();

        Server.GET();

        //A. Update credits on docking to station.
        Server.UpdateCredits(Client.GetUserName(), Client.GetCredits());
		

        //AllShipsInGUI = GameObject.FindGameObjectsWithTag("DisableGroup");
        RentedShip = Client.GetRentedShipName();
        WaitListedShip = Client.GetWaitListedShipName();
        RentedShipGuiBox = "none";
        
        GameObject.Find("CurrentShip").GetComponentInChildren<DropMe>().ResetImage();
        AllShipsInGUI = GameObject.FindGameObjectsWithTag("DisableGroup");

        UpdateDockingStation(false);
        Client.SetShipRented(RentedShip, GetSelectedShip(), true);
	}
	
	// Update is called once per frame
	void Update () {
		if(RentedShip != "" && Client.GetRentedStatus()){
			//if a ship is rented, test to see if it's time to return it.
			//Debug.Log("Returning Ship in: " + Client.GetTimeToReturnByObject().Subtract(DateTime.Now).Seconds);
            if (DateTime.Now.CompareTo(Client.GetTimeToReturnByObject()) > 0)
            {
				GameObject.Find("DockingFunctions").GetComponent<ReturnShipClass>().ReturnShipButton(true);
				if(WaitListedShip != "" && Client.GetWaitListStatus()){
					Debug.Log("AUTO WAITLISTING AFTER SHIP RETURN NOT IMPLEMENTED YET");
				}
			}
		}
	}

	/// <summary>
	/// Updates the docking station.
	/// </summary>
    public void UpdateDockingStation(bool CallFromServer)
    {
        
        StartCoroutine(DockingStationCoRoutine(CallFromServer));
        
    }

    IEnumerator DockingStationCoRoutine(bool CallFromServer)
    {
        Debug.Log("In Station Coroutine");
        Queue<int> coroutineQueue;
        coroutineQueue = GameObject.FindGameObjectWithTag("Server").
            GetComponent<ServerPretend>().coroutineQueue;
        int myCoroutineFrame = Time.frameCount;
        coroutineQueue.Enqueue(myCoroutineFrame);

        if (coroutineQueue.Peek() != myCoroutineFrame)
        {
            yield return coroutineQueue.Peek() != myCoroutineFrame;
        }

        if (!CallFromServer)
        {
            Debug.Log("ShipTracking > UpdateDockingStation > Normal");
            RentedShips = Server.GetRentedShips(Client.GetUserName());
        }
        else
        {
            //doesn't make update call to server, which prevents infinite loop.
            Debug.Log("ShipTracking > UpdateDockingStation > CallFromServer");
            RentedShips = Server.GetRentedShips2(Client.GetUserName());
        }
        for (int i = 0; i < AllShipsInGUI.Length; i++)
        {
            string ShipName = "Ship " + AllShipsInGUI[i].name.Remove(0, 9);
            if (RentedShips.Contains(ShipName))
            {
                Debug.Log("Rented Ship Found is: " + ShipName);
                AllShipsInGUI[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log("NOT rented Ship Found is: " + ShipName);
                AllShipsInGUI[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            Destroy(GameObject.Find("icon"));
            //Debug.Log("Ship name: " + AllShipsInGUI[i].name);
            //AllShipsInGUI[shipNum].SetActive(false);
        }
        //if player has rented a ship, display it on the station
        if (Client.GetRentedStatus())
        {

            //go through the GUI list and grab the right box image
            for (int i = 0; i < AllShipsInGUI.Length; i++)
            {
                string ShipName = "Ship " + AllShipsInGUI[i].name.Remove(0, 9);
                if (ShipName == RentedShip)
                {
                    Debug.Log("Rented Ship GUI Found: " + RentedShip);
                    AllShipsInGUI[i].transform.GetChild(0).gameObject.SetActive(true);
                    Sprite RentedSprite = AllShipsInGUI[i].transform.GetChild(0).GetComponent<Image>().sprite;
                    GameObject.Find("CurrentShip").transform.GetChild(0).GetChild(1).
                        GetComponent<Image>().overrideSprite = RentedSprite;
                    //AllShipsInGUI[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            GameObject.Find("CurrentShip").GetComponentInChildren<DropMe>().ResetImage();
        }
        //if player has WAITLISTED a ship, display it on the station
        if (Client.GetWaitListStatus())
        {

            //go through the GUI list and grab the right box image
            for (int i = 0; i < AllShipsInGUI.Length; i++)
            {
                string ShipName = "Ship " + AllShipsInGUI[i].name.Remove(0, 9);
                if (ShipName == WaitListedShip)
                {
                    Debug.Log("WaitListed Ship GUI Found: " + RentedShip);
                    Sprite RentedSprite = AllShipsInGUI[i].transform.GetChild(0).GetComponent<Image>().sprite;
                    GameObject.Find("AddToHold").transform.GetChild(0).GetChild(1).
                        GetComponent<Image>().overrideSprite = RentedSprite;
                    //AllShipsInGUI[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            GameObject.Find("AddToHold").GetComponentInChildren<DropMe>().ResetImage();
        }
        coroutineQueue.Dequeue();
    }


	public GameObject GetSelectedShip(){
		for(int i = 0; i < ShipPrefabs.Length; i++){
			if(RentedShip == ShipPrefabs[i].name){
				Debug.Log("Ship Prefab Match is: " + ShipPrefabs[i].name);
				return ShipPrefabs[i];
			}else{
				//Debug.Log("Ship NOT match is: " + RentedShip + ", " + ShipPrefabs[i].name);
			}
		}
		return null;
	}

    public void ShipReturned(string ShipName)
    {
		Debug.Log("ShipTracking > ShipReturned");
        RentedShip = "";
        //UpdateDockingStation(false);
        //Server is alerted of returned ship elsewhere, 
        // so we just do local ship return (="";) and update GUI 
        // Docking Station.
    }
    
    public void ShipWaitListed(string NewShip){
		Debug.Log("ShipTracking > ShipWaitlist");
		if(RentedShip != ""){
            if (!Server.WaitListShip(Client.GetUserName(), NewShip))
            {
                Debug.Log("WAITLISTING FAILED");
            }
			WaitListedShip = NewShip;
			Client.SetShipWaitListed(NewShip, false);
			UpdateDockingStation(false);
			GameObject.Find("Hold List").GetComponentInChildren<GetRentalTimeRemaining>().NewTime();
		}else{
			ShipRented(NewShip, false);
		}
    }
    
    /// <summary>
    /// Ships the removed from wait list.
    /// Should only be called by the server!!
    /// </summary>
    public void RemoveShipFromWaitList(){
		Debug.Log("ShipTracking > RemoveShipFromWaitList");
		WaitListedShip = "";
		WaitListedShipGuiBox = "";
		
		GameObject.Find("AddToHold").GetComponentInChildren<DropMe>().ResetImage();
		
		Client.SetWaitListRemoved();
    }
    
    
    public void ShipRented(string NewShip, bool FromServer)
    {
		Debug.Log("ShipTracking > ShipRented");
		if(!FromServer && Server.RentingShip(Client.GetUserName(), NewShip)){
            Debug.Log("ShipRented Success?");
            RentedShip = NewShip;
            Client.SetShipRented(NewShip, GetSelectedShip(), false); //IS ALREADY CALLED IN ONDROP CLASS
            UpdateDockingStation(false);
            GameObject.Find("Rental Time Remaining").GetComponentInChildren<GetRentalTimeRemaining>().NewTime();
            Debug.Log("ShipRented Success...");
        }
        else if(FromServer){
			//Being called by the server, so we don't need the RentingShip function to be called.
			//We just need the GUI related functions to be called.
			RentedShip = NewShip;
			Client.SetShipRented(NewShip, GetSelectedShip(), false);
        }
        else {
            Debug.Log("RENTING SHIP RETURNED FALSE.");
            GameObject.Find("DockingFunctions").GetComponent<ReturnShipClass>().ReturnShipButton(false);
        }
    }
}
