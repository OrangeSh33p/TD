using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {
	[Header("END GAME")]
	public GameObject gameoverOverlay;
	public GameObject victoryOverlay;

	[Header("TEXT DISPLAYS")]
	public float textDisplayDuration;
	public Text goldText;
	public GameObject insufficientGoldText;
	public Text livesText;

	[Header("GOLD")]
	public int startGold;

	[Header("LIVES")]
	public int maxLives;

	[Header ("CLOUDS")]
	public Transform cloudHolder;
	public List<GameObject> cloudPrefabs;
	public Vector3 cloudSpawnPos;
	public int cloudSpeed;
	public int cloudZRandom;
	public int cloudMinX;
	public int cloudSpawnDelay;

	[Header("TIME")]
	public float pauseSpeed;
	public float playSpeed;
	public float fastSpeed;
	public float superFastSpeed;
	public Button PauseButton;
	public Button PlayButton;
	public Button FastButton;
	public Button SuperFastButton;
	public Image SuperFastImage;

	[Header("TOWERS")]
	public Transform towerHolder;
	public List<TowerManager.TowerType> towers;

	[Header ("MONSTER")]
	public Transform monsterHolder;
	public List<MonsterManager.monsterType> monsters;

	void Start () {
		GoldManager._Start();
		LivesManager._Start();
		TimeManager._Start();
		TowerManager._Start();
	}

	void Update ()	{
		GoldManager._Update();
		CloudManager._Update();
		TimeManager._Update();
		FlowManager._Update();
	}

	public GameObject _Instantiate (GameObject original, Vector3 position, Quaternion rotation, Transform parent) {
		return Instantiate(original, position, rotation, parent);
	}

	public void _Destroy (GameObject target) {
		Destroy (target);
	}
}
