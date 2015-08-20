using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DockError : MonoBehaviour {
	public GameObject Error;
	public GameObject Window;
	public GameObject LeaderboardScreen;
	public GameObject Leaderboard;
	public GameObject Profile;
	public GameObject Buttons;
	public GameObject SearchWindow;
	public GameObject SearchField;
	public Text ProfileMsg;
	private Player NewPlayer;
	private ServerPretend Server;
	private Text LeaderboardNames;
	private Text LeaderboardScores;
	private Text SearchResults;
	public GameObject Hover;
	private Text HoverText;
	public GameObject HowTo;
	private Text HoverTitle;


	//public Text ErrorMsg;
	// Use this for initialization
	void Start () {
		Buttons = GameObject.Find ("Buttons");
		Error = GameObject.Find("ErrorWindow");
		Window = GameObject.Find("MainScreen");
		LeaderboardScreen = GameObject.Find ("LeaderboardScreen");
		Profile = GameObject.Find ("UserProfile");
		Leaderboard = GameObject.Find ("LeaderboardScreen/Leaderboard");
		SearchWindow = GameObject.Find ("SearchResults");
		Hover = GameObject.Find ("HoverAttributes");
		HowTo = GameObject.Find ("HowToPlayWindow");
		HoverTitle = Hover.transform.GetChild (0).GetComponent<Text> ();
		HoverText = Hover.transform.GetChild (1).GetComponent<Text>();
		Server = GameObject.Find("Server").GetComponent<ServerPretend>();
		ProfileMsg = Profile.transform.GetChild(2).GetComponent<Text>();
		NewPlayer = GameObject.Find ("Player").GetComponent<Player>();
		LeaderboardNames = LeaderboardScreen.transform.GetChild (0).transform.GetChild (2).GetComponent<Text> ();
		LeaderboardScores = LeaderboardScreen.transform.GetChild (0).transform.GetChild (3).GetComponent<Text> ();
		SearchResults = SearchWindow.transform.GetChild (2).transform.GetChild (0).GetComponent<Text> ();
		//ErrorMsg = Error.transform.GetChild (0).GetComponent<Text>();
		//set error window to inactive
		Error.SetActive (false);
		LeaderboardScreen.SetActive (false);
		Profile.SetActive (false);
		SearchWindow.SetActive (false);
		Hover.SetActive (false);
		HowTo.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeToLeaderboard(){
		Window.SetActive (false);
		LeaderboardScreen.SetActive (true);
		Profile.SetActive (false);
		LeaderboardNames.text = Server.TopNames();
		LeaderboardScores.text = Server.TopScores ();
		/*if(Server.UpdateCredits(NewPlayer.GetUserName (), NewPlayer.GetCredits()) == false){
			Debug.Log("Credit Updating failed! User: " + NewPlayer.GetUserName () + " Credits: " + NewPlayer.GetCredits());
		}else{
			Debug.Log("Credit Updating Success User: " + NewPlayer.GetUserName () + " Credits: " + NewPlayer.GetCredits());
		}*/
	}

	public void ShowProfile(){
		Buttons.SetActive (false);
		Leaderboard.SetActive (false);
		Profile.SetActive (true);
		string type = NewPlayer.GetRentedShipName();
		if (type == null) {
			type = "none";
		}
		ProfileMsg.text = NewPlayer.GetUserName() + "\n" + NewPlayer.GetCredits() + "\n" + type;

	}
	public void Search(){
		SearchField = GameObject.Find ("SearchField");
		string search = SearchField.GetComponentInChildren<InputField> ().text;
		Buttons.SetActive (false);
		Leaderboard.SetActive (false);
		SearchWindow.SetActive (true);
		GameObject.Find ("SearchResults/Title").GetComponentInChildren<Text> ().text = "Search Results For: " + search;
		SearchResults.text = Server.Search (search);
	}

	public void Close(){
		Error.SetActive (false);
		Window.SetActive (true);
	}
	public void BackToDock(){
		LeaderboardScreen.SetActive (false);
		Window.SetActive (true);
	}
	public void BackToLeaderboard(){
		Leaderboard.SetActive (true);
		Buttons.SetActive (true);
		Profile.SetActive (false);
		SearchWindow.SetActive (false);
	}
	public void ShowHowTo(){
		Window.SetActive (false);
		HowTo.SetActive (true);
	}
	public void ExitHowTo(){
		HowTo.SetActive (false);
		Window.SetActive (true);
	}
	public void Ship1Hover(){
		//Make a tostring for the ships to display all their astributes then just have 
		//HoverText.text = Ship1.toString(); or whatever! should have a description of what it does
		//if its currently rented, how long the hold list is if rented and, and who is renting it.
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[0].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[0].GetComponent<ShipMove>().description;
	}	
	public void Ship2Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[1].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[1].GetComponent<ShipMove>().description;
	}	
	public void Ship3Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[2].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[2].GetComponent<ShipMove>().description;
	}	
	public void Ship4Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[3].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[3].GetComponent<ShipMove>().description;
	}
	public void Ship5Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[4].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[4].GetComponent<ShipMove>().description;
	}
	public void Ship6Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[5].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[5].GetComponent<ShipMove>().description;
	}
	public void Ship7Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[6].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[6].GetComponent<ShipMove>().description;
	}
	public void Ship8Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[7].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[7].GetComponent<ShipMove>().description;
	}
	public void Ship9Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[8].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[8].GetComponent<ShipMove>().description;
	}
	public void Ship10Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[9].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[9].GetComponent<ShipMove>().description;
	}
	public void Ship11Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[10].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[10].GetComponent<ShipMove>().description;
	}
	public void Ship12Hover(){
		Hover.SetActive (true);
		HoverTitle.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[11].GetComponent<ShipMove>().title;
		HoverText.text = GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipPrefabs[11].GetComponent<ShipMove>().description;
	}
	public void ExitHover(){
		Hover.SetActive (false);
	}
}


