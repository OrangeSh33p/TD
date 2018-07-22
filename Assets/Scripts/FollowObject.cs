using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour 
{
	public Transform target;

	void Update () 
	{
		if (transform.position != target.position)
			transform.position = target.position;
	}
}
