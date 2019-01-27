using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public GameObject canvas;

	public Text end_text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void End () {



		end_text.text = ("");
		end_text.gameObject.SetActive (true);

	}

}
