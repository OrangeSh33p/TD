using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour 
{
	float y;

	void Start ()
	{
		//StartCoroutine (Tweens.Linear (0, 360, 5, y));
		Tweens.Quad(0,360,5, (float v)=>{
			y = v;
		} );
			
	}

	void Update ()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, y, transform.rotation.z));
	}

	void RotateMe(float toThis){
		y = toThis;
	}
}
