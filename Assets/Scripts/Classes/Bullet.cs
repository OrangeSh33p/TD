using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	//State
	[HideInInspector] public Transform targetTransform;

	//References
	TowerManager towerManager = TowerManager.Instance;
	TimeManager timeManager = TimeManager.Instance;

	void Update () 
	{
		if (targetTransform == null)
			Destroy (gameObject);
		else
		{
			Move ();
			CheckIfTargetReached ();
		}
	}

	void Move ()
	{
		transform.LookAt (targetTransform);
		transform.position += transform.forward * Mathf.Min (towerManager.bulletSpeed * Time.deltaTime * timeManager.timeScale, Vector3.Distance (transform.position, targetTransform.position));
	}

	///Triggers damage sequence if close enough to the target
	void CheckIfTargetReached ()
	{
		if (Vector3.Distance(transform.position, targetTransform.position) < 1)
		{
			targetTransform.GetComponent<Monster> ().Damage (towerManager.damage);
			Destroy (gameObject);		
		}	
	}
}
