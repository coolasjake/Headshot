using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : Shooteable {

	public override void Hit() {
		GetComponentInParent<BaseEnemy> ().Headshot();
	}
}
