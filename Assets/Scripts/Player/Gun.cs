using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public bool WannaShoot = false;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			WannaShoot = true;
		}
	}

	void FixedUpdate () {
		if (WannaShoot) {
			WannaShoot = false;

			Ray R;
			RaycastHit Hit;

			if (Physics.Raycast(transform.position, transform.forward, out Hit)) {
				Debug.DrawRay (transform.position, transform.forward, Color.red, 2f);
				Shooteable SH = Hit.transform.GetComponent<Shooteable>();
				if (SH) {
					SH.Hit();
				}
			}
		}
	}
}
