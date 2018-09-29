using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
	//Serialized
	public int typeNumber;
	public HpBar hpBar;

	//Type
	[HideInInspector] public MonsterManager.monsterType type;

	//HP
	float hp;
	[HideInInspector] public float predictiveHP;

	//Pathfinding
	[HideInInspector] public Transform origin;
	[HideInInspector] public float distanceWalked;
	int currentStep = 1;

	//monsterList : list of all monsters
	static List<Transform> _monsterList;
	public static List<Transform> monsterList {
		get {
			if (_monsterList == null)
				_monsterList = new List<Transform> ();
			return _monsterList;
		}
	}

	//path : list of vector3 positions the monster has to pass through
	List<Vector3> path;

	void Start () {
		type = MonsterManager.monsters [typeNumber];

		monsterList.Add(transform);
		transform.parent = GameManager.Instance.monsterHolder;

		hp = type.maxHp;
		hpBar.maxHP = type.maxHp;
		predictiveHP = hp;

		path = new List<Vector3> ();
		Transform[] spawnKids = origin.GetChild (0).GetComponentsInChildren<Transform> ();
		for (int i = 1; i < spawnKids.Length; i++)
			path.Add (spawnKids [i].position);
	}

	void Update () {
		Move();
	}

	void Move () {
		//The full distance you have to move this frame
		float moveDistance = type.speed * GameManager.Instance.tileSize * TimeManager.scaledDeltaTime;
		distanceWalked += moveDistance;

		while (moveDistance > 0) {
			//Calculate either moveDistance, or the distance to the next path step
			float partialMoveDistance = Mathf.Min (moveDistance, Vector3.Distance (transform.position, path [currentStep])); 
			moveDistance -= partialMoveDistance;

			transform.LookAt(path[currentStep]);
			
			transform.position += Vector3.Normalize(path[currentStep]-transform.position) * partialMoveDistance;

			if (Vector3.Distance(transform.position, path[currentStep]) <= Mathf.Epsilon)
				currentStep ++;		

			if (currentStep == path.Count)
				Respawn();
			moveDistance -= .0001f; //Just so that we never have an infinite loop
		}
	}

	void Respawn () {
		LivesManager.LoseLife ();
		transform.position = path[0];
		currentStep = 1;
		distanceWalked = 0;
	}

	public void Damage (float damage) {
		hp -= damage;
		hpBar.setHP(hp);
		if (hp <= 0)
			Death ();
	}

	public void PredictiveDamage (float damage) {
		predictiveHP -= damage;
	}

	void Death () {
		GoldManager.AddGold (type.reward);
		monsterList.Remove (transform);

		if (WaveManager.wavesAreOver && monsterList.Count == 0)
			FlowManager.Victory();
		
		Destroy (gameObject);
	}
}
