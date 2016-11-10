using UnityEngine;
using System.Collections;

public class DestroysTerrain : MonoBehaviour {
	public float destructionRadius;
	public Vector2 lastPosition;
	private bool firstFrame;
	// Use this for initialization
	void Awake() 
	{
		firstFrame = true;
		lastPosition = new Vector2 (transform.position.x, transform.position.y);
	}

	void Update() 
	{
		bool foundTerrain = false;
		Vector2 location = new Vector2 ();
		if (!firstFrame) {
			/** draw a collision line to previous position, so that shell cannot 'teleport' through terrain 
			 * without causing a collision event */
			Vector2 currentPositon = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
			RaycastHit2D[] hits = Physics2D.LinecastAll(lastPosition, currentPositon);
			for (int hitIndex = 0; hitIndex < hits.Length; hitIndex++) {
				GameObject hitObj = hits [hitIndex].collider.gameObject;
				if (hitObj.GetComponent<TerrainCollision> () != null) {
					foundTerrain = true;
					location = new Vector2 (hitObj.transform.position.x, hitObj.transform.position.y);
					break;
				}
			}
		} else
			firstFrame = false;

		if (foundTerrain) {
			DestructibleTerrain.globalTerrainHandler.collision (location, destructionRadius, this.gameObject);
		} else
			lastPosition = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
	}
}
