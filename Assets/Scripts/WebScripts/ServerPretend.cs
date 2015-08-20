using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Reflection;


public class ServerPretend : MonoBehaviour {


    private string url;
	private bool isDownloadFinished;

    void Awake()
    {
        if (Application.isEditor)
        {
             url = "http://52.6.243.86:8082";
        }
        else
        {
             url = "http://52.6.243.86:8083";
        }
        DontDestroyOnLoad(transform.gameObject);
		if (FindObjectsOfType(GetType()).Length > 1){
			Destroy(gameObject);
		}
	}
	
	[Serializable]
    public struct DataTable 
    {
        public double TOTAL_RENT_TIME;

		[Serializable]
        public struct UserProfile
        {
            public string Username;
            public string Password;
            public int Credits;
            public string RentedShipName;
            public string WaitListShipName;
            public DateTime WaitExpireTime; //Says when the waitlisted ship will be given up.
            public DateTime TimeToReturnBy;

            
			public string ToString(){
				return "Username: " + Username +  " Credits: " + Credits + "\n";
			}

			public static int Compare(UserProfile x, UserProfile y){
				return y.Credits.CompareTo (x.Credits);
			}
        }

        public List<UserProfile> UserList;

        public bool NewProfile(string Username, string Password)
        {
            //server must be locked here.
            UserProfile tempProf;
            tempProf.Username = Username;
            tempProf.Password = Password;
            tempProf.Credits = 500;
            tempProf.RentedShipName = "";
            tempProf.WaitListShipName = "";
            tempProf.WaitExpireTime = DateTime.MaxValue;
            tempProf.TimeToReturnBy = DateTime.MinValue;


            
            for (int i = 0; i < UserList.Count; i++ )
            {
                //make sure there are no same usernames
                if (UserList[i].Username == Username)
                {
                    return false;
                }
            }
            UserList.Add(tempProf);
            return true;
        }
        
        //THESE RETURNS MUST BE CAST TO USERPROFILE!!
        public UserProfile? GetProfile(string Username, string Password)
        {
            for (int i = 0; i < UserList.Count; i++)
            {
                //make sure there are no same usernames
                if (UserList[i].Username == Username)
                {
                    if (UserList[i].Password == Password)
                    {
                        return UserList[i];
                    }
                }
            }
            //if no userprofile, return null
            return null;
        }

        public UserProfile? CheatProfile(string Username)
        {
            for (int i = 0; i < UserList.Count; i++)
            {
                //make sure there are no same usernames
                if (UserList[i].Username == Username)
                {
                    //no need to check for password, admin calls.
                    return UserList[i];
                }
            }
            //if no userprofile, return null
            return null;
        }

