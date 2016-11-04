using UnityEngine;
using System.Collections;

public class TerrainCollision : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		DestroysTerrain destroyer = col.GetComponent<DestroysTerrain> ();
		if (destroyer != null)
		{
			DestructibleTerrain.globalTerrainHandler.collision (new Vector2 (col.transform.position.x, col.transform.position.y), destroyer.destructionRadius, col.gameObject);
		}
	}
}
