using UnityEngine;
using System.Collections;

public class changeState : MonoBehaviour 
{

	// this variable is going to store the address of any animationed object you give it 
	Animator anim;

	void Start () 
	{
		anim = GetComponent<Animator>();
	}

	void Update ()
	{
		// handling the idle - firing - idle
		if (Input.GetKeyDown (KeyCode.Space )) 
		{
			anim.SetInteger ("State", 0);
		}
		if (Input.GetKeyUp (KeyCode.Space )) 
		{
			anim.SetInteger ("State", 1);
		}

	}
}
