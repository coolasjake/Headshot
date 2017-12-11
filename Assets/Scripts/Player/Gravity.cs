using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

	private TriggerChecker FacingWall;
	private bool Climbing = false;
	private bool ClimbStop = false;
	private Rigidbody RB;
	private Movement PM;

	public float ClimbingSpeed = 5f;

	// Use this for initialization
	void Start () {
		RB = GetComponent<Rigidbody> ();
		PM = GetComponent<Movement> ();

		//Frontal Trigger for jumping.
		//FacingWall = GetComponentInChildren<TriggerChecker> ();
		if (FacingWall == null) {
			var GO = Instantiate (new GameObject (), transform);
			FacingWall = GO.AddComponent<TriggerChecker>();
			var SC = GO.AddComponent<SphereCollider> ();
			SC.radius = 0.3f;
			SC.isTrigger = true;
			SC.center = new Vector3 (0, 0, 0.3f);
		}

		//Physics.gravity
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.Space) && FacingWall.Triggered) {
			Climbing = true;
			PM.DisableMovement = true;
			if (Input.GetKey (KeyCode.LeftControl))
				ClimbStop = true;
		}
	}

	void FixedUpdate () {
		if (ClimbStop)
			RB.velocity = new Vector3(0, 0, 0);
		else if (Climbing)
			RB.velocity = new Vector3(RB.velocity.x, ClimbingSpeed, RB.velocity.z);
		Climbing = false;
		ClimbStop = false;
	}
}
