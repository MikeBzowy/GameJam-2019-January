﻿using System.Collections;
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

	public AudioClip Grabsound;

	private AudioSource source;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
		p1 = Input.GetAxis ("P1_Grab");
		p2 = Input.GetAxis ("P2_Grab");
		p3 = Input.GetAxis ("P3_Grab");
		p4 = Input.GetAxis ("P4_Grab");

		if ((p1 > 0.5f) && one != 1){
			Debug.Log("P1 in game");
			one = 1;
			GameObject.Find("P1_Grey").SendMessage ("Triger");
			source.PlayOneShot(Grabsound,40);
		}
		if ((p2 > 0.5f) && two != 1){
			Debug.Log("P2 in game");
			two = 1;
			GameObject.Find("P2_Grey").SendMessage ("Triger");
			source.PlayOneShot(Grabsound,40);
		}
		if ((p3 > 0.5f) && three != 1){
			Debug.Log("P3 in game");
			three = 1;
			GameObject.Find("P3_Grey").SendMessage ("Triger");
			source.PlayOneShot(Grabsound,40);
		}
		if ((p4 > 0.5f) && four != 1){
			Debug.Log("P4 in game");
			four = 1;
			GameObject.Find("P4_Grey").SendMessage ("Triger");
			source.PlayOneShot(Grabsound,40);
		}
	
		if ((one + two + three + four) >= 3){
			SceneManager.LoadScene("main");
		}

	}
}
