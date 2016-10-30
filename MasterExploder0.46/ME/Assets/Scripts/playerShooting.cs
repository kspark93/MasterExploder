using UnityEngine;
using System.Collections;

public class playerShooting : MonoBehaviour {
	public float currentPower;
	public float minPower;
	public float maxPower;
	public float forcePerSecond;
	public string fireKey;
	public Object tankShellPrefab;
	public GameObject tank;
	public GameObject gunPivot;
	// Use this for initialization
	void Start () {
		currentPower = minPower;
	}
	
	// Update is called once per frame
	void Update () {
		// steadily increase shot power while key is held
		if (Input.GetKey(fireKey) && currentPower < maxPower) {
			currentPower = Mathf.Min(currentPower + (Time.deltaTime * forcePerSecond), maxPower);
		}
		// player releases key
		if (Input.GetKeyUp (fireKey)) {
			// create shell
			GameObject newTankShell = (GameObject)GameObject.Instantiate (tankShellPrefab);
			// set the shell's "owner" to self
			newTankShell.GetComponent<tankShellBehaviour> ().owner = tank;
			// set shell position to own postion
			newTankShell.gameObject.transform.position = this.transform.position;
			// get angle of gun
			float angle = gunPivot.transform.rotation.eulerAngles.z;
			// adjust angle if tank is flipped
			if (tank.transform.localScale.x == -1)
				angle -= 180;// - angle;

			Rigidbody2D shellPhysics = newTankShell.GetComponent<Rigidbody2D> ();
			// launch shell using angle and power
			shellPhysics.AddForce(new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * currentPower, Mathf.Sin(Mathf.Deg2Rad * angle) * currentPower));
			// reset shot power
			currentPower = minPower;
		}
	}
}
