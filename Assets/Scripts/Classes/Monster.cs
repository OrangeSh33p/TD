using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Monster : MonoBehaviour 
{

	//Change HP Bar system
	[Header("Boring Variables")]
	public Slider hpBar;

	//State
	float hp;
	Vector3 origin;
	Vector3 destination;

	//References
	GoldManager goldManager = GoldManager.Instance;
	LivesManager livesManager = LivesManager.Instance;
	MonsterManager monsterManager = MonsterManager.Instance;
	TimeManager timeManager = TimeManager.Instance;
	NavMeshAgent navMeshAgent;

	void Start ()
	{
		transform.parent = monsterManager.transform;
		hp = monsterManager.maxHp;

		origin = Spawner.Instance.transform.position;
		destination = Base.Instance.transform.position;

		navMeshAgent = GetComponent<NavMeshAgent> ();
		navMeshAgent.destination = destination;
	}

	void Update ()
	{
		navMeshAgent.speed = monsterManager.speed * timeManager.timeScale;

		if (Vector3.Distance (transform.position, destination) < 1)
			Respawn ();
	}

	void Respawn ()
	{
		livesManager.LoseLife ();
		transform.position = origin;
	}

	public void Damage (float damage)
	{
		hp -= damage;
		hpBar.value = hp / monsterManager.maxHp;
		if (hp <= 0)
			Death ();
	}

	void Death ()
	{
		goldManager.AddGold (monsterManager.reward);
		if (Spawner.Instance.WavesAreOver() && monsterManager.MonsterList().Count == 1)
			StartCoroutine (GameManager.Instance.Victory());
		Destroy (gameObject);
	}
}
