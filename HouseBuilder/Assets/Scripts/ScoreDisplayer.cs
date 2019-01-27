using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {
	public Text atmosphere;
	public Text safety;
	public Text luxury;
	public Text design;

	public Text total;

	public void showScore(PlayerScore p) {
		atmosphere.text += " " + p.atmosphere;
		safety.text += " " + p.safety;
		luxury.text += " " + p.luxury;
		design.text += " " + p.interior_design;

		total.text += " " + p.totalScore;
	}
}
