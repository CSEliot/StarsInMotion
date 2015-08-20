using UnityEngine;
using System.Collections;

public class Laser2 : MonoBehaviour {

	public float Ax;
	public float AxSq;
	public float ASin;
	public float AxCu;
	public float ACos;
	public float ATan;
	public float ALog;
	public float sinMov;
	public float sqMod = 0f;
	public float cuMod = 0f;
	public float xMod = 0f;
	public float logMod = 0f;
	public float sinMod = 0f;
	public float cosMod = 0f;
	public float tanMod = 0f;
	private Vector2 moveDirection;
	public float moveSpeed;
	private float X; //should x be time, or distance from spawn location?
	private float Y;
	private Vector2 spawnLocation;
	private float birthTime; 
    private Rigidbody2D r;
    public float globalAmplitudeMod;
	// Use this for initialization
	void Start () {
		
		X = 0f;
		birthTime = 0f;
		r = GetComponent<Rigidbody2D>(); // get a reference to the physical rigidbody
		
		//SpawnLocation is relative to the ship, so if it's [0,0], no matter where the ship moves, [0,0] is always
		// at the center of the ship. 
		birthTime = Time.time;
		spawnLocation.Set(transform.position.x, transform.position.y); //spawnLocation is a 3d vector, & we assign x and y
	}
	
	//y = A*x + A*x^2 + A*Sin(x) + A*x^3 + A*Cos(x) + etc.
	// Update is called once per frame.
	void Update () {
		//distance from ship. It's the Absolute value of the spawn
		X = (Time.time - birthTime)*globalAmplitudeMod;
		//THE EQUATIOOOOOONS
		Y = x(X) + Sin(X) + Cos(X) + Log(X) + Sq (X) + Cu(X) + Tan(X) + 0f;
		//Y = x(X); //this IS y=x
		//--------
		Debug.Log("Y distance is: " + Y);
		
		moveDirection.Set((transform.up*moveSpeed).x, (transform.up*moveSpeed).y);
		//moveDirection.Set((transform.right*Y).x, (transform.right*Y).y);
		moveDirection += (Vector2)(transform.right*Y);
		//rigidbody2D.MovePosition(rigidbody2D.position+moveDirection*Time.fixedDeltaTime);
		r.MovePosition(r.position+moveDirection*Time.deltaTime);
	}
	
	private float Log(float x){
		return ALog*Mathf.Log(x+0.00000001f)*logMod;
	}
	
	private float Sq(float x){
		return AxSq*x*x*sqMod;
	}
	
	private float Cu(float x){
		return AxCu*x*x*x*cuMod;
	}
	
	private float Sin(float x){
		return ASin*Mathf.Sin(x)*sinMod;
	}
	
	private float Tan(float x){
		return ATan*Mathf.Tan(x*0.9f)*tanMod;
	}
	
	private float x(float x){
		return Ax*x*xMod;
	}
	
	private float Cos(float x){
		return ACos*Mathf.Cos(x)*cosMod;
	}
	
	public void toggleCos(){
		cosMod = cosMod==1f? 0f : 1f;
	}
	public void toggleSin(){
		sinMod = sinMod==1f? 0f : 1f;
	}
	public void toggleX(){
		xMod = xMod==1f? 0f : 1f;
	}
	public void toggleLog(){
		logMod = logMod==1f? 0f : 1f;
	}
	public void toggleTan(){
		tanMod = tanMod==1f? 0f : 1f;
	}
	public void toggleCu(){
		cuMod = cuMod==1f? 0f : 1f;
	}
	public void toggleSq(){
		sqMod = sqMod==1f? 0f : 1f;
	} 
	
	//private float 
}
