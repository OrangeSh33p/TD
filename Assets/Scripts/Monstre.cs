using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monstre : MonoBehaviour 
{
	public float speed;
	public float maxHp;
	public float hp;

	public Slider hpBar;

	void Start ()
	{
		hp = maxHp;
	}

	void Update ()
	{
		transform.position += transform.forward*speed*Time.deltaTime;
	}

	public void Damaged (float damage)
	{
		hp -= damage;
		hpBar.value = hp / maxHp;
		if (hp == 0) Destroy (transform.parent.gameObject);
	}
}
