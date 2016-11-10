using UnityEngine;
using System.Collections;

public class WindAffected : MonoBehaviour {
	public float windMultiplier;
	Rigidbody2D body;
	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 force = new Vector2 ();
		force.x = Mathf.Cos (WindBehaviour.globalWind.arrow.rotation.eulerAngles.z * Mathf.Deg2Rad) * WindBehaviour.globalWind.windSpeed;
		force.y = Mathf.Sin (WindBehaviour.globalWind.arrow.rotation.eulerAngles.z * Mathf.Deg2Rad) * WindBehaviour.globalWind.windSpeed;
		body.AddForce (force * windMultiplier * Time.deltaTime);
	}
}
