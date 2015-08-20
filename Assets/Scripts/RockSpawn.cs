using UnityEngine;
using System.Collections;

public class RockSpawn : MonoBehaviour {

	public GameObject[] Asteroids;
	//public ArrayList<GameObject> HandFulAsteroids = new ArrayList<GameObject> ();
	//public GameObject[] HandFulAsteroids;
	Object temp;
	public Vector3 spawnValues;
	float speedX = 5.0f;  //X軸移動速度
	private float x;		//x軸座標值
	float speedY = 5.0f;  //Y軸移動速度  [步驟一]
	private float y;
	float sita;

	public int MaxSize;
	public float AsteroidLifeSpan;
	public float AsteroidSpeedMod;
    private float AsteroidMaxSpeed;
    private float ShipDistanceModifier;

	private Transform ShipTransform;
    private Vector3 StartingLocation;
	private bool ShipSpawned;
    private float MaxDistance;

    private float DistanceTravelled;
	private float SpaceDepth;
	private int DangerLevel;
    public int SpawnDistance;
	
	private Vector3[] SpawnLocs;

	// Use this for initialization
	void Start () {
		SpawnLocs = new Vector3[8]{
		   new Vector3( 0,SpawnDistance, 0),
		   new Vector3(SpawnDistance,SpawnDistance, 0),
		   new Vector3(SpawnDistance, 0, 0),
		   new Vector3(SpawnDistance,-SpawnDistance, 0),
		   new Vector3( 0,-SpawnDistance, 0),
		   new Vector3(-SpawnDistance,-SpawnDistance, 0),
		   new Vector3(-SpawnDistance, 0, 0),
		   new Vector3(-SpawnDistance,SpawnDistance, 0)
		};
		DangerLevel = 1;
		//AsteroidSpeedMod = 20f;

        MaxDistance = 2800f;
        ShipDistanceModifier = 0.1f;
		ShipSpawned = false;
		//Attempt to get Ship reference at startup.
		if(GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().IsShipSpawned()){
			ShipTransform = ((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).transform;
			ShipSpawned = true;
		}
		if(ShipSpawned){
			SpawnWaves ();
		}
        StartingLocation = ShipTransform.position;
        //Start Distance travelled a bit from the start, so that rocks don't spawn IN the station.
        DistanceTravelled = Mathf.Abs((StartingLocation - ShipTransform.position).magnitude) + 15;
	}
	
	void Update(){
        //ShipDistanceModifier = start
		//if we fail to get shipspawned at startup, attempt it here
		if(ShipSpawned == false){
			if(GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().IsShipSpawned()){
				ShipTransform = ((GameObject)GameObject.Find("ShipSpawn").GetComponent<SpawnShip>().TheShip).transform;
				ShipSpawned = true;
				SpawnWaves ();
			}else{
				Debug.LogError("ShipTransform still not got. It should be got.");
			}
		}

        //Every 5 meters travelled from Spawn, Spawn rocks and 
        if (DistanceTravelled + 10f < Mathf.Abs((StartingLocation - ShipTransform.position).magnitude))
        {
            Debug.Log("Distance travelled + 10 < current, spawning rocks.");
            DistanceTravelled = Mathf.Abs((StartingLocation - ShipTransform.position).magnitude);
            SpawnWaves();
        }
	}
	
	public void SpawnWaves (){

		SpaceDepth = (DistanceTravelled / MaxDistance);
        AsteroidMaxSpeed = AsteroidSpeedMod * SpaceDepth;
		DangerLevel = (int)(SpaceDepth / 10) + 1;//Every 10%, increase rock spawn amount
		//Instantiate(GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().GetSelectedShip(), transform.position, Quaternion.Euler(Vector3.zero));
		
		//Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), 
		//                                     Random.Range(-spawnValues.y, spawnValues.y), spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity;

        Debug.Log("Space Depth: " + SpaceDepth + " Asteroid Speed: " + AsteroidMaxSpeed);
		for(int k = 0; k < DangerLevel; k++){
			for(int i = 0; i < 8; i ++){
				temp = Instantiate (Asteroids [Random.Range (0, 3)], 
				ShipTransform.position+SpawnLocs[i], spawnRotation);
				
				Destroy(temp, AsteroidLifeSpan);
				((GameObject)temp).transform.localScale = new Vector3(1F,1F,1F)*MaxSize*Random.Range(0.4f, 1.0f);
				sita = Random.Range(0,360);  //隨機產生0~360度之間的夾角  [步驟二]
                speedX = Mathf.Cos(sita);  //[步驟三]
                speedY = Mathf.Sin(sita);  //利用此夾角算出移動的向量, 5是移動的速度
				((GameObject)temp).transform.GetComponent<Rigidbody>().velocity = 
					new Vector3(speedX, speedY, 0F).normalized * AsteroidMaxSpeed;
			} 
		}
	}

}
