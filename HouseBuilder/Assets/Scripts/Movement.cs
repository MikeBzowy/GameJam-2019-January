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

	// Use this for initialization
	void Start () {
		player_physics = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal_input = Input.GetAxis ("P1_Horizontal");
		this.transform.position += new Vector3 (horizontal_input * horizontal_movement_speed * Time.deltaTime, 0, 0);

		float vertical_input = Input.GetAxis ("P1_Vertical");
		this.transform.position += new Vector3 (0, vertical_input * vertical_movement_speed * Time.deltaTime, 0);

		if (Input.GetAxis ("P1_Grab") > 0.5 && !canGrab)
		{
			canGrab = true;
		}
		if (Input.GetAxis ("P1_Grab") < 0.5 && isGrabing) 
		{
			GetComponent<HingeJoint> ().connectedBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
			canGrab = false;
			isGrabing = false;
			Destroy(GetComponent<HingeJoint>());
		}
	}
	void OnTriggerStay(Collider other)
	{
		if (canGrab == true && !isGrabing)
		{
			canGrab = false;
			isGrabing = true;
			Debug.Log ("Grabing");
			this.gameObject.AddComponent<HingeJoint>();
			this.gameObject.GetComponent<HingeJoint>().connectedBody=other.GetComponent<Rigidbody>();
			this.gameObject.GetComponent<HingeJoint>().axis=new Vector3(0,0,1);
			other.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

		}
	}
}