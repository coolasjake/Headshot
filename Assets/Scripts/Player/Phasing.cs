using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phasing : MonoBehaviour {

	private PhaseFailsafe FailSafe;
	private Canvas C;

	// Use this for initialization
	void Start () {
		GameObject Child = Instantiate (new GameObject (), transform);
		CapsuleCollider CC = Child.AddComponent<CapsuleCollider> ();
		CC.radius = GetComponent<CapsuleCollider> ().radius * 1.1f;
		CC.height = GetComponent<CapsuleCollider> ().height * 1.1f;
		CC.isTrigger = true;
		FailSafe = Child.AddComponent<PhaseFailsafe> ();

		C = FindObjectOfType<Canvas> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.E)) {
			gameObject.layer = 9;
			C.GetComponentInChildren<Image> ().enabled = true;
		} else {
			if (!FailSafe.InsideSomething) {
				gameObject.layer = 8;
				C.GetComponentInChildren<Image> ().enabled = false;
			}
		}
	}
}
