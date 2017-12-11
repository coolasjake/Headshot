using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	//Stored references
	private Camera MainCamera;
	private Rigidbody RB;
	private TriggerChecker GroundedTrigger;

	//Private values
	bool Grounded = false;
	bool OnSomething = false;

	//--->Messages between Update and FixedUpdate
	private Vector3 DirectionToMove = new Vector3();
	private bool WantToJump = false;

	//--->Messages from other scripts;
	public bool DisableMovement = false;

	//Public values (for adjustment)
	/// <summary>
	/// The min and max angles the camera can face when looking up or down.
	/// </summary>
	public float Clamp = 45;
	/// <summary>
	/// The in game sensitivity multiplier (for settings).
	/// </summary>
	public float Sensitivity = 1;
	/// <summary>
	/// The maximum non-vertical speed the player can cause themselves to move at with 'walking' (does not constrain gravity, explosions etc).
	/// </summary>
	[Range(1f, 20f)]
	public float MaxSpeed = 3;
	/// <summary>
	/// How quickly direction is changed.
	/// </summary>
	[Range(0f, 1f)]
	public float Acceleration = 0.5f;
	/// <summary>
	/// How quickly direction is returned to 0 (replaces friction).
	/// </summary>
	[Range(0f, 1f)]
	public float Decceleration = 0.05f;
	/// <summary>
	/// The multiplier on movement while in the air. Cannot result in greater air speed than ground speed.
	/// </summary>
	[Range(0f, 1f)]
	public float AirControlFactor = 0.5f;
	/// <summary>
	/// The jump velocity.
	/// </summary>
	public float JumpVelocity = 10;




	void Start () {
		MainCamera = GetComponentInChildren<Camera> ();
		Cursor.lockState = CursorLockMode.Locked;
		RB = GetComponent<Rigidbody> ();
		RB.freezeRotation = true;

		//Grounded Trigger for jumping.
		//GroundedTrigger = GetComponentInChildren<TriggerChecker> ();
		if (GroundedTrigger == null) {
			var GO = Instantiate (new GameObject (), transform);
			GroundedTrigger = GO.AddComponent<TriggerChecker>();
			var SC = GO.AddComponent<SphereCollider> ();
			SC.radius = 0.3f;
			SC.isTrigger = true;
			SC.center = new Vector3 (0, -0.8f, 0);
		}
	}

	void Update () {

		//Rotate player
		float rotationX = Input.GetAxis ("Mouse X") * Sensitivity;
		transform.localRotation *= Quaternion.AngleAxis(rotationX, transform.up);

		//Rotate camera
		float rotationY = -Input.GetAxis ("Mouse Y") * Sensitivity;
		Quaternion RotQ = new Quaternion();
		RotQ.eulerAngles = new Vector3 (ClampAngle(MainCamera.transform.localRotation.eulerAngles.x + rotationY, -Clamp, Clamp), 0, 0);
		MainCamera.transform.localRotation = RotQ;


		Vector3 desiredDirection = new Vector3 ();
		if (Input.GetKey (KeyCode.W))
			desiredDirection += transform.forward;
		if (Input.GetKey (KeyCode.S))
			desiredDirection += -transform.forward;
		if (Input.GetKey (KeyCode.A))
			desiredDirection += -transform.right;
		if (Input.GetKey (KeyCode.D))
			desiredDirection += transform.right;

		desiredDirection.Normalize ();
		//desiredDirection = desiredDirection * transform.forward;

		Vector3 newVelocity;
		if (GroundedTrigger.Triggered && OnSomething)
			newVelocity = RB.velocity + (desiredDirection * Acceleration);
		else
			newVelocity = RB.velocity + (desiredDirection * Acceleration * AirControlFactor);
		newVelocity = new Vector3 (newVelocity.x, 0, newVelocity.z);	//Remove y movement
		if (newVelocity.magnitude > MaxSpeed) {
			newVelocity = newVelocity.normalized * MaxSpeed;
		}
		DirectionToMove = newVelocity - new Vector3 (RB.velocity.x, 0, RB.velocity.z);


		if (Input.GetKey (KeyCode.Space)) {
			if (GroundedTrigger.Triggered && OnSomething) {
				GroundedTrigger.Triggered = false;
				OnSomething = false;
				WantToJump = true;
				//DirectionToMove += new Vector3 (0, JumpVelocity, 0);
			}
		}
	}

	void FixedUpdate () {

		if ((GroundedTrigger.Triggered && DirectionToMove.magnitude == 0) || DisableMovement) {
			//Apply a 'friction' force to the player.
			Vector3 NotY = new Vector3 (RB.velocity.x, 0, RB.velocity.z);
			float Y = RB.velocity.y;
			NotY = NotY.normalized * Mathf.Max (0, NotY.magnitude - Decceleration);
			RB.velocity = new Vector3 (NotY.x, Y, NotY.z);
		} else {
			if (WantToJump == true)
				RB.velocity = new Vector3 (RB.velocity.x, JumpVelocity, RB.velocity.z);
			WantToJump = false;

			//Move the player the chosen direction (in fixed to regulate speed).
			RB.velocity += DirectionToMove;
		}	
		DisableMovement = false;
	}

	/// <summary>
	/// Clamps the angle intuitively (goes into negatives instead of back to 360), so long as the clamps are 85 or less.
	/// </summary>
	public static float ClampAngle (float angle, float min, float max) {

		min = Mathf.Clamp (min, -85, 0);
		max = Mathf.Clamp (max, 0, 85);

		if (angle >= 180f)
			angle -= 360f;

		return Mathf.Clamp (angle, min, max);
	}

	/*
	void OnTriggerExit (Collider col) {
		Grounded = false;
	}

	void OnTriggerStay (Collider col) {
		Grounded = true;
	}
	*/

	void OnCollisionExit (Collision col) {
		OnSomething = false;
	}

	void OnCollisionStay (Collision col) {
		OnSomething = true;
	}
}
