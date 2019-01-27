﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float horizontal_movement_speed = 3.0f;
	public float vertical_movement_speed = 3.0f;
	public Rigidbody player_physics;	
	public BoxCollider player_collider;
	public float breakForce = 10f;
	bool canGrab = false;
	bool isGrabing = false;

	public int player = 1;

	HingeJoint HJ;
	Rigidbody connected;

	// Use this for initialization
	void Start () {
		player_physics = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal_input = Input.GetAxis ("P" + player + "_Horizontal");
		this.transform.position += new Vector3 (horizontal_input * horizontal_movement_speed * Time.deltaTime, 0, 0);

		float vertical_input = Input.GetAxis ("P" + player + "_Vertical");
		this.transform.position += new Vector3 (0, vertical_input * vertical_movement_speed * Time.deltaTime, 0);

		if (Input.GetAxis ("P" + player + "_Grab") > 0.5 && !canGrab)
		{
			canGrab = true;
		}
		if (Input.GetAxis ("P" + player + "_Grab") < 0.5 && isGrabing) 
		{
			
			if (HJ != null)
			{
				HJ.connectedBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
				connected = null;
				Destroy(HJ);
			}
			canGrab = false;
			isGrabing = false;
		}

		if (isGrabing && HJ == null) {
			connected.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;;
		}

	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer != LayerMask.NameToLayer ("Furniture")) {
			return;
		}
		if (canGrab == true && !isGrabing)
		{
			canGrab = false;
			isGrabing = true;
			Debug.Log ("Grabing");

			bool found = false;
			Transform current = other.gameObject.transform;
			while (!found && current != null) {
				Rigidbody rb = current.gameObject.GetComponent<Rigidbody> ();
				Debug.Log ("checking: " + current.name);
				if (rb != null) {
					Debug.Log ("Found rb: " + current.name);
					rb.constraints = RigidbodyConstraints.None;
					found = true;
				} else {
					current = current.transform.parent;
				}
			}

			if (!found)
				Debug.Log ("couldn't find rb");
			else {
				HJ = this.gameObject.AddComponent<HingeJoint> ();
				HJ.connectedBody = current.gameObject.GetComponent<Rigidbody> ();
				connected = HJ.connectedBody;
				HJ.axis = new Vector3 (0, 0, 1);
				HJ.breakForce = 1000f;
			}

				

		}
	}
}