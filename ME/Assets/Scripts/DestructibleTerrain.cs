using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DestructibleTerrain : MonoBehaviour {
	public static DestructibleTerrain globalTerrainHandler;
	public Transform colliderContainer;
	public GameObject manualCollider;
	public Texture2D terrainImage;
	public const float MINIMUM_ALPHA = 0.1f;
	public float pixelsPerUnit;
	// Use this for initialization
	void Awake () 
	{
		// make sure there is only ever one instance of DestructibleTerrain
		if (globalTerrainHandler == null) {
			globalTerrainHandler = this;
		} else {
			GameObject.Destroy (this.gameObject);
		}
		// create terrain image
		terrainImage = gameObject.GetComponent<SpriteRenderer>().sprite.texture;
		Color[] pixels = terrainImage.GetPixels ();
		terrainImage = new Texture2D (terrainImage.width, terrainImage.height);
		terrainImage.SetPixels (pixels);
		gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(terrainImage,
			new Rect (0f, 0f, (float)terrainImage.width, (float)terrainImage.height), 
			new Vector2 (0.5f, 0.5f), pixelsPerUnit);
		terrainImage.Apply ();
		// initialise collision
		initCollsionEdge (terrainImage);
	}

	// called from TerrainCollision.cs on CollisionPixelContainer
	public void collision(Vector2 position, float radius, GameObject triggeringObject)
	{
		GameObject.Destroy ((UnityEngine.Object)triggeringObject);
		// convert the world-position to terrain image pixel position
		Vector2 terrainPos = getTerrainPoint(position);
		destroyTerrain (terrainImage, terrainPos, radius);
		// update terrain image
		terrainImage.Apply ();
		// and then update colliders
		changeCollisionEdge(terrainImage, colliderContainer, new Rect(terrainPos.x - radius, terrainPos.y - radius, radius * 2 + 1, radius * 2 + 1));
	}

	/** Converts image pixels to unity 'units'*/
	private Vector2 getWorldPoint(Vector2 terrainPoint)
	{
		Vector2 worldPoint = new Vector2 (terrainPoint.x, terrainPoint.y);
		worldPoint.x = (worldPoint.x - (terrainImage.width / 2f)) / pixelsPerUnit * gameObject.transform.localScale.x;
		worldPoint.y = (worldPoint.y - (terrainImage.height / 2f)) / pixelsPerUnit * gameObject.transform.localScale.y;
		return worldPoint;
	}

	/** Convert unity 'units' to image pixels*/
	private Vector2 getTerrainPoint(Vector2 worldPoint)
	{
		Vector2 terrainPoint = new Vector2 (worldPoint.x, worldPoint.y);
		terrainPoint.x = terrainPoint.x / gameObject.transform.localScale.x * pixelsPerUnit + (terrainImage.width / 2.0f);
		terrainPoint.y = terrainPoint.y / gameObject.transform.localScale.y * pixelsPerUnit + (terrainImage.height / 2.0f);
		return terrainPoint;
	}

	private void initCollsionEdge(Texture2D terrain)
	{
		List<Vector2> points = new List<Vector2> ();

		for (int imgY = 0; imgY < terrain.height; imgY++) {
			for (int imgX = 0; imgX < terrain.width; imgX++) {
				Vector2 pixel = new Vector2(imgX, imgY);
				if (isEdgePixel (pixel, terrain))
					points.Add (pixel);
			}
		}
		// create edge collider objects
		Vector2[] arrPoints = points.ConvertAll<Vector2>(getWorldPoint).ToArray();
		foreach (Vector2 vec in arrPoints)
		{
			GameObject col = (GameObject)GameObject.Instantiate ((UnityEngine.Object)manualCollider, colliderContainer);
			col.transform.position = new Vector3(vec.x, vec.y);
		}
	}

	/** Update colliders in specified areaOfChange */
	private void changeCollisionEdge(Texture2D terrain, Transform collisionParent, Rect areaOfChange)
	{
		// add one extra pixel on each side, to make sure none are missed
		Rect areaToCheck = new Rect (areaOfChange.x - 1f, areaOfChange.y - 1f, areaOfChange.width + 2f, areaOfChange.height + 2f);
		// find and 'remove' child colliders that no longer match terrain
		List<Transform> removedChildren = new List<Transform> ();
		for (int childIndex = collisionParent.childCount - 1; childIndex >= 0; childIndex--)
		{
			Transform child = collisionParent.GetChild (childIndex);
			Vector2 childPos = getTerrainPoint(new Vector2 (child.position.x, child.position.y));
			if (areaToCheck.Contains (new Vector3 (childPos.x, childPos.y))) 
			{
				if (!isEdgePixel (childPos, terrain)) 
				{
					removedChildren.Add (child);
				}
			}
		}
		// create new (or move existing 'removed') child colliders
		for (int py = Mathf.Max(0, (int)areaToCheck.y); py < Mathf.Max(0, Mathf.Min(terrain.height, (int)(areaToCheck.y + areaToCheck.height))); py++) 
		{
			for (int px = Mathf.Max(0, (int)areaToCheck.x); px < Mathf.Max(0, Mathf.Min(terrain.width, (int)(areaToCheck.x + areaToCheck.width))); px++) 
			{
				if (isEdgePixel(new Vector2(px, py), terrain))
				{
					Vector2 newPos = getWorldPoint(new Vector2 (px, py));
					if (removedChildren.Count > 0)
					{
						// reuse child that was 'removed'
						removedChildren[0].position = new Vector3 (newPos.x, newPos.y);
						removedChildren.RemoveAt (0);
					}
					else 
					{
						// create new child
						GameObject col = (GameObject)GameObject.Instantiate ((UnityEngine.Object)manualCollider, collisionParent);
						col.transform.position = new Vector3(newPos.x, newPos.y);
					}
				}
			}
		}
		// actually destroy any remaining child colliders
		foreach (Transform tran in removedChildren)
		{
			GameObject.Destroy ((UnityEngine.Object)tran.gameObject);
		}
	}

	/** Is this pixel both opaque and directly adjacent to a non-opaque pixel?*/
	private bool isEdgePixel(Vector2 pixel, Texture2D texture) 
	{
		if (texture.GetPixel ((int)pixel.x, (int)pixel.y).a >= MINIMUM_ALPHA)
		{
			if (pixel.x > 0 && texture.GetPixel ((int)pixel.x - 1, (int)pixel.y).a < MINIMUM_ALPHA)
				return true;
			else if (pixel.y > 0 && texture.GetPixel ((int)pixel.x, (int)pixel.y - 1).a < MINIMUM_ALPHA)
				return true;
			else if (pixel.x < texture.width - 1 && texture.GetPixel ((int)pixel.x + 1, (int)pixel.y).a < MINIMUM_ALPHA)
				return true;
			else if (pixel.y < texture.height - 1 && texture.GetPixel ((int)pixel.x, (int)pixel.y + 1).a < MINIMUM_ALPHA)
				return true;
		}
		return false;
	}

	/** Remove pixels from terrain image in radius at point */
	private void destroyTerrain(Texture2D terrain, Vector2 point, float radius)
	{
		int radiusInt = (int)Mathf.Floor (radius);
		Vector2 radiate = new Vector2 ();
		Vector2 center = new Vector2 ();
		for (int desX = -radiusInt; desX < radiusInt; desX++)
		{
			radiate.x = desX;
			for (int desY = -radiusInt; desY < radiusInt; desY++)
			{
				radiate.y = desY;
				if (Vector2.Distance (radiate, center) < radius)
					terrain.SetPixel (desX + (int)point.x, desY + (int)point.y, Color.clear);
			}
		}
	}
}
