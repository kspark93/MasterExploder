using UnityEngine;
using System.Collections;

public class tankBarrelPlayer1Movement : MonoBehaviour {

	// Use this for initialization
	float tankBarrel1Speed = 50;
	public GameObject pivotPoint;
	//float rotation = 0;

	void Start () {
	
	}





	// Update is called once per frame
	void Update () 
	{

		// Check input keys and verify that the w key has been hit
		if (Input.GetKey ("w"))
			rotateUp ();



		// Check input keys and verify that the s key has been hit. 
		else if (Input.GetKey ("s"))
			rotateDown ();

	}




		// Rotate up 

	void rotateUp()
	{

		// Attempting to come up with code that will allow us to check if the rotation of the tank has gone past a certain angle. Still figuring this out.
		

			transform.Rotate (Vector3.forward * (Time.deltaTime * tankBarrel1Speed));
		
	}



	// Rotate down.
	void rotateDown()
	{
		transform.Rotate (Vector3.back * (Time.deltaTime * tankBarrel1Speed));

	}





}
