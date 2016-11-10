using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerControl2 : MonoBehaviour 
{

	// Check in Unity to change this speed
	public int moveSpeed;
	public GameObject graphics;

	[SerializeField]
	public Stat health;

	private void Awake ()
	{
		health.Initialize();
	}

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

		//this is for testing if the player healthbar will go if  the health gets attacked ( by pressing  a key)
		// If you press the z key player1 will lose 10 health
		//if(Input.GetKeyDown(KeyCode.Z))
		//{
		//	health.CurrentVal -= 10;
		//}
		//If you press the X key player1 will gain 10 health
		//if(Input.GetKeyDown(KeyCode.X))
		//{
		//	health.CurrentVal += 10;
		//}
		if (health.CurrentVal <= 0) 
		{
			Die ();
		}
	}
	void Die()
	{
		SceneManager.LoadScene("level1");
	}
}
