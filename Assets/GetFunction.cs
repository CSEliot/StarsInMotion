using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GetFunction : MonoBehaviour {

    public Text BulletFunction;
    public SpawnShip ShipSpawner;
    private bool GotName;
    private string bulletName;

	// Use this for initialization
	void Start () {
	    GotName = false;
		//bulletName = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipMove>().GetBulletName();
	}
	
	// Update is called once per frame
	void Update () {
		if(ShipSpawner.IsShipSpawned()
		   && GotName == false
		   && GameObject.FindGameObjectWithTag("Ship") != null 
		   && GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipMove>() != null
		){
			bulletName = GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipMove>().GetBulletName();
            BulletFunction.text = bulletName;
            GotName = true;
        }
    }
}

