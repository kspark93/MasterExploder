using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarScript : MonoBehaviour {

	private float fillAmount;

	[SerializeField]
	private Image content;

	[SerializeField]
	private Text valueText;

	public float MaxValue { get; set; }

	public float Value 
	{
		set 
		{
			string[] tmp = valueText.text.Split(':');
			valueText.text = tmp[0] + ": " + value;
			fillAmount = Map(value, 0, MaxValue, 0 ,1);
		}
	}

	void Start () 
	{
	
	}

	void Update () 
	{
		HandleBar();
	}

	private void HandleBar ()
	{
		// only updates when its different from the fillamount
		if (fillAmount != content.fillAmount) 
		{
			//accessing the fillAmount in the content object.
			content.fillAmount = fillAmount;
		}
	}

	//Map is going to take health and translate it to a value the fill amount can understand
	//current health , minuim health, maxium health, values we are translating into ( miniuim and maximuim
	private float Map (float value, float inMin, float inMax, float outMin, float outMax)
	{
		return (value - inMin)* (outMax - outMin) / (inMax - inMin) + outMin;
		// Example 80 health : ( 80- 0 ) * ( 1 - 0 ) / (100 - 0 ) + 0 
		// 80 * 1 / 100 
		// 0.8
	}
}
