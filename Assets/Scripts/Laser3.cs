using UnityEngine;
using System.Collections;

public class Laser3 : MonoBehaviour {

	public int Ax;
	public int AxSq;
	public int ASin;
	public int AxCu;
	public int ACos;
	public int ATan;
	public int ALog;
	public float TanMod;
	private Vector2 moveDirection;
	public float moveSpeed;
	private float X; //should x be time, or distance from spawn location?
	private float Y;
	private Vector2 spawnLocation;

    private Rigidbody2D r;
	// Use this for initialization
	void Start () {
		X = 0f;
        	r = GetComponent<Rigidbody2D>();
		spawnLocation.Set(transform.position.x, transform.position.y);
	}
	
	//y = A*x + A*x^2 + A*Sin(x) + A*x^3 + A*Cos(x) + etc.
	// Update is called once per frame.
	void Update () {
		X = Mathf.Abs((spawnLocation - new Vector2(transform.position.x, transform.position.y)).magnitude);
		
		//THE EQUATIOOOOOONS
		Y = 0f;
		//Y = x(X); //this IS y=x
		//--------
		
		moveDirection.Set((transform.up*moveSpeed).x, (transform.up*moveSpeed).y);
		//moveDirection.Set((transform.right*Y).x, (transform.right*Y).y);
		moveDirection += (Vector2)(transform.right*Y);
		//rigidbody2D.MovePosition(rigidbody2D.position+moveDirection*Time.fixedDeltaTime);
		r.MovePosition(r.position+moveDirection*Time.deltaTime);
	}
	
	private float Log(float x){
		return ALog*Mathf.Log(x+0.00000001f);
	}
	
	private float Sq(float x){
		return AxSq*x*x;
	}
	
	private float Cu(float x){
		return AxCu*x*x*x;
	}
	
	private float Sin(float x){
		return ASin*Mathf.Sin(x);
	}
	
	private float Tan(float x){
		return ATan*Mathf.Tan(x*TanMod);
	}
	
	private float x(float x){
		return Ax*x;
	}
	
	private float Cos(float x){
		return ACos*Mathf.Cos(x);
	}
	
	//private float 
}
