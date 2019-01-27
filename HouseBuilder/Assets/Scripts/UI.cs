using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	public Text timerText;
	public float TimeLeft;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		TimeLeft -= Time.deltaTime;

		float t = TimeLeft;
		string minutes = ((int)t / 60).ToString ("00");
		string seconds = (t % 60).ToString ("00");
		string miliseconds = ((int)(t * 100f) % 100).ToString("00");

		timerText.text = minutes + ":" + seconds + ":" + miliseconds;

		if (TimeLeft <= 0.0f) {
			timerEnded ();
			timerText.text = ("00:00:00");
		}
	}

	void timerEnded()
	{
		//do stuff here
		Debug.Log("Time Over!!!!");
	}
}