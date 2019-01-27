using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI : MonoBehaviour {

<<<<<<< HEAD
	public Text timerText;
	public float TimeLeft = 120;
	bool gameStart = false;

=======
>>>>>>> 9cbab1fd3c64d76bc1999002cc136634d5f6d6d8
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
<<<<<<< HEAD
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
=======
	void Update () {
		
>>>>>>> 9cbab1fd3c64d76bc1999002cc136634d5f6d6d8
	}
}
