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
	float value; //Current value of the rotation
	int direction = 1; //Equal to 1 if going forward, -1 if going backward
	float amplitude;

	void Start () {
		value = start;

		float signedAmplitude = finish - start;
		amplitude = Mathf.Abs(signedAmplitude);
		direction = (int)Mathf.Sign(signedAmplitude);
	}

	void Update ()
	{
		value += TimeManager.scaledDeltaTime * amplitude * direction / duration;

		if (direction * (value-finish) > 0) {
			if (pingPong) {
				value = 2*finish - value;
				Reverse();
			} else
				value -= amplitude * direction;
		}
		if (direction * (value-start) < 0) 
			if (pingPong) {
				value = 2*start - value;
				Reverse();
			} else
				value += amplitude * direction;

		if (x) transform.localRotation = Quaternion.Euler (new Vector3 (value, transform.localRotation.y, transform.localRotation.z));
		if (y) transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.x, value, transform.localRotation.z));
		if (z) transform.localRotation = Quaternion.Euler (new Vector3 (transform.localRotation.x, transform.localRotation.y, value));
	}

	///<summary> Switches the value of start and finish, reverses the direction	</summary>
	void Reverse () {
				direction *= -1;
				
				float tmp = start;
				start = finish;
				finish = tmp;
	}
}
