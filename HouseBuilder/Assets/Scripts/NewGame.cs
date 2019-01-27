using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour {

	float p1;
	float p2;
	float p3;
	float p4;

	int one = 0;
	int two = 0;
	int three = 0;
	int four = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
		p1 = Input.GetAxis ("P1_Grab");
		p2 = Input.GetAxis ("P2_Grab");
		p3 = Input.GetAxis ("P3_Grab");
		p4 = Input.GetAxis ("P4_Grab");

		if ((p1 > 0.5f) && !one){
			Debug.Log("P1 in game");
			one = 1;
			GameObject.Find("P1_Grey").SendMessage ("Triger");
		}
		if ((p2 > 0.5f) && !two){
			Debug.Log("P2 in game");
			two = 1;
			GameObject.Find("P2_Grey").SendMessage ("Triger");
		}
		if ((p3 > 0.5f) && !three){
			Debug.Log("P3 in game");
			three = 1;
			GameObject.Find("P3_Grey").SendMessage ("Triger");
		}
		if ((p4 > 0.5f) && !four){
			Debug.Log("P4 in game");
			four = 1;
			GameObject.Find("P4_Grey").SendMessage ("Triger");
		}
	
		if ((one + two + three + four) >= 3){
			SceneManager.LoadScene("main");
		}

	}
}
