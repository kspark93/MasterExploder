using UnityEngine;
using System.Collections;

public class tankShellBehaviour : MonoBehaviour {
	public float leftBound;
	public float rightBound;
	public float downBound;
	// set from playerShooting.cs
	public GameObject owner;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// destroy the tank shell if it is outside the playing bounds (to left, right or down)
		if (gameObject.transform.position.x < leftBound || gameObject.transform.position.x > rightBound || gameObject.transform.position.y < downBound)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<playerHealth> () != null) 
		{
    		col.gameObject.GetComponent<playerHealth> ().currentPlayerHealth -= 20;
		}
	}
}
