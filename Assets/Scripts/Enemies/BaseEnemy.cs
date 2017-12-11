using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Shooteable {

	public int Health = 3;

	public void Headshot() {
		Destroy (gameObject);
	}

	public override void Hit() {
		Health -= 1;
		if (Health <= 0) {
			Destroy (gameObject);
		}
	}
}
