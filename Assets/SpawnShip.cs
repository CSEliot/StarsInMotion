using UnityEngine;
using System.Collections;

public class SpawnShip : MonoBehaviour {

    private bool ShipSpawned = false;
	public Object TheShip;

	// Use this for initialization
	void Start () {
        ShipSpawned = false;
		TheShip = Instantiate(GameObject.Find("Player").GetComponent<Player>().GetSelectedShip(), transform.position, transform.rotation);
		ShipSpawned = true;
        
        if (TheShip != null)
        {
            Debug.Log("SHIP SPAWNING LIKELY SUCCESSFUL");
        }
        else
        {
            Debug.Log("SHIP SPAWNING un-SUCCESSFUL");
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    public bool IsShipSpawned()
    {
        return ShipSpawned;
    }
}
