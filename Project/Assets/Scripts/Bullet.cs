using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	[HideInInspector] public GameObject target;
	[HideInInspector] public float speed;
	[HideInInspector] public float damage;

	void Update () 
	{
		if (target == null)
		{
			Destroy (gameObject);
		}

		transform.LookAt (target.transform);
		transform.position += transform.forward*speed*Time.deltaTime;

		if (Vector3.Distance(transform.position, target.transform.position) < 1)
		{
			target.GetComponent<Monstre> ().Damaged (damage);
			Destroy (gameObject);		
		}
	}
}
