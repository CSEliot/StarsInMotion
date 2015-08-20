using UnityEngine;
using System.Collections;
using System;

public class DockShip : MonoBehaviour {

	DateTime TimeDestroyed;
	DateTime TimeReturnToBase;
	bool CountDownBegin;
	bool ShipDestroyed;
	// Use this for initialization
	void Start () {
		ShipDestroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(ShipDestroyed && !CountDownBegin){
			Time.timeScale = 0.2f;
			CountDownBegin = true;
			TimeDestroyed = DateTime.Now;
			TimeReturnToBase = TimeDestroyed.AddSeconds(3);
		}
		if(CountDownBegin){
			//Debug.Log("Countdown remaining: " + TimeReturnToBase.Subtract(DateTime.Now).Seconds);
			//Debug.Log("Now VS Return: " + DateTime.Now.CompareTo(TimeReturnToBase));
			if(DateTime.Now.CompareTo(TimeReturnToBase) > 0){
				Debug.Log("Returning to Base");
				GameObject.Find("Player").GetComponent<Player>().SetCredits(0);
				Time.timeScale = 1.0f;
				Application.LoadLevel("Docking Station");
			}
		}
	}
	
	void OnTriggerEnter(Collider collide){
		if(collide.tag == "ShipCollider" || collide.tag == "Ship"){
			Application.LoadLevel("Docking Station");
		}
	}
	
	public void PlayerDied(bool TheShipDestroyed){
		ShipDestroyed = TheShipDestroyed;
	}
}
