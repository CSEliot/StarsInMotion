using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class SignUp : MonoBehaviour {
	public GameObject Username;
	public GameObject Password;
	public GameObject RepeatPassword;	
	public GameObject Window;
	public GameObject Error;
	public Text ErrorMsg;
	private int MAX_CHARACTERS = 9;
	private int MIN_CHARACTERS = 4;
	private Player NewPlayer;
    private ServerPretend Server;

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
	public void Start(){
        Server = GameObject.Find("Server").GetComponent<ServerPretend>();
		//Get game objects that will be used on start
		Error = GameObject.Find ("ErrorWindow");
		Window = GameObject.Find ("MainMenu/Window");
		ErrorMsg = Error.transform.GetChild (0).GetComponent<Text>();
		NewPlayer = GameObject.Find ("Player").GetComponent <Player>();
		//set error window to inactive
		Error.SetActive (false);
	}
	public void Update(){
		if(Input.GetKeyDown("return"))SignUpClicked();
	}
	//ran when "Sign Up" button clicked
	public void SignUpClicked() {
		//get field entries when login pushed
		Username = GameObject.Find ("UsernameField");
		Password = GameObject.Find ("PasswordField");
		RepeatPassword = GameObject.Find ("RepeatPasswordField");

		//get the strings, Password must use InputField.text rather then 
		//Text.text so that it doesnt return ***'s
		string name = Username.GetComponentInChildren<Text> ().text;
		string pass = MD5Hash(Password.GetComponentInChildren<InputField>().text);
		string repeat = MD5Hash(RepeatPassword.GetComponentInChildren<InputField> ().text);
		//Debug.Log ("Username : " + name + "Password: " + pass);
        Debug.Log("Child got text says: " + name);
        Debug.Log("Child got text says: " + pass);
        Debug.Log("Child got text says: " + repeat);

       
		//make sure fields arent empty
		if (name == "" || pass == "" || repeat == "") {
			Error.SetActive (true);
			ErrorMsg.text = "Fields cannot be left empty";
			Window.SetActive (false);
		}
		//Check that name is the correct length
		else if(name.Length > MAX_CHARACTERS || name.Length < MIN_CHARACTERS){
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Username must be between 4 and 9 characters";
		}		
		//make sure password & repeat password match
		else if (repeat != pass) {
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Passwords don't match";
		}
		//put username into database
		else if (Server.NewProfile(name, pass) == true){
            //ServerPretend.DataTable.UserProfile UserProfile = (ServerPretend.DataTable.UserProfile)Server.GetProfile(name, pass);
            NewPlayer.NewStats(name, pass);
            if(name == "Admin"){NewPlayer.SetAdmin();}
            Application.LoadLevel("Docking Station");
        }
        else{
			Error.SetActive (true);
			Window.SetActive (false);
			ErrorMsg.text = "Username is already taken, choose something else";
        }
	}
	
	public void Close(){
		Error.SetActive (false);
		Window.SetActive (true);
	}
	
}
