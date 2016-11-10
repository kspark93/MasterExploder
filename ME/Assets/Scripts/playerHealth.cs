using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour 
{
	public int currentPlayerHealth;
	public int maxPlayerHealth;
	public Image healthBar;

	void Start () 
	{
		// when you set the currentPlayerHealth to 0 in settings as the game has loaded the player will die
		currentPlayerHealth = maxPlayerHealth;
	}

	void Update () 
	{
		healthBar.fillAmount = (float)currentPlayerHealth / maxPlayerHealth;

		if (currentPlayerHealth <= 0) 
		{
			Die ();
		}
	}
	// When the player dies it reloads the level
	void Die()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}

