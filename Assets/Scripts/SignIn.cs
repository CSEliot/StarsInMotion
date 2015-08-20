using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class SignIn : MonoBehaviour {
	public GameObject Username;
	public GameObject Password;
	public GameObject Error;
	public GameObject Window;
	public Text ErrorMsg;
	private int MAX_CHARACTERS = 9;
	private int MIN_CHARACTERS = 4;
    public bool NewDatabaseAndClearOld;

    private ServerPretend Server;

    private int AdminClicks = 0;

	public void Start(){
        Server = GameObject.Find("Server").GetComponent<ServerPretend>();

        if (NewDatabaseAndClearOld)
        {
            Server.EmptyDatabase();
        }
        else
        {
		    Server.GET();
        }
        
		//get all the GameObjects needed on start
		Error = GameObject.Find("ErrorWindow");
		Window = GameObject.Find("MainMenu/Window");
		ErrorMsg = Error.transform.GetChild (0).GetComponent<Text>();
		//set error window to inactive
		Error.SetActive (false);
	}
	
	public void Update(){
		if(Input.GetKeyDown("return"))Login();
	}

	private string MD5Hash(string password){
		System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] bs = System.Text.Encoding.UTF8.GetBytes (password);
		bs = x.ComputeHash (bs);
		System.Text.StringBuilder s = new System.Text.StringBuilder ();
		foreach (byte b in bs) {
			s.Append (b.ToString ("x2").ToLower ());
		}
		return s.ToString ();
	}

	//Called when Login button is pressed
	public void Login() {
        ServerPretend.DataTable.UserProfile UserProfile;
        bool InvalidNameOrPass;
		//get username and password fields and copy their text component to a local variable
		Username = GameObject.Find ("UsernameField");
		Password = GameObject.Find ("PasswordField");
		string name = Username.GetComponentInChildren<Text> ().text;
        Debug.Log("Child got text says: " + name);
		string pass = MD5Hash(Password.GetComponentInChildren<InputField>().text);
        Debug.Log("Child got text says: " + pass);
        if (AdminClicks > 10)
        {
            if (Server.CheatProfile(name) != null)
            {
                UserProfile = (ServerPretend.DataTable.UserProfile)Server.CheatProfile(name);
                pass = UserProfile.Password;
                InvalidNameOrPass = false;
                Debug.Log("Admin logging into someone else's account.");
            }
            else
            {
                //give default profile. Will be empty.
                UserProfile = new ServerPretend.DataTable.UserProfile();
                InvalidNameOrPass = true;
            }
        }

        else if (Server.GetProfile(name, pass) != null)
        {
            UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(name, pass);
			Debug.Log("User Profile Found!! Name: " + UserProfile.Username + " Credits: " + UserProfile.Credits
			          + "Pass: " + UserProfile.Password);
            InvalidNameOrPass = false;
        }
        else
        {
            //give default profile. Will be empty.
            UserProfile = new ServerPretend.DataTable.UserProfile();
            InvalidNameOrPass = true;
        }
		//Debug.Log ("Username : " + name + "Password: " + pass);
		//check if either field is empty
		if (name == "" || pass == "") {
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Username or Password field cannot be left empty";
		}
		//check if usename is correct length
		else if (name.Length > MAX_CHARACTERS || name.Length < MIN_CHARACTERS) {
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Username must be between 4 and 9 characters";
		}
		//check if username is in the database
		else if (InvalidNameOrPass) {
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Username Or Password not valid";
		}
		//login and change to game screen
		else {
            GameObject.Find("Player").GetComponent<Player>().SetStats(name, pass);
			if(name == "Admin"){GameObject.Find("Player").GetComponent<Player>().SetAdmin();}
            Application.LoadLevel("Docking Station");
            return;
		}
	}

	public void Close(){
		Error.SetActive (false);
		Window.SetActive (true);
	}

    public void AddAdminClick()
    {
        AdminClicks += 1;
    }


}
