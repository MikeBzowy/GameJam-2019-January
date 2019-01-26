using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public float horizontal_movement_speed = 3.0f;
	public float vertical_movement_speed = 3.0f;
	public Rigidbody player_physics;	
	public BoxCollider player_collider;

	// Use this for initialization
	void Start () {
		player_physics = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal_input = Input.GetAxis ("Horizontal");
		this.transform.position += new Vector3 (horizontal_input * horizontal_movement_speed * Time.deltaTime, 0, 0);

		float vertical_input = Input.GetAxis ("Vertical");
		this.transform.position += new Vector3 (0, vertical_input * vertical_movement_speed * Time.deltaTime, 0);
	}
}
