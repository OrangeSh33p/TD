using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transparent : MonoBehaviour 
{
	public float f;

	void Update ()
	{
		Color c = gameObject.GetComponent<Renderer> ().material.color;
		gameObject.GetComponent<Renderer> ().material.color = new Color (c.r, c.g, c.b, f);
	}
}
