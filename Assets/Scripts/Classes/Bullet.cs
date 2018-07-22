using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	[HideInInspector] public Transform target;
	[HideInInspector] public float speed;
	[HideInInspector] public float damage;

	void Update () 
	{
		//Script keeps executing after gameobject has been destoyed ???
		if (target == null)
		{
			Destroy (gameObject);
		}
		else
		{
			transform.LookAt (target);
			transform.position += transform.forward*speed*Time.deltaTime;

			if (Vector3.Distance(transform.position, target.position) < 1)
			{
				target.GetComponent<Monster> ().Damaged (damage);
				Destroy (gameObject);		
			}			
		}
	}
}
