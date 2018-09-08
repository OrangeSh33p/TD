using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	//State
	[HideInInspector] public TowerManager.TowerType type;
	[HideInInspector] public Transform targetTransform;

	//Arbitrary balancing
	float hitDistance = 1;

	void Update ()  {
		if (targetTransform == null)
			Destroy (gameObject);
		else {
			Move ();
			CheckIfTargetReached ();
		}
	}

	void Move () {
		transform.LookAt (targetTransform);
		transform.position += 
			transform.forward * Mathf.Min (type.bulletSpeed * Time.deltaTime * TimeManager.timeScale, Vector3.Distance (transform.position, targetTransform.position));
	}

	///Triggers damage sequence if close enough to the target
	void CheckIfTargetReached () {
		if (Vector3.Distance(transform.position, targetTransform.position) < hitDistance) {
			targetTransform.GetComponent<Monster> ().Damage (type.damage);
			Destroy (gameObject);		
		}	
	}
}