        public bool WaitListShip(string Username, string ShipName)
        {
			List<string> WaitListedShipsList = GetAllWaitListedShips();
			if(WaitListedShipsList.Contains(ShipName)){
				Debug.Log("Can't waitlist this: " + ShipName + ", it's already rented!!");
				return false;
			}
            //test to make sure we were given a proper UserName
            bool UsernameFound = false;
            int MatchingUserNum = 0;
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].Username == Username)
                {
                    UsernameFound = true;
                    MatchingUserNum = i;
                }
            }
            if (UsernameFound == false) return UsernameFound;

            DateTime OldestWaitEntry = DateTime.MinValue;
            //Search through users and find the user waitlisting this ship that is highest
            for (int i = 0; i < UserList.Count; i++)
            {
				//if person waitlisting target ship.
                if (UserList[i].WaitListShipName == ShipName)
                {
                    //check and see if they are the oldest / most recent to add to wait list.
                    if (UserList[i].WaitExpireTime.CompareTo(OldestWaitEntry) > 0)
                    {
						Debug.Log("User " + Username + " has latest date: " + UserList[i].WaitExpireTime.ToString());
                        OldestWaitEntry = UserList[i].WaitExpireTime;
                    } 
                }
            }
            
            //if the renter is wait list #1, check to see if the ship is rented.
            //if the ship isn' rented and our user hasn't rented a ship, rent the ship
            bool ShipRented = false;
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].RentedShipName == ShipName)
                {
                    ShipRented = true;
                }
            }
            if (ShipRented == false && UserList[MatchingUserNum].RentedShipName == "")
            {
                RentingShip(Username, ShipName);
                Debug.Log("WaitList attempted, Rented instead");
                return false;
            }
            //If the user can't auto-rent the ship, it gets waitlisted:
            else{
				Debug.Log("Ship Currently Waitlisted, renting instead");
				//if no one else had waitlisted this ship . . .
				if(OldestWaitEntry.CompareTo(DateTime.MinValue) == 0){
					OldestWaitEntry = DateTime.Now;
				}
            }

            //WaitList time is last person in waitlist + rent time.
            OldestWaitEntry = OldestWaitEntry.AddHours(TOTAL_RENT_TIME);

            //reassign all stuff to insert into userlists. Structs can't be modified, 
            // only reassigned.
            UserProfile tempProf;
            tempProf.Username = UserList[MatchingUserNum].Username;
            tempProf.Password = UserList[MatchingUserNum].Password;
            tempProf.Credits = UserList[MatchingUserNum].Credits;
            tempProf.TimeToReturnBy = UserList[MatchingUserNum].TimeToReturnBy;
            tempProf.RentedShipName = UserList[MatchingUserNum].RentedShipName;
            tempProf.WaitExpireTime = OldestWaitEntry;
            tempProf.WaitListShipName = ShipName;
            UserList[MatchingUserNum] = tempProf;
            Debug.Log("WaitList attempted, Waitlisting Success: TIme: " + OldestWaitEntry.ToString());
            UpdateWaitLists(Username);
            return true;
        }

        /*
        public bool DropFromWaitList(string Username)
        {
            //test to make sure we were given a proper UserName
            bool UsernameFound = false;
            int MatchingUserNum = 0;
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].Username == Username)
                {
                    UsernameFound = true;
                    MatchingUserNum = i;
                }
            }
            if (UsernameFound == false) return UsernameFound;

            DateTime OldestWaitEntry = DateTime.MinValue;
            //Search through users and find the user waitlisting this ship that is highest
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].WaitListShipName == ShipName)
                {
                    //
                    if (UserList[i].WaitExpireTime.CompareTo(OldestWaitEntry) == 1)
                    {
                        OldestWaitEntry = UserList[i].WaitExpireTime.AddHours(TOTAL_RENT_TIME);
                    }
                }
            }
            //if the renter is wait list #1, check to see if the ship is rented.
            //if the ship isn' rented and our user hasn't rented a ship, rent the ship
            bool ShipNotRented = true;
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].RentedShipName == ShipName)
                {
                    ShipNotRented = false;
                }
            }
            if (ShipNotRented && UserList[MatchingUserNum].RentedShipName == "")
            {
                RentingShip(Username, ShipName);
                Debug.Log("WaitList attempted, Rented instead");
                return false;
            }
            //If the user can't auto-rent the ship, it gets waitlisted:

            //reassign all stuff to insert into userlists. Structs can't be modified, 
            // only reassigned.
            UserProfile tempProf;
            tempProf.Username = UserList[MatchingUserNum].Username;
            tempProf.Password = UserList[MatchingUserNum].Password;
            tempProf.Credits = UserList[MatchingUserNum].Credits;
            tempProf.TimeToReturnBy = UserList[MatchingUserNum].TimeToReturnBy;
            tempProf.RentedShipName = UserList[MatchingUserNum].RentedShipName;
            tempProf.WaitExpireTime = OldestWaitEntry;
            tempProf.WaitListShipName = ShipName;
            UserList[MatchingUserNum] = tempProf;
            Debug.Log("WaitList attempted, Waitlisting Success");
            return true;
        }*/

        public bool UpdateWaitLists(string CurrentUsername)
        {
            //if ship IS rented, check to see if time is up
            //if NOT rented, check to see if 
            
            string ShipTestingName = "Ship ";
            int TotalShipsToCheck = 12;
            int TotalPlayersToCheck = UserList.Count;
            for (int x = 0; x < TotalShipsToCheck; x++ ){
                ShipTestingName = ShipTestingName.Substring(0, 5) + (x+1);
                int ShipRentedBy = 0;
                //Check to see if the ship is rented
                bool ShipIsRented = false;
                for (int y = 0; y < TotalPlayersToCheck; y++)
                {
                    if (UserList[y].RentedShipName == ShipTestingName)
                    {
                        ShipIsRented = true;
                        ShipRentedBy = y;
                    }
                    //Debug.Log("Ship Not Rented: " + ShipTestingName + ", No Match: " 
                      //  + UserList[y].RentedShipName);
                }

                //if the ship is rented
                if(ShipIsRented){
                    bool RentExpired;
                    //is the ship's rent expired?
                    //TimeSpan span = DateTime.Now.Subtract(UserList[ShipRentedBy].TimeToReturnBy);
                    if (DateTime.Now.CompareTo(UserList[ShipRentedBy].TimeToReturnBy) > 0)
                    {
                        //if yes, forcibly de-rent ship.
                        ShipIsRented = false;
                        Debug.Log(UserList[ShipRentedBy].Username + "'s rental has expired!! At: " +
                            UserList[ShipRentedBy].TimeToReturnBy.ToString());
						if(CurrentUsername == UserList[ShipRentedBy].Username){
                            Debug.Log("Returning Ship In Server Script.");
							GameObject.Find("DockingFunctions").GetComponent<ReturnShipClass>().ReturnShipButton(true);
						}
                        UserProfile tempProf;
                        tempProf.Username = UserList[ShipRentedBy].Username;
                        tempProf.Password = UserList[ShipRentedBy].Password;
                        tempProf.Credits = 0;
                        tempProf.WaitListShipName = UserList[ShipRentedBy].WaitListShipName;
                        tempProf.WaitExpireTime = UserList[ShipRentedBy].WaitExpireTime;
                        tempProf.TimeToReturnBy = DateTime.MinValue;
                        tempProf.RentedShipName = "";
                        UserList[ShipRentedBy] = tempProf;
                    }
                } 
                if (ShipIsRented == false)
                {
                    //ship is no longer rented or was never rented.
                    //is the ship waitlisted?
                    List<int> TempWaitList = new List<int>();
                    int WaitUserIndex = -1; //Index of first user in waitlist.
                    DateTime MinWaitList = DateTime.MaxValue;
                    for (int y = 0; y < TotalPlayersToCheck; y++)
                    {
                        if (UserList[y].WaitListShipName == ShipTestingName)
                        {
                            //if yes, is this person earliest in waitlist?
                            if (UserList[y].WaitExpireTime.CompareTo(MinWaitList) < 0)
                            {
                                //finding the earliest (#1 spot) member in the waitlist
                                WaitUserIndex = y;
                                MinWaitList = UserList[y].WaitExpireTime;
                                TempWaitList.Add(WaitUserIndex);
                            }
                        }
                    }
                    //check if we found a waitlist spot #1
                    if (WaitUserIndex != -1 && MinWaitList.CompareTo(DateTime.MaxValue) != 0)
                    {
                        //check to see if they are renting a ship
                        if (UserList[WaitUserIndex].RentedShipName != "")
                        {
                            //if this player's rent HAS expired
                            //TimeSpan span = DateTime.Now.Subtract(UserList[WaitUserIndex].TimeToReturnBy);
                            if (DateTime.Now.CompareTo(UserList[ShipRentedBy].TimeToReturnBy) > 0)
                            {
                                //forcibly return their ship and . . .
                                Debug.Log(UserList[WaitUserIndex].Username + "'s rental has expired!! At: " +
                                    UserList[WaitUserIndex].TimeToReturnBy.ToString());
                                UserProfile tempProf;
                                tempProf.Username = UserList[WaitUserIndex].Username;
                                tempProf.Password = UserList[WaitUserIndex].Password;
                                tempProf.Credits = 0;
                                //forcibly make them rent their #1 spot on waitlist
                                tempProf.WaitListShipName = "";
                                if (CurrentUsername == UserList[WaitUserIndex].Username){
                                    tempProf.TimeToReturnBy = DateTime.Now.AddHours(TOTAL_RENT_TIME);
                                }
                                else{
                                    tempProf.TimeToReturnBy = UserList[WaitUserIndex].WaitExpireTime;
                                }
                                tempProf.WaitExpireTime = DateTime.MaxValue;
                                tempProf.RentedShipName = UserList[WaitUserIndex].WaitListShipName;
                                UserList[WaitUserIndex] = tempProf;
                            }
                            //if NOT expired, do nothing.
                        }
                        else
                        {
                            //player hasn't rented a ship, make them auto-rent their waitlist ship
                            Debug.Log("Making user " + UserList[WaitUserIndex].Username + " Rent wait ship: " +
                                    UserList[WaitUserIndex].WaitListShipName);
                            string ShipToRent = UserList[WaitUserIndex].WaitListShipName;

                            if (CurrentUsername == UserList[ShipRentedBy].Username)
                            {
                                Player CurrentPlayer = GameObject.Find("Player").GetComponent<Player>();
                                ShipTracking Station = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>();

                                //client returns their ship.
                                CurrentPlayer.SetShipReturned();
                                //docking station GUI returns the ship.
                                GameObject.Find("DockingFunctions").GetComponent<ReturnShipClass>().ReturnShipButton(true);
                                //station clears it's waitlist.
                                Station.RemoveShipFromWaitList(); //Also calls client's remove from waitlist

                                Station.ShipRented(ShipToRent, true);
                                CurrentPlayer.SetShipRented(ShipToRent, Station.GetSelectedShip(), false);
                            }

                            UserProfile tempProf;
                            tempProf.Username = UserList[WaitUserIndex].Username;
                            tempProf.Password = UserList[WaitUserIndex].Password;
                            tempProf.Credits = UserList[WaitUserIndex].Credits;
                            //forcibly make them rent their #1 spot on waitlist
                            tempProf.WaitListShipName = "";
                            if (CurrentUsername == UserList[WaitUserIndex].Username)
                            {
                                tempProf.TimeToReturnBy = DateTime.Now.AddHours(TOTAL_RENT_TIME);
                            }
                            else { tempProf.TimeToReturnBy = UserList[WaitUserIndex].WaitExpireTime; }
                            tempProf.WaitExpireTime = DateTime.MaxValue;
                            tempProf.RentedShipName = UserList[WaitUserIndex].WaitListShipName;
                            UserList[WaitUserIndex] = tempProf;
                        }
                    }
                }
            }
			GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().UpdateDockingStation(true);
            return true;
        }


		public bool RentingShip(string Username, string ShipName)
		{
			List<string> RentedShipsList = GetAllRentedShips();
			if(RentedShipsList.Contains(ShipName)){
				Debug.Log("Can't rent this: " + ShipName + ", it's already rented!!");
				return false;
			}
			
			///CHANGE TO NEW THING!!
			UserProfile tempProf;
			
			for (int i = 0; i < UserList.Count; i++)
			{
				//find matching username. 
				if (UserList[i].Username == Username)
				{   
					tempProf.Username = UserList[i].Username;
					tempProf.Password = UserList[i].Password;
					tempProf.Credits = UserList[i].Credits;
                    tempProf.WaitListShipName = UserList[i].WaitListShipName;
                    tempProf.WaitExpireTime = UserList[i].WaitExpireTime;
                    tempProf.TimeToReturnBy = DateTime.Now.AddHours(TOTAL_RENT_TIME);
					tempProf.RentedShipName = ShipName;
					UserList[i] = tempProf;
					return true;
				}
			}
			//if not found, return false.
			return false;
		}
		
		public bool ReturningShip(string Username)
		{
			///CHANGE TO NEW THING!!
			
			UserProfile tempProf;
			
			for (int i = 0; i < UserList.Count; i++)
			{
				//find matching username. 
				if (UserList[i].Username == Username)
				{   
                    //TimeSpan span = DateTime.Now.Subtract(UserList[i].TimeToReturnBy);
                    if (DateTime.Now.CompareTo(UserList[i].TimeToReturnBy) > 0)
                    {
                        Debug.Log("LATE RETURN BY USER: " + Username);
                        tempProf.Credits = 0;
                        GameObject.Find("Player").GetComponent<Player>().SetCredits(0);
                    }
                    else
                    { 
					    tempProf.Credits = UserList[i].Credits;
                        TimeSpan buffer = UserList[i].TimeToReturnBy.Subtract(DateTime.Now);
                        //if user returned EARLY add that time to everyone on the waitlist
                        Debug.Log("User " + UserList[i].Username + "Had " + buffer.TotalHours
                            + "Hours remaining to turn their ship in.");
                        for (int x = 0; x < UserList.Count; x++)
                        {
                            if(UserList[x].WaitListShipName == UserList[i].WaitListShipName && x != i
							   && UserList[x].WaitListShipName != "")
                            {
                                //UserList X is on the waitlist for the 
                                //same ship, add time difference to theirs.
								Debug.Log("USer: "+UserList[x].Username+" is waitlisted for the same ship: "+UserList[x].WaitListShipName +
								          " as User: " +UserList[x]+ "With ship: "+UserList[x].WaitListShipName+", adding buffer: "+
								          buffer.TotalHours);
                                UserList[x].WaitExpireTime.AddHours(buffer.TotalHours);
                            }
                        }

                    }

                    
					tempProf.Username = UserList[i].Username;
					tempProf.Password = UserList[i].Password;
                    tempProf.WaitListShipName = UserList[i].WaitListShipName;
                    tempProf.WaitExpireTime = UserList[i].WaitExpireTime;
                    tempProf.TimeToReturnBy = DateTime.MinValue;
					tempProf.RentedShipName = "";
					UserList[i] = tempProf;
                    Debug.Log("Ship Successfully Returned.");
					return true;
				}
			}
            UpdateWaitLists(Username); //this should make a player auto-rent their waitlisted ship.
			//if not found, return false.
			return false;
		}

        public bool UpdateCredits(string Username, int NewCredits)
        {
			///CHANGE TO NEW THING!!
        
			UserProfile tempProf;
        
            for (int i = 0; i < UserList.Count; i++)
            {
                //find matching username. 
                if (UserList[i].Username == Username)
                {   
					tempProf.Username = UserList[i].Username;
					tempProf.Password = UserList[i].Password;
					tempProf.RentedShipName = UserList[i].RentedShipName;
                    tempProf.WaitListShipName = UserList[i].WaitListShipName;
                    tempProf.WaitExpireTime = UserList[i].WaitExpireTime;
                    tempProf.TimeToReturnBy = UserList[i].TimeToReturnBy;
					tempProf.Credits = NewCredits;
                    UserList[i] = tempProf;
                    return true;
                }
            }
            //if not found, return false.
            return false;
        }

        public bool UpdatePassword(string Username, string NewPassword)
        {
        
			UserProfile tempProf;
			
            for (int i = 0; i < UserList.Count; i++)
            {
                //find matching username. 
                if (UserList[i].Username == Username)
                {
					tempProf.Username = UserList[i].Username;
					tempProf.Credits = UserList[i].Credits;
					tempProf.RentedShipName = UserList[i].RentedShipName;
                    tempProf.WaitListShipName = UserList[i].WaitListShipName;
					tempProf.Password = NewPassword;
                    tempProf.WaitExpireTime = UserList[i].WaitExpireTime;
                    tempProf.TimeToReturnBy = UserList[i].TimeToReturnBy;
					UserList[i] = tempProf;
                    return true;
                }
            }
            //if not found, return false.
            return false;
        }
        
        
		

        public int GetMinutesRemaining(string Username)
        {
            for (int i = 0; i < UserList.Count; i++)
            {
                //find matching username. 
                if (UserList[i].Username == Username)
                {
                    TimeSpan span = DateTime.Now.Subtract(UserList[i].TimeToReturnBy);
                    return Convert.ToInt32(span.TotalMinutes);
                }
            }
            return -1; // if username not found.
        }

        public List<string> GetAllRentedShips()
        {
            List<string> tempList = new List<string>();
            for (int i = 0; i < UserList.Count; i++)
            {
                if (UserList[i].RentedShipName != "")
                {
                    if(tempList.Contains(UserList[i].RentedShipName)){
                        Debug.LogError("TWO OF SAME SHIP NAME BEING RENTED!!");
                    }
                    tempList.Add(UserList[i].RentedShipName);
                }
            }
            return tempList;
        }
        
		public List<string> GetAllWaitListedShips()
		{
			List<string> tempList = new List<string>();
			for (int i = 0; i < UserList.Count; i++)
			{
				if (UserList[i].WaitListShipName != "")
				{
					if(tempList.Contains(UserList[i].WaitListShipName)){
						Debug.LogError("TWO OF SAME SHIP NAME BEING RENTED!!");
					}
					tempList.Add(UserList[i].WaitListShipName);
				}
			}
			return tempList;
		}
		
		public string SearchProfiles(string search){
			string results = "";
			for (int i = 0; i < UserList.Count; i++) {
				if(UserList[i].Username.ToUpper().Contains(search.ToUpper ()))
					results = results + UserList[i].ToString ();
			}
			if (results.Equals (""))
				results = "No Matches Found";
			return results;
		}
		public void PrintProfiles(){
			for (int i = 0; i < UserList.Count; i++){
				Debug.Log(UserList[i].ToString());
			}
		}
		public void PrintTopTen(){
			UserList.Sort (UserProfile.Compare);
			for (int i = 0; i < UserList.Count; i++){
				Debug.Log(UserList[i].ToString());
			}
		}
		
		public string ReturnTopTenUsers(){
			UserList.Sort (UserProfile.Compare);
			string names = "";
			int max = 10;
			if (UserList.Count < 10) {
				max = UserList.Count;
			}
			for (int i = 0; i < max; i++){
				names = names + UserList[i].Username + "\n";
			}
			return names;
		}
		public string ReturnTopTenScores(){
			UserList.Sort (UserProfile.Compare);
			string points = "";
			int max = 10;
			if (UserList.Count < 10) {
				max = UserList.Count;
			}
			for (int i = 0; i < max; i++){
				points = points + UserList[i].Credits + "\n";
			}
			return points;
		}
	}

	public bool WaitListShip(string CurrentUser, string ShipName){
        // GET() is called within UpdateWaitLists
		UpdateWaitLists(CurrentUser);

        bool WaitListSuccess = MyTable.WaitListShip(CurrentUser, ShipName);
        POST();
		return WaitListSuccess;
	}

    
	public string Search(string search){
		return MyTable.SearchProfiles (search);
	}

    public List<string> GetRentedShips(string CurrentPlayer)
    {
        UpdateWaitLists(CurrentPlayer);
        return MyTable.GetAllRentedShips();
    }
    
	public List<string> GetRentedShips2(string CurrentPlayer)
	{
		return MyTable.GetAllRentedShips();
	}


	public List<string> GetWaitListedShips(string CurrentPlayer){
        // GET() is called in UpdateWaitLists()
		UpdateWaitLists(CurrentPlayer);

		return MyTable.GetAllWaitListedShips();
	}

    public int GetMinutesRemaining(string Username)
    {
        return MyTable.GetMinutesRemaining(Username);
    }
    
    public DataTable.UserProfile? GetProfile(string Username, string Password)
    {
        return MyTable.GetProfile(Username, Password);
    }

    public DataTable.UserProfile? CheatProfile(string Username)
    {
        return MyTable.CheatProfile(Username);
    }

    private DataTable MyTable;
    
    
    public bool NewProfile(string Username, string Password)
    {
        GET();
        ServerLocked = true;
        Debug.Log("Locking: New Profile");
        bool NewProfileSuccess = MyTable.NewProfile(Username, Password);
        if (NewProfileSuccess) { POST(); }
        return NewProfileSuccess;
    }


    /// <summary>
    /// This function should only be called when: 
    /// A. We enter the docking station.
    /// B. Player blows their ship up.
    /// C. Player runs out of time.
    /// </summary>
    /// <param name="Username"></param>
    /// <param name="Credits"></param>
    /// <returns>If update was successful.</returns>
    public bool UpdateCredits(string Username, int Credits)
    {


        GET();
        ServerLocked = true;
        Debug.Log("Locking: Update Credits");
        bool UpdateSuccess = MyTable.UpdateCredits(Username, Credits);
        POST();
        return UpdateSuccess;

    }

    public bool ReturnShip(ref Player CurrentPlayer)
    {
        Debug.Log("Telling Player to Return");
        CurrentPlayer.SetShipReturned();
        //Before we write to the server, we make sure we
        //have the latest server.
        GET();
        ServerLocked = true;
        Debug.Log("Locking: Returning Ship");
        Debug.Log("Telling Server to Return");
        bool ReturnSuccess = MyTable.ReturningShip(CurrentPlayer.GetUserName());
		UpdateWaitLists(CurrentPlayer.GetUserName());

        //Uploading to Server.
        POST();

        return ReturnSuccess;
    }

    public bool RentingShip(string Username, string ShipName)
    {
        GET();
        ServerLocked = true;
        Debug.Log("Locking: Rent Ship");
        UpdateWaitLists(Username);
        bool RentSuccess = MyTable.RentingShip(Username, ShipName);
        POST();
        return RentSuccess;
    }

    public bool UpdateWaitLists(string CurrentPlayer)
    {
        GET();
        ServerLocked = true;
        Debug.Log("Locking: Update WaitList");
        bool UpdateSuccess = MyTable.UpdateWaitLists(CurrentPlayer);
        POST();
        return UpdateSuccess;
    }

    //public bool SearchByShipRented()
    /// <summary>
    /// This function is not currently in use.
    /// </summary>
    /// <param name="Username"></param>
    /// <param name="Password"></param>
    /// <returns></returns>
    public bool UpdatePassword(string Username, string Password)
    {
        return MyTable.UpdatePassword(Username, Password);
    }

    private bool ServerLocked;
    public Queue<int> coroutineQueue = new Queue<int>();



	// Use this for initialization
	void Start () 
    {
        //coroutineQueue = new Queue<int>();
        ServerLocked = false;
		isDownloadFinished = true;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Use of P for testing only!!");
            TEST_POST();
        }
	}

    public void EmptyDatabase()
    {
        MyTable = new DataTable();
        MyTable.TOTAL_RENT_TIME = 48; //hours
        MyTable.UserList = new List<DataTable.UserProfile>();
        Debug.Log("New Database Made: Size: " + MyTable.UserList.Count);
        POST();
        GET();
    }

    public void GET()
    {
        StartCoroutine(GetHelper()); //calls load_values
    }

    private void TEST_POST()
    {
        GET();
		byte[] bytesToUpload = Save_Values();
        WWWForm form = new WWWForm();
        string base64 = Convert.ToBase64String(bytesToUpload);
        form.AddField("foo", base64);
        ServerLocked = true;
		StartCoroutine(PostHelper());
        Debug.Log("First push done.");
        GET();
        bytesToUpload = Save_Values();
        form = new WWWForm();
        base64 = Convert.ToBase64String(bytesToUpload);
        form.AddField("foo", base64);
        ServerLocked = true;
        StartCoroutine(PostHelper());
        Debug.Log("Second push done.");
        GET();
    }

    private void POST()
    {
        StartCoroutine(PostHelper());
    }

    private byte[] Save_Values()
    {
        //Debug.Log("Attempting to save. . . ");
        IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream("Table.bin", FileMode.Create, FileAccess.Write, FileShare.None);
        //formatter.Serialize(stream, MyTable);
        //stream.Close();
		byte[] bytes;
		using(MemoryStream stream = new MemoryStream())
		{
			formatter.Serialize(stream, MyTable);
			bytes = new byte[stream.Length];
			stream.Position = 0;
			stream.Read(bytes, 0, (int)stream.Length);
		}
		//Debug.Log("Saving Database likely successful");
		return bytes;
    }

    private void Load_Values(WWW download)
    {
		//convert the downloaded text back into bytes
		byte[] bytes = Convert.FromBase64String(download.text);
		
        IFormatter formatter = new BinaryFormatter();
		try
		{
			Byte[] content = bytes;
			using (MemoryStream ms = new MemoryStream(content))
			{
				MyTable = (DataTable)formatter.Deserialize(ms); 
				//Debug.Log("Uploaded file is: " + MyTable.textFile);                       
			}
			//ServerText.text = MyTable.textFile;
		}
		catch (Exception ex)
		{
			Debug.Log("Exception: " + ex.Message);
			Debug.Log("Stack Trace: " + ex.StackTrace);
		}
		Debug.Log("Loading Database likely successful");
    }

	public bool getDownloadStatus()
	{
		return isDownloadFinished;
	}
	
	public void PrintAll()
	{
		MyTable.PrintTopTen();
	}

	public string TopNames(){
		return MyTable.ReturnTopTenUsers ();
	}
	public string TopScores(){
		return MyTable.ReturnTopTenScores ();
	}

    IEnumerator GetHelper()
    {
        int myCoroutineFrame = Time.frameCount;
        coroutineQueue.Enqueue(myCoroutineFrame);

        while (coroutineQueue.Peek() != myCoroutineFrame)
        {
            yield return null;
        }
        /*
        while (ServerLocked)
        {
            Debug.Log("ServerLocked. . . Waiting ");
            yield return new WaitForSeconds(.1f);
        }*/

        // Create a download object
        WWW download = new WWW(url+"/bar");
        
        // Wait until the download is done
        yield return download;

		//Debug.Log("Downloading . . ." + download.progress);
        if (!string.IsNullOrEmpty(download.error))
        {
            Debug.Log("Error downloading: " + download.error);
        }
        else
        {
            Load_Values(download);
            // show the highscores
            Debug.Log("Downloaded data is done? " + download.isDone);

            //MyTable.textFile = (string)download.text;
        }
        coroutineQueue.Dequeue();
    }
    
    IEnumerator PostHelper()
    {
        int myCoroutineFrame = Time.frameCount;
        coroutineQueue.Enqueue(myCoroutineFrame);

        while (coroutineQueue.Peek() != myCoroutineFrame)
        {
            yield return null;
        }

        byte[] bytesToUpload = Save_Values();
        WWWForm form = new WWWForm();

        string base64 = Convert.ToBase64String(bytesToUpload);

        form.AddField("foo", base64);

        WWW upload = new WWW(url+"/foo", form);
        Debug.Log("UPLOAD INFO: " + upload.isDone);
        yield return upload;
        Debug.Log("Results are: \n-"+upload.text+"\n-"+upload.responseHeaders);
        //Debug.Log("After-Yield Point");
		if (!string.IsNullOrEmpty(upload.error)) {
			Debug.Log("Error Uploading: " + upload.error);
            ServerLocked = false;
		}
		else {
			Debug.Log("Finished Uploading File.");
            ServerLocked = false;
		}
        coroutineQueue.Dequeue();
    }
}
