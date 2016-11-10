using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WindBehaviour : MonoBehaviour {
	public static WindBehaviour globalWind;
	public RectTransform arrow;
	public Text speedDisplay;
	public float directionSpeed;
	public float directionAcceleration;
	public float directionAccX2;
	public float windSpeed;
	public float windAcceleration;
	public float windAccX2;
	public float averageWindSpeed;
	// Use this for initialization
	void Start () {
		globalWind = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (directionSpeed < 0) {
			directionAcceleration += directionAccX2 * Time.deltaTime;
		} else {
			directionAcceleration -= directionAccX2 * Time.deltaTime;
		}
		if (windSpeed < averageWindSpeed) {
			windAcceleration += windAccX2 * Time.deltaTime;
		} else {
			windAcceleration -= windAccX2 * Time.deltaTime;
		}

		directionSpeed += directionAcceleration * Time.deltaTime;
		windSpeed += windAcceleration * Time.deltaTime;
		if (windSpeed < 0) {
			windSpeed = 0;
		}

		arrow.Rotate(new Vector3(0f, 0f, directionSpeed * Time.deltaTime));
		speedDisplay.text = (windSpeed * 5).ToString ("F0") + "km/h wind";
	}
}
