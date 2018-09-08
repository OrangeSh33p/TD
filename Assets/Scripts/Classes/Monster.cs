using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
	[Header("Balancing")]
	public int typeNumber;

	[Header("Boring Variables")]
	public Slider hpBar;

	//State
	float hp;
	[HideInInspector] public float predictiveHP;
	Vector3 origin;
	int currentStep = 1;
	[HideInInspector] public float distanceWalked;
	[HideInInspector] public MonsterManager.monsterType type;

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
	static List<Vector3> _path;
	public static List<Vector3> path {
		get {
			if (_path == null) {
				_path = new List<Vector3> ();
				Transform[] tmp = GameManager.Instance.monsterHolder.GetChild(0).GetComponentsInChildren<Transform> ();

				for (int i = 0; i < tmp.Length; i++)
					_path.Add (tmp[i].transform.position);
			}
			return _path;
		}
	}


	void Start () {
		type = MonsterManager.monsters [typeNumber];

		monsterList.Add(transform);
		transform.parent = GameManager.Instance.monsterHolder;

		hp = type.maxHp;
		predictiveHP = hp;

		origin = Spawner.Instance.transform.position;
	}

	void Update () {
		Move();
	}

	void Move () {
		float moveDistance = type.speed * Time.deltaTime * TimeManager.timeScale; //The full distance you have to move this frame
		distanceWalked += moveDistance;

		while (moveDistance > 0) {
			float partialMoveDistance = Mathf.Min (moveDistance, Vector3.Distance (transform.position, path [currentStep])); //MoveDistance, or the distance to the next path step
			moveDistance -= partialMoveDistance;

			transform.LookAt (path[currentStep]);
			transform.position += transform.forward * partialMoveDistance;

			if (Vector3.Distance(transform.position, path[currentStep]) <= Mathf.Epsilon)
				currentStep ++;		

			if (currentStep == path.Count)
				Respawn();
			moveDistance -= .0001f; //Just so that we never have an infinite loop
		}
	}

	void Respawn () {
		LivesManager.LoseLife ();
		transform.position = origin;
		currentStep = 1;
		distanceWalked = 0;
	}

	public void Damage (float damage) {
		hp -= damage;
		hpBar.value = hp / type.maxHp;
		if (hp <= 0)
			Death ();
	}

	public void PredictiveDamage (float damage) {
		predictiveHP -= damage;
	}

	void Death () {
		GoldManager.AddGold (type.reward);
		monsterList.Remove (transform);

		if (Spawner.Instance.WavesAreOver && monsterList.Count == 0)
			FlowManager.Victory();
		
		Destroy (gameObject);
	}
}
