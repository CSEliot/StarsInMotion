using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PostLocal : MonoBehaviour {

    public InputField ClientInputField;
    public Text ClientText; 

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SendLocalText()
    {
        ClientText.text = ClientInputField.text;
    }
}
