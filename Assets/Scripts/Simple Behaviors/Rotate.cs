using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour 
{
	[Header("Axis")]
	[SerializeField] bool x;
	[SerializeField] bool y;
	[SerializeField] bool z;

	[Header("Balancing")]
	[SerializeField] float start;
	[SerializeField] float finish;
	[SerializeField] float duration;

	//State
	float value;
	float timeTillRestart;

	void Update ()
	{
		if (x)
			transform.rotation = Quaternion.Euler (new Vector3 (value, transform.rotation.y, transform.rotation.z));
		if (y)
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, value, transform.rotation.z));
		if (z)
			transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, value));

		timeTillRestart -= Time.deltaTime * TimeManager.timeScale;
		if (timeTillRestart <= 0)
		{
			Tweens.Linear(start, finish, duration, (float f) => {value = f;});
			timeTillRestart = duration;
		}
	}
}
