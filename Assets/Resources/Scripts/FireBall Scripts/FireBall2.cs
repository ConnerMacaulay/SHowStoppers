﻿using UnityEngine;
using System.Collections;

public class FireBall2 : MonoBehaviour {

	public AudioClip thro;

	Player2 player;
	public GameObject ballStateObj;
	public BallCollisions ballState;
	
	public GameObject PrefabManager;
	public PrefabManager prefabManagerScript;
	
	public GameObject ballPrefab;
	
	public GameObject BallSourceObj;
	public BallForce ballForceScript;
	
	Vector2 pos;
	
	public GameObject gameObject = null;
	
	// Use this for initialization
	void Start () 
	{
		player = GetComponent<Player2>();
		BallSourceObj = GameObject.Find ("BallSource");
		ballForceScript = BallSourceObj.GetComponent<BallForce>();
		
		ballStateObj = GameObject.Find ("BallState");
		ballState = ballStateObj.GetComponent<BallCollisions>();
		
		PrefabManager = GameObject.Find ("PrefabManager");
		prefabManagerScript = PrefabManager.GetComponent<PrefabManager>();
		
		ballPrefab = prefabManagerScript.Prefabs [0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		pos.x = player.myPos.x +1.0f;
		pos.y = player.myPos.y +1.0f;
		
		if (player.hasBall == true) 
		{
			pos.x = player.myPos.x +0.0f;
			pos.y = player.myPos.y +1.2f;
			
			if (player.hasBall == true) 
			{
				if (player.facingRight == true)
				{
					ballForceScript.fireSpeed = 4000;
				}
				if (player.facingRight == false)
				{
					ballForceScript.fireSpeed = -4000;
				}
				
				if (Input.GetButtonDown ("BButton_P3") || Input.GetButtonDown ("BallButton")) 
				{
					audio.clip = thro;
					audio.Play();
					ballState.lastHad = 3;
					GameObject Ball = Instantiate (prefabManagerScript.Prefabs[0]/*call from array her*/, pos, Quaternion.identity) as GameObject;
					Ball.name = "Ball";
					player.hasBall = false;
				}
			}
		}
	}
}