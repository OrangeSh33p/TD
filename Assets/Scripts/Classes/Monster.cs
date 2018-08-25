using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour {
	[Header("Boring Variables")]
	public Slider hpBar;

	//State
	float hp;
	[HideInInspector] public float predictiveHP;

	Vector3 origin;
	int currentStep = 1;
	[HideInInspector] public float distanceWalked;


	//References
	GoldManager goldManager = GoldManager.Instance;
	LivesManager livesManager = LivesManager.Instance;
	MonsterManager monsterManager = MonsterManager.Instance;
	TimeManager timeManager = TimeManager.Instance;


	//Declarations
	static List<Transform> _monsterList;
	public static List<Transform> monsterList {
		get {
			if (_monsterList==null)
				_monsterList = new List<Transform> ();
			return _monsterList;
		}
	}

	static List<Vector3> _path;
	public static List<Vector3> path {
		get {
			if (_path == null) {
				_path = new List<Vector3> ();
				Transform[] tmp = MonsterManager.Instance.transform.GetChild(0).GetComponentsInChildren<Transform> ();

				for (int i = 0; i < tmp.Length; i++)
					_path.Add (tmp[i].transform.position);
			}
			return _path;
		}
	}


	void Start () {
		monsterList.Add(transform);
		transform.parent = monsterManager.transform;

		hp = monsterManager.maxHp;
		predictiveHP = hp;

		origin = Spawner.Instance.transform.position;
	}

	void Update () {
		Move();
	}

	void Move () {
		float moveDistance = monsterManager.speed * Time.deltaTime * timeManager.timeScale; //The full distance you have to move this frame
		distanceWalked += moveDistance;

		while (moveDistance > 0) {
			float partialMoveDistance = Mathf.Min (moveDistance, Vector3.Distance (transform.position, path [currentStep]));
			moveDistance -= partialMoveDistance;

			transform.LookAt (path[currentStep]);
			transform.position += transform.forward * partialMoveDistance;

			if (Vector3.Distance(transform.position, path[currentStep]) == 0)
				currentStep ++;		

			if (currentStep == path.Count)
				Respawn();
			moveDistance -= .0001f;
		}
	}

	void Respawn () {
		livesManager.LoseLife ();
		transform.position = origin;
		currentStep = 1;
		distanceWalked = 0;
	}

	public void PredictiveDamage (float damage) {
		predictiveHP -= damage;
	}

	public void Damage (float damage) {
		hp -= damage;
		hpBar.value = hp / monsterManager.maxHp;
		if (hp <= 0)
			Death ();
	}

	void Death () {
		goldManager.AddGold (monsterManager.reward);
		monsterList.Remove (this.transform);

		if (Spawner.Instance.WavesAreOver && monsterList.Count == 0)
			GameManager.Instance.victory = true;
		
		Destroy (gameObject);
	}
}
