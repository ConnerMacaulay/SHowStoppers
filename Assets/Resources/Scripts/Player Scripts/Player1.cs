﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PlayerController))]
public class Player1 : MonoBehaviour {
	
	public GameObject ball;
	public GameObject hit2;
	public GameObject World;
	public WorldSettings worldS;
	
	public AudioClip jump;
	public AudioClip hit;
	public AudioClip collect;
	
	public GameObject ballStateObj;
	public BallCollisions ballState;
	
	public FireBall1 fireBallScript;
	
	public Vector2 myPos;
	public float facing;
	public bool facingRight;
	public bool dying;
	public bool hasBall;
	
	public int playerNo = 2;
	
	public int playerHealth = 3;
	public bool playerStatus = true;
	
	public float movespeed; //movemetn speed of the player
	float gravity; //= 2*jumpHeight/timeToJumpApex^2
	float jumpVelocity; // = gravity * timeToJumpApex
	
	public float jumpHeight = 4; //The height of the players jump
	public float timeToJumpApex = .4f; //How long it takes to get to the max height of the jump
	
	public float accelerationTimeAirborne = .2F; //How quickly the player can change direction in the air
	public float accelerationTimeGrounded = .1F; //How quickly the player can change direction on the ground
	float velocityXSmoothing;
	
	Vector3 velocity;
	
	PlayerController controller; //Links the PlayerController script
	
	// Use this for initialization
	void Start () 
	{
		hasBall = false;
		
		fireBallScript = GetComponent<FireBall1>();
		
		World = GameObject.Find ("World");
		worldS = World.GetComponent<WorldSettings>();
		
		ballState = ballStateObj.GetComponent<BallCollisions>();
		controller = GetComponent<PlayerController> (); //Links the PlayerController script
		ballStateObj = GameObject.Find ("BallState");
		
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2); //calculates the gravity of  
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex; //Calculates how quickly the player jumps
		print ("Gravity: " + gravity + "Jump Velocity: " + jumpVelocity); //Prints the details of the calculations
	}
	
	// Update is called once per frame
	void Update () 
	{
		myPos = transform.position;		
		
		if (controller.collisions.above || controller.collisions.below) //Checks if there is any collisions above or below the player
		{
			velocity.y = 0; 
		}
		
		Vector2 input = new Vector2 (Input.GetAxisRaw ("LeftJoystickHorizontal_P2"), Input.GetAxisRaw ("LeftJoystickVertical_P2")); //Links the left joystick of controller 1 to this player
		
		if (Input.GetButtonDown ("AButton_P2") && controller.collisions.below) //Checks that the player is on the ground and that the A button on controller 1 was pressed
		{
			velocity.y = jumpVelocity; //Sets the upwards velocity to the jump velocity
			
			audio.clip = jump;
			audio.Play();
		}
		
		float targetVelocityX = input.x * movespeed; //Sets the movement in the X axis equal to the valuse from the left joystick * the movement speed
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded : accelerationTimeAirborne); //Creates a smooth trancistion when chnaging directions
		velocity.y += gravity * Time.deltaTime; // sets the velocity in the Y direction to be gravity * delta time plus the inital y value
		controller.Move (velocity * Time.deltaTime); //passes the velocity * deltatime value to the move class in controller
		facing = input.x;
		
		if (facing <= 0) {
			facingRight = false;
		}
		else
		{
			facingRight = true;
		}
		
		if (playerHealth <= 0) 
		{
			gameObject.SetActive(false);
		}
		
		if (gameObject.activeSelf) 
		{
			playerStatus = true;
		}
		else 
		{
			playerStatus = false;
			print (playerStatus);
		}
		
		if(hasBall == true)
		{
			ball.SetActive(true);
		}
		else
		{
			ball.SetActive(false);
		}
	}
	
	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "ball" && ballState.pickUp == false && ballState.lastHad != 2) 
		{
			dying = true;
			StartCoroutine(Death());
		}
		if (other.gameObject.tag == "ball" && ballState.pickUp == true && dying == false)
		{
			audio.clip = collect;
			audio.Play();
			hasBall = true;
			ballState.gotBall = true;
			ballState.pickUp = false;
			Destroy(other.gameObject);
			return;
		}
	}
	
	IEnumerator Death()
	{
		movespeed = 0;
		audio.clip = hit;
		audio.Play();
		hit2.SetActive(true);
		yield return new WaitForSeconds(0.9f);
		-- playerHealth;
		dying = false;
	}
}
