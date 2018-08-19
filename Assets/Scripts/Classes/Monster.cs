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
	[HideInInspector] public float predictiveHP;

	//References
	GoldManager goldManager = GoldManager.Instance;
	LivesManager livesManager = LivesManager.Instance;
	MonsterManager monsterManager = MonsterManager.Instance;
	TimeManager timeManager = TimeManager.Instance;
	NavMeshAgent navMeshAgent;


	static List<Transform> _monsterList;
	public static List<Transform> monsterList 
	{
		get 
		{
			if (_monsterList==null)
				_monsterList = new List<Transform> ();
			return _monsterList;
		}
	}

	void Start ()
	{
		monsterList.Add(this.transform);
		transform.parent = monsterManager.transform;
		hp = monsterManager.maxHp;
		predictiveHP = hp;

		origin = Spawner.Instance.transform.position;
		destination = Base.Instance.transform.position;

		navMeshAgent = GetComponent<NavMeshAgent> ();
		navMeshAgent.destination = destination;
	}

	void OnDestroy(){
	}

	void Update ()
	{
		navMeshAgent.speed = monsterManager.speed * timeManager.timeScale;

		if (Vector3.Distance (transform.position, destination) < 3)
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

	public void PredictiveDamage (float damage)
	{
		predictiveHP -= damage;
	}

	void Death ()
	{
		goldManager.AddGold (monsterManager.reward);
		monsterList.Remove (this.transform);

		if (Spawner.Instance.WavesAreOver && monsterList.Count == 0)
			GameManager.Instance.victory = true;
		
		Destroy (gameObject);
	}
}
