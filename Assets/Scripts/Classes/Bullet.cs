using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	//State
	Transform targetTransform;

	//References
	TowerManager towerManager = TowerManager.Instance;

	void Update () 
	{
		if (targetTransform == null)
			Destroy (gameObject);
		else
		{
			transform.LookAt (targetTransform);
			transform.position += transform.forward*towerManager.bulletSpeed*Time.deltaTime;

			if (Vector3.Distance(transform.position, targetTransform.position) < 1)
			{
				targetTransform.GetComponent<Monster> ().Damage (towerManager.damage);
				Destroy (gameObject);		
			}			
		}
	}

	public void Initialize (Transform target)
	{
		targetTransform = target;
	}
}
