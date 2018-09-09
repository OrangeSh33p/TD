using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsTarget : MonoBehaviour {

	[SerializeField] Tower tower;

	void Update () {
		transform.LookAt (tower.target, Vector3.up);
		transform.rotation = Quaternion.Euler (0, transform.eulerAngles.y, 0);
	}
}
