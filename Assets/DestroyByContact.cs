using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject[] Gift;
	void OnTriggerEnter(Collider other) 
	{
        if (tag == null)
        {
            Debug.Log("THIS OBJECT AINT GOT NO TAG: " + gameObject.name);
        }
        if (other.tag == null)
        {
            Debug.Log("THIS OBJECT AINT GOT NO TAG: " + gameObject.name);
        }
		if(tag != null && other.tag != null && tag == other.tag ){
			
			return;
		}else if(tag == "dropGift" && other.tag == "ShipCollider"){//pick up drop stuff
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject.transform.parent.gameObject);
			Debug.Log("Equipping bulletzz " + gameObject.transform.parent.name);
			if(gameObject.transform.parent.name == "BoltGift(Clone)"){
				((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).GetComponent<ShipMove>().setBullet("Bolt");
			}
			else if(gameObject.transform.parent.name == "SinGift(Clone)"){
				Debug.Log("Equipping bulletzz");
				((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).GetComponent<ShipMove>().setBullet("SinShot");
			}
			else if(gameObject.transform.parent.name == "CosGift(Clone)"){
				Debug.Log("Equipping bulletzz");
				((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).GetComponent<ShipMove>().setBullet("CosShot");
			}
			else if(gameObject.transform.parent.name == "XGift(Clone)"){
				Debug.Log("Equipping bulletzz");
				((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).GetComponent<ShipMove>().setBullet("XShot");
			}
			else if(gameObject.transform.parent.name == "XSquaredGift(Clone)"){
				Debug.Log("Equipping bulletzz");
				((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).GetComponent<ShipMove>().setBullet("XSquaredShot");
			}
		}
		else if( other.tag == "ShipCollider"){
			GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipMove>().Died();
			/*
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(other.gameObject);
			GameObject.Find("DockingStationCollider").GetComponent<DockShip>().AutoDock(true);
			GameObject.FindGameObjectWithTag("Ship").GetComponent<ShipMove>().enabled = false;
			Destroy(gameObject);*/
			Destroy(gameObject);
		}
		else if( tag == "Asteroid"  && other.tag == "Laser"){
			Instantiate(explosion, transform.position, transform.rotation);
			GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddCredits(100);
			Vector3 speedAndDirection = transform.GetComponent<Rigidbody>().velocity;
			if(Random.Range (0, 3)==2){
				Instantiate(Gift[Random.Range(0, Gift.Length)], transform.position, transform.rotation);
			}
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}