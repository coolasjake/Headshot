using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimGroup : MonoBehaviour {

	public int Hits = 0;

	public void BlockHit(bool Correct) {
		int i = Mathf.FloorToInt(Random.value * 16);
		GetComponentsInChildren<AimTester> () [i].HitMe = true;

		if (Correct)
			Hits += 1;
		else {
			Hits = 0;
			foreach (AimTester AT in GetComponentsInChildren<AimTester> ()) {
				AT.HitMe = true;
			}
		}
	}
}
