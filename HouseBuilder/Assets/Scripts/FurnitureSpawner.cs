using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpawner : MonoBehaviour {

	public static GameObject[] furniture;
	public static GameObject[] walls;

	public GameObject furniturePrefab;

	public Transform[] slots;

	Rigidbody[] inventory;

	float MAX_DELAY = 3;
	float counter = 0;

	bool grabbed = true;

	public List<GameObject> placedWalls;
	public List<GameObject> placedFurniture;

	void Start()
	{
		//Important note: place your prefabs folder(or levels or whatever) 
		//in a folder called "Resources" like this "Assets/Resources/Prefabs"
		furniture = Resources.LoadAll<GameObject>("furniture");

		walls = Resources.LoadAll<GameObject>("walls");

		inventory = new Rigidbody[slots.Length];

		placedWalls = new List<GameObject> ();
		placedFurniture = new List<GameObject> ();
	}

	void SpawnRandomFurniture(int slot) 
	{    
		GameObject prefab = Instantiate (furniturePrefab, slots[slot].position, slots[slot].rotation);
		int whichItem = Random.Range (0, furniture.Length);
		GameObject myObj = Instantiate (furniture [whichItem], prefab.transform.position, prefab.transform.rotation, prefab.transform) as GameObject;
		inventory [slot] = prefab.GetComponent<Rigidbody>();
		ChangeLayersRecursively (prefab.transform, "UI");

	}

	void SpawnRandomWalls(int slot) 
	{    
		GameObject prefab = Instantiate (furniturePrefab, slots[slot].position, slots[slot].rotation);
		int whichItem = Random.Range (0, walls.Length);
		GameObject myObj = Instantiate (walls [whichItem], prefab.transform.position, walls [whichItem].transform.rotation, prefab.transform) as GameObject;
		inventory [slot] = prefab.GetComponent<Rigidbody>();
		ChangeLayersRecursively (prefab.transform, "UI");
		prefab.tag = "wall";
	}

	void Update() {
		if (!grabbed) {
			foreach (Rigidbody r in inventory) {
				if (r.constraints != RigidbodyConstraints.FreezeAll) {
					grabbed = true;
					ChangeLayersRecursively (r.transform, "Furniture");
					if (r.gameObject.tag == "wall")
						placedWalls.Add (r.gameObject.transform.GetChild(0).gameObject);
					else
						placedFurniture.Add (r.gameObject);
				}
			}

			if (grabbed) {
				for(int i= 0; i < inventory.Length; i++){
					if (inventory[i].constraints == RigidbodyConstraints.FreezeAll) {
						Destroy (inventory [i].gameObject);
					}
					inventory [i] = null;
				}
			}
		} else {
			if (furniture.Length != 0 && walls.Length != 0) {
				counter -= Time.deltaTime;
				if (counter < 0) {
					grabbed = false;
					for (int i = 0; i < slots.Length; i++) {
						if (Random.Range (0, 4) > 0) {
							SpawnRandomFurniture (i);
						} else {
							SpawnRandomWalls (i);
						}
					}
					counter = MAX_DELAY;
				}
			}
		}
	}

	public static void ChangeLayersRecursively(Transform trans, string name)
	{
		trans.gameObject.layer = LayerMask.NameToLayer(name);
		foreach(Transform child in trans)
		{            
			ChangeLayersRecursively(child, name);
		}
	}
}
