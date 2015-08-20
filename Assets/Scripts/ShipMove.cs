using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipMove : MonoBehaviour {

	private float moveHori;
	private float moveVert;
	private float rotationSpeed;
	public float speed;
	public float rotSpeed;
	private float targetSpeedHori;
	private float targetSpeedVert;
	private float rotationAngle;
	private Vector3 rotationVector;
	public float thrust;
	public float rotThrust;
	public float bulletLife;
	private Vector3 moveDirection;
	private float moveRot;
	public GameObject LaserBullet1;
	private int RockSpawnCount;
	private int RockSpawnCount2;
	public string title;
	public string description;
	public GameObject[] bullets;

    private Rigidbody r;
    private Transform TransformBg;
    
    private bool Dead;
    
    public GameObject explosion;
	// Use this for initialization
	void Start () {
		Dead = false;
		//Screen.lockCursor = true;
		thrust = 5f;
		moveDirection = new Vector3(0f, 0f, 0f);
        r = GetComponentInChildren<Rigidbody>();
		RockSpawnCount = 0;
		RockSpawnCount2 = 0;
		TransformBg = transform.GetChild(0).GetChild(0).transform;
		rotationVector = new Vector3();
		//NOTE:: trying to have shipmove di the function rename ,...GameObject.Find("")
	}
	
	void Update(){
		moveHori = Input.GetAxisRaw ("Horizontal");
		moveVert = Input.GetAxisRaw ("Vertical");
		moveRot = Input.GetAxisRaw("Horizontal_Rot");
		//Debug.Log("moveRot  is : " + moveRot);
		//Debug.Log("moveHori is : " + moveHori);
		//Debug.Log("moveVert is : " + moveVert);


        ///MOVEMENT-------------------
        if (moveHori != 0f)
        {
            //RockSpawnCount2++;
            //if (RockSpawnCount2 > 90)
            //{
            //    GameObject.Find("Rock Spawner").GetComponent<RockSpawn>().SpawnWaves();
            //    RockSpawnCount2 = 0;
            //}
            //Debug.Log("targetSpeedHori is : " + targetSpeedHori);
            targetSpeedHori = Mathf.Lerp(targetSpeedHori, speed * moveHori, Time.deltaTime * thrust);
        }
        else
        {
            targetSpeedHori = Mathf.Lerp(targetSpeedHori, 0f, Time.deltaTime * thrust);
        }

		if (moveVert != 0f)
		{
			targetSpeedVert = Mathf.Lerp(targetSpeedVert, speed * moveVert, Time.deltaTime * thrust);
		}
		else
		{
			targetSpeedVert = Mathf.Lerp(targetSpeedVert, 0f, Time.deltaTime * thrust);
		}

		moveDirection.Set((transform.up*targetSpeedVert).x, (transform.up*targetSpeedVert).y, 0f);
		//rigidbody2D.MovePosition(rigidbody2D.position+moveDirection*Time.fixedDeltaTime);
		moveDirection += (transform.right*targetSpeedHori) ;
		//moveDirection.Set((transform.right*targetSpeedHori).x, (transform.right*targetSpeedHori).y);
		//Debug.Log("Z rotation is: " + transform.localRotation.eulerAngles.z);
		TransformBg.localRotation = Quaternion.Euler(new Vector3(0f, 0f,-1 * transform.localRotation.eulerAngles.z));
		rotationSpeed = Mathf.Lerp (rotationSpeed, rotSpeed*moveRot, Time.deltaTime*rotThrust);		
		
		rotationAngle = -1 * rotationSpeed; // inverted
		
		///END MOVEMENT------------------
		
		
		///SHOOTING
		if(Input.GetButtonDown("Fire")){
			Debug.Log("Fire Button Pressed!!");
			DestroyObject(Instantiate(LaserBullet1,transform.GetChild(2).position, transform.rotation), bulletLife);   
		}
		
		///END SHOOTING
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(!Dead)
		{
			r.MovePosition(r.position + moveDirection*Time.fixedDeltaTime);
			//Rotation
			
			rotationVector.Set(0, 0, rotationAngle);
			r.MoveRotation(Quaternion.Euler(rotationVector * Time.deltaTime) * r.rotation);
		}
	}

    public string GetBulletName()
    {
        return LaserBullet1.name;
    }
    
    public void Died(){
		if (Dead == false){
			Dead = true;
			Instantiate(explosion, transform.position, transform.rotation);
			GameObject.Find("DockingStationCollider").GetComponent<DockShip>().PlayerDied(true);
			GameObject.Find("Player").GetComponent<Player>().SetHadDied();
			Destroy(gameObject.transform.GetChild(1).gameObject); //Destroy Sprite & collider
		}
    }

	public void setBullet(string bullet){
        //each number times AmpMod is the original default.
		if (bullet == "Bolt") {
			LaserBullet1 = bullets[0];
            float AmpMod = Random.Range(-2f, 2f);
            LaserBullet1.GetComponent<Mover>().speed = 25 * AmpMod;
            string ModString = ("" + AmpMod).Substring(0, 5);
            GameObject.Find("Function").GetComponent<Text>()
                .text = ModString + "*(Y=1) Shot";
			Debug.Log("Equipping bullet");
		}
		else if (bullet == "SinShot") {
			LaserBullet1 = bullets[1];
            float AmpMod = Random.Range(-2f, 2f);
            LaserBullet1.GetComponent<SinMover>().amplitude = 17 * AmpMod;
            string ModString = ("" + AmpMod).Substring(0, 5);
            GameObject.Find("Function").GetComponent<Text>().text = ModString+"*Sin(X) Shot";
			Debug.Log("Equipping bullet");
		}
		else if (bullet == "CosShot") {
			LaserBullet1 = bullets[2];
            float AmpMod = Random.Range(-2f, 2f);
            LaserBullet1.GetComponent<CosMover>().amplitude = 17 * AmpMod;
            string ModString = ("" + AmpMod).Substring(0, 5);
			GameObject.Find("Function").GetComponent<Text>().text = ModString+"*Cos(X) Shot";
			Debug.Log("Equipping bullet");
		}
		else if (bullet == "XShot") {
			LaserBullet1 = bullets[3];
            float AmpMod = Random.Range(-2f, 2f);
            LaserBullet1.GetComponent<XMover>().amplitude = 0.4f * AmpMod;
            string ModString = ("" + AmpMod).Substring(0, 5);
			GameObject.Find("Function").GetComponent<Text>().text = "Y="+ModString+"*X Shot";
			Debug.Log("Equipping bullet");
		}
		else if (bullet == "XSquaredShot") {
			LaserBullet1 = bullets[4];
            float AmpMod = Random.Range(-2f, 2f);
            string ModString = ("" + AmpMod).Substring(0, 5);
            LaserBullet1.GetComponent<XSquaredMover>().amplitude = 25 * AmpMod;
			GameObject.Find("Function").GetComponent<Text>().text = ModString+"*X^2 Shot";
			Debug.Log("Equipping bullet");
		}
	}
}
