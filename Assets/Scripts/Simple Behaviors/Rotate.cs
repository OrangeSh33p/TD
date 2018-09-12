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
	[SerializeField] bool pingPong;

	//State
	float value;
	int direction = 1;
	float amplitude;

	void Start () {
		value = start;
		amplitude = Mathf.Abs(finish-start);
		direction = (int)Mathf.Sign(finish-start);
	}

	void Update ()
	{
		value += amplitude * Time.deltaTime * TimeManager.timeScale/(duration) * direction;

		if (direction*(value-finish) > 0) {
			if (pingPong) {
				direction *= -1;
				value = 2*finish-value;
				float tmp = start;
				start = finish;
				finish = tmp;
			} else {
				value -= amplitude * direction;
			}
		}
		if (direction*(value-start) < 0) 
			if (pingPong) {
				direction *= -1;
				value = 2*start-value;
				float tmp = start;
				start = finish;
				finish = tmp;
			} else {
				value += amplitude * direction;
			}

		if (x) transform.localRotation = Quaternion.Euler (new Vector3 (value, transform.localRotation.y, transform.localRotation.z));
		if (y) transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.x, value, transform.localRotation.z));
		if (z) transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.x, transform.localRotation.y, value));
	}
}
