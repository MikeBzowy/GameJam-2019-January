using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDetection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			Collider[] c = Physics.OverlapBox (gameObject.transform.position, gameObject.transform.localScale / 2 + new Vector3(0.25f,0.25f,0.25f), gameObject.transform.rotation);
			Debug.Log("found: ");
			foreach (Collider cs in c)
				Debug.Log (cs.gameObject.name);
			Debug.Log ("end found");
		}

		if (Input.GetKeyDown (KeyCode.O)) {
			foreach (Vector3 v in GetColliderVertexPositions(gameObject)) {
				Debug.Log (v);
			}
		}
	}

	Vector3[] GetColliderVertexPositions (GameObject go) {
		var vertices = new Vector3[8];
		var thisMatrix = go.transform.localToWorldMatrix;
		var storedRotation = go.transform.rotation;
		go.transform.rotation = Quaternion.identity;

		var extents = go.GetComponent<BoxCollider>().bounds.extents;
		vertices[0] = thisMatrix.MultiplyPoint3x4(extents);
		vertices[1] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, extents.z));
		vertices[2] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, extents.y, -extents.z));
		vertices[3] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, extents.y, -extents.z));
		vertices[4] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, extents.z));
		vertices[5] = thisMatrix.MultiplyPoint3x4(new Vector3(-extents.x, -extents.y, extents.z));
		vertices[6] = thisMatrix.MultiplyPoint3x4(new Vector3(extents.x, -extents.y, -extents.z));
		vertices[7] = thisMatrix.MultiplyPoint3x4(-extents);

		go.transform.rotation = storedRotation;
		return vertices;
	}
}
