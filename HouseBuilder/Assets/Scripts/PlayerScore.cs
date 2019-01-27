using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour {
	public FurnitureSpawner fs;
	public RoomManager rm;

	public float atmosphere = 0;
	public float safety = 0;
	public float luxury = 0;
	public float interior_design = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ScoreGame() {
		foreach (GameObject g in fs.placedFurniture) {
			luxury += g.transform.GetChild(0).GetComponent<Furniture> ().points; // points for furniture placed
		}

		foreach (List<GameObject> rooms in rm.roomContents) {
			List<string> colors = new List<string> ();
			List<int> colorCount = new List<int> ();

			List<string> sets = new List<string> ();
			List<int> setCount = new List<int> ();

			List<string> types = new List<string> ();

			for (int i = 0; i < rooms.Count; i++) {
				Furniture currentF = rooms [i].transform.GetChild(0).GetComponent<Furniture> ();
				luxury += currentF.points; // bonus points for furniture inside

				int cIndex = colors.IndexOf(currentF.myColor);
				if (cIndex == -1) {
					colors.Add (currentF.myColor);
					colorCount.Add (1);
				} else {
					colorCount[cIndex]++;
				}

				int sIndex = sets.IndexOf(currentF.mySet);
				if (sIndex == -1) {
					sets.Add (currentF.mySet);
					setCount.Add (1);
				} else {
					setCount[sIndex]++;
				}

				int tIndex = types.IndexOf(currentF.myType);
				if (sIndex == -1) {
					types.Add (currentF.mySet);
				}
			}

			for (int i = 0; i < colorCount.Count; i++) {
				if (colorCount [i] >= 3)
					interior_design += 5 * colorCount [i];
			}

			for (int k = 0; k < setCount.Count; k++) {
				if (setCount [k] >= 3)
					interior_design += 5 * setCount [k];
			}

			if(types.Count >= 3)
				interior_design += types.Count * 5;
		}

		foreach (RoomManager.Room r in rm.rooms) {
			atmosphere += (r.maxX - r.minX) * (r.maxY - r.minY); // calculate indoor living space
		}
	}
}
