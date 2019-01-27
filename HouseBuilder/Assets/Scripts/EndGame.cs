using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour {

	public GameObject canvas;

	public Text end_text;

	public ScoreDisplayer[] displayers;
	public PlayerScore[] scores;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void End () {

		for(int i = 0; i < displayers.Length; i++){
			displayers [i].showScore (scores [i]);
			displayers [i].gameObject.SetActive (true);
		}

		float maxScore = 0;
		int player = 0;
		for (int i = 0; i < scores.Length; i++) {
			if (scores [i].totalScore > maxScore) {
				maxScore = scores [i].totalScore;
				player = i;
			}
		}

		end_text.text = ("Player " + (player + 1) + " wins!");
		end_text.gameObject.SetActive (true);

		StartCoroutine (reset ());
	}

	IEnumerator reset() {
		yield return new WaitForSeconds (20);
		SceneManager.LoadScene(0);
	}

}
