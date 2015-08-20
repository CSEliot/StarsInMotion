using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour {

    private string Username;
    private string Password;
    private int Credits;
    private ServerPretend Server;
    private string RentedShipName;
    private string WaitListShipName;
    private DateTime WaitExpireTime; //Says when the waitlisted ship will be given up.
    private DateTime TimeToReturnBy;
    //private string 

    private GameObject RentedShipObject;

    private bool ShipRented;
    private bool ShipWaitlisted;
    private bool IsAdmin;
    private bool HadDied;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
		if (FindObjectsOfType(GetType()).Length > 1){
			Destroy(gameObject);
		} 
	}
	
	// Use this for initialization
	void Start () {
		IsAdmin = false;
		HadDied = false;
		Server = GameObject.Find("Server").GetComponent<ServerPretend>();
	}
	


	// Update is called once per frame
	void Update () {
		
		if(IsAdmin && Input.GetKeyDown("k")){
			Debug.Log("GIVIN MONEY");
			Credits += 100;
		}
		if(IsAdmin && Input.GetKeyDown("l")){
			Debug.Log("TAKIN' MONEY");
			Credits -= 100;
		}
		if(Input.GetKeyDown(";")){
			Debug.Log("Printing all Profiles. . . ");
			Server.PrintAll();
		}
		
		//Die when time runs out, or you collide with a ship.
		if(HadDied && Application.loadedLevelName == "Docking Station"){
			HadDied = false;
            Debug.Log("Player died or time ran out, returning Ship.");
			GameObject.Find("DockingFunctions").GetComponent<ReturnShipClass>().ReturnShipButton(true);
		}
	}
	
	public void SetAdmin(){
		IsAdmin = true;
	}

    public void AddCredits(int AddBy)
    {
        Credits += AddBy;
    }

    public void SetCredits(int NewCredits)
    {
        Credits = NewCredits;
    }

    public GameObject GetSelectedShip()
    {
        return RentedShipObject;
    }

	public void SetHadDied(){
		HadDied = true;
		Credits = 0;
	}

    public void RentalPastDue()
    {
        Credits = 0;
    }

    public void SetShipReturned()
    {
        if (ShipRented == false)
        {
            Debug.Log("PLAYER HAS NO SHIP TO RETURN");
            //Debug.LogError("PLAYER HAS NO SHIP TO RETURN");
        }
        else
        {
            RentedShipName = "";
            ShipRented = false;
            //Server -> Return Ship is called from ReturnShipClass.
            //GameObject.Find("Server").GetComponent<ServerPretend>().ReturnShip(Username);
        }
    }

    public void SetShipRented(string NewShipName, GameObject NewRentedShipObject, bool IsFromStart)
    {
        ServerPretend.DataTable.UserProfile
        UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(Username, Password);
        TimeToReturnBy = UserProfile.TimeToReturnBy;
        RentedShipObject = NewRentedShipObject;
        if (ShipRented && IsFromStart == false)
        {
            Debug.LogError("PLAYER HAS SHIP RENTED: "+RentedShipName+", CANNOT RENT ANOTHER");
        }
        else if(IsFromStart == false)
        {
            Debug.Log("" + NewShipName + "Is being rented.");

            RentedShipName = NewShipName;
            ShipRented = true;
            //GameObject.Find("Server").GetComponent<ServerPretend>().RentingShip(Username, NewShipName);
        }
    }
    
	public void SetShipWaitListed(string NewShipName, bool IsFromStart)
	{
		ServerPretend.DataTable.UserProfile
			UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(Username, Password);
		
        WaitExpireTime = UserProfile.WaitExpireTime;
		if (ShipWaitlisted && IsFromStart == false)
		{
			Debug.LogError("PLAYER HAS SHIP WAITLISTED: "+RentedShipName+", CANNOT WAITLSIT ANOTHER");
		}
		else if(IsFromStart == false)
		{
			Debug.Log("" + NewShipName + "Is being WaitListed");
			
			WaitListShipName = NewShipName;
			ShipWaitlisted = true;
			//GameObject.Find("Server").GetComponent<ServerPretend>().RentingShip(Username, NewShipName);
		}
	}

	public void SetWaitListRemoved(){
		ShipWaitlisted = false;
		WaitExpireTime = DateTime.MinValue;
		WaitListShipName = "";
	}

    public void SetStats(string SetUsername, string SetPassword){
		
        ServerPretend.DataTable.UserProfile 
        UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(SetUsername, SetPassword);
        Username = UserProfile.Username;
        Password = UserProfile.Password;
        Credits = UserProfile.Credits;
        RentedShipName = UserProfile.RentedShipName;
        WaitListShipName = UserProfile.WaitListShipName;
        WaitExpireTime = UserProfile.WaitExpireTime;
        TimeToReturnBy = UserProfile.TimeToReturnBy;

        Debug.Log("TimeToReturnBy : " + TimeToReturnBy.ToString());
        Debug.Log("WaitExpireTime : " + WaitExpireTime.ToString());
        if (RentedShipName == "")
        {
            Debug.Log("On SignIn, ShipRented Marked FALSE");
            ShipRented = false;
        }
        else
        {
            Debug.Log("On SignIn, ShipRented Marked True: " + RentedShipName);
            ShipRented = true;
        }
        if (WaitListShipName == "")
        {
            ShipWaitlisted = false;
        }
        else
        {
            ShipWaitlisted = true;
        }
    }



    public void NewStats(string NewUsername, string NewPassword)
    {
        ServerPretend.DataTable.UserProfile 
        UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(NewUsername, NewPassword);
        Username = UserProfile.Username;
        Password = UserProfile.Password;
        Credits = UserProfile.Credits;
        RentedShipName = UserProfile.RentedShipName;
        WaitListShipName = UserProfile.WaitListShipName;
        WaitExpireTime = UserProfile.WaitExpireTime;
        TimeToReturnBy = UserProfile.TimeToReturnBy;

        if (RentedShipName == "")
        {
            ShipRented = false;
        }
        else
        {
            ShipRented = true;
        }
        if (WaitListShipName == "")
        {
            ShipWaitlisted = false;
        }
        else
        {
            ShipWaitlisted = true;
        }
    }

    public string GetRentalDueDate()
    {
        
        return TimeToReturnBy.ToString();
    }

    public string GetWaitTimeRemaining()
    {
        return WaitExpireTime.ToString();
    }
    
    public int GetCredits()
    {
        return Credits;
    }

    public bool GetRentedStatus()
    {
        return ShipRented;
    }

    public DateTime GetTimeToReturnByObject()
    {
        return TimeToReturnBy;
    }

    public bool GetWaitListStatus()
    {
        return ShipWaitlisted;
    }

    public string GetWaitListedShipName()
    {
        return WaitListShipName;
    }

    public string GetUserName()
    {
        return Username;
    }
	public string GetRentedShipName(){
		return RentedShipName;
	}
    
    public void SaveGame(){
        //SAVE GAME FUNCTION SHOULD NOT BE USED. Code kept just in case.
        Debug.LogError("DO NOT USE SAVEGAME FUNCTION.");
		//ship rental status is always parallel to server rental status,
        // but credits get updated so quickly, we only do it when saving to server.
		/*if(Server.UpdateCredits(Username, Credits) == false){
			Debug.Log("Credit Updating failed! User: " + Username + " Credits: " + Credits);
		}else{
			Debug.Log("Credit Updating Success User: " + Username + " Credits: " + Credits);
		}*/
		
    }
}
