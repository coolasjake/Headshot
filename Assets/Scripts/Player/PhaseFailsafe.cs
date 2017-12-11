using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseFailsafe : MonoBehaviour {

	public bool InsideSomething = false;

	void OnTriggerStay (Collider col) {
		if (col.tag != "Ground")
			InsideSomething = true;
	}

	void OnTriggerExit (Collider col) {
		if (col.tag != "Ground")
			InsideSomething = false;
	}
}
