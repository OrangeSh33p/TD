using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotation : MonoBehaviour 
{
	[SerializeField] Vector3 rotation;

	void Update () 
	{
		transform.rotation = Quaternion.Euler(rotation);
	}
}
