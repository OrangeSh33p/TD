using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

	[Space(10)]
	[Header("GOLD")]
	public int startGold;

	[Space(10)]
	[Header("LIVES")]
	public int maxLives;

	[Space(10)]
	[Header("TOWERS")]
	public Transform towerHolder;
	public List<TowerManager.TowerType> towers;
	public Material transparent;
	public Material transparentRed;
	public Material opaque;

	[Space(10)]
	[Header ("MONSTERS")]
	public Transform monsterHolder;
	public List<MonsterManager.monsterType> monsters;

	[Header("SPAWNER")]
	public List<Transform> spawners;
	public GameObject waveMessage;
	public float timeBeforeFirstWave;
	public float timeBetweenWaves; //How long to wait between the last monster of a wave and the first monster of the next one
	public float timeBetweenMonsters;
	public List<WaveManager.Wave> waves = new List<WaveManager.Wave> ();

	[Space(10)]
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

	[Space(10)]
	[Header("END GAME")]
	public GameObject gameoverOverlay;
	public GameObject victoryOverlay;

	[Space(10)]
	[Header("TEXT DISPLAYS")]
	public float textDisplayDuration;
	public Text goldText;
	public GameObject insufficientGoldText;
	public Text livesText;

	[Space(10)]
	[Header ("CLOUDS")]
	public Transform cloudHolder;
	public List<GameObject> cloudPrefabs;
	public Vector3 cloudSpawnPos;
	public int cloudSpeed;
	public int cloudZRandom;
	public int cloudMinX;
	public int cloudSpawnDelay;

	void Start () {
		StartManagers ();
	}

	void Update ()	{
		UpdateManagers ();
	}

	//Calls _Start method in all managers
	void StartManagers () {
		GoldManager._Start();
		LivesManager._Start();
		TimeManager._Start();
		TowerManager._Start();
		WaveManager._Start();
	}

	//Calls _Update method in all managers
	void UpdateManagers () {
		GoldManager._Update();
		CloudManager._Update();
		TimeManager._Update();
		FlowManager._Update();
		WaveManager._Update();
	}

	//Instantiate method that can be used by classes that do not inherit monoBehavior
	public GameObject _Instantiate (GameObject original, Vector3 position, Quaternion rotation, Transform parent) {
		return Instantiate(original, position, rotation, parent);
	}

	//Destroy method that can be used by classes that do not inherit monoBehavior
	public void _Destroy (GameObject target) {
		Destroy (target);
	}
}
