﻿using UnityEngine;
using System.Collections;

public class playerControl2 : MonoBehaviour 
{

	// Check in Unity to change this speed
	public int moveSpeed;
	public GameObject graphics;

	Rigidbody2D rb2D;	

	void Start () 
	{
		rb2D = GetComponent<Rigidbody2D> ();
	}

	void Update () 
	{
		Vector2 moveDir = new Vector2 (Input.GetAxisRaw ("player2") * moveSpeed, rb2D.velocity.y);
		rb2D.velocity = moveDir;


		// Depending on which way the tank is going this changes the direction of the sprite.
		if (Input.GetAxisRaw ("player2") == 1) 
		{
			transform.localScale = new Vector3 (1, 1, 1);
		}
		else if(Input.GetAxisRaw("player2") == -1)
		{
			transform.localScale = new Vector3 (-1, 1, 1);
		}
		if (Input.GetAxisRaw ("player2") != 0) 
		{
			graphics.GetComponent<Animator> ().SetBool ("isMoving2", true);
		} 
		else 
		{
			graphics.GetComponent<Animator> ().SetBool ("isMoving2", false);
		}
	}
}