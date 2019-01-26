using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float horizontal_movement_speed = 3.0f;
	public float vertical_movement_speed = 3.0f;
	public Rigidbody player_physics;	
	public BoxCollider player_collider;
	bool canGrab = false;
	bool isGrabing = false;

	public int player = 1;

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
			GetComponent<HingeJoint> ().connectedBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
			canGrab = false;
			isGrabing = false;
			Destroy(GetComponent<HingeJoint>());
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
			GameObject current = other.gameObject;
			while (!found && current.transform.parent != null) {
				Rigidbody rb = current.GetComponent<Rigidbody> ();
				Debug.Log ("checking: " + current.name);
				if (rb != null) {
					Debug.Log ("Found rb: " + current.name);
					rb.constraints = RigidbodyConstraints.None;
					found = true;
				}
				current = current.transform.parent.gameObject;
			}

			if (!found)
				Debug.Log ("couldn't find rb");

			this.gameObject.AddComponent<HingeJoint>();
			this.gameObject.GetComponent<HingeJoint>().connectedBody=current.GetComponent<Rigidbody>();
			this.gameObject.GetComponent<HingeJoint>().axis=new Vector3(0,0,1);

				

		}
	}
}