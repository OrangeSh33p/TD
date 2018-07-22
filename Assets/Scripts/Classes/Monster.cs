using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour 
{
	public Slider hpBar;

	[SerializeField] float speed;
	[SerializeField] float maxHp;
	[SerializeField] float hp;

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
		if (hp <= 0) Destroy (gameObject);
	}
}
