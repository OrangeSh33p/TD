using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> {

	[Space(10)]
	[Header("RESOURCES")]
	public int startGold;
	public int maxLives;

	[Space(10)]
	[Header("GRID")]
	public float tileSize;
	public Vector2Int gridSize;
	public List<Vector2Int> path;
	public bool createGrid;
	public Transform greenHolder;
	public Transform pathHolder;
	public GameObject tileFree;
	public GameObject tilePath;

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

	[Space(10)]
	[Header("SPAWNER")]
	public List<Transform> spawners;
	public GameObject waveMessage;
	public float timeBeforeFirstWave;
	public float timeBetweenWaves; //from the LAST monster of a wave to the FIRST monster of the next one
	public float timeBetweenMonsters;
	public List<WaveManager.Wave> waves = new List<WaveManager.Wave> ();

	[Space(10)]
	[Header("HP Bar")]
	public Slider chunkPrefab;
	public float chunkHP;
	public float spaceWidth;
	public Image backGroundPrefab;
	public float BGXOffset;
	public float BGYOffset;
	public float BGLeftMargin;
	public float BGTopMargin;

	[Space(10)]
	[Header("TIME")]
	public float pauseSpeed;
	public float playSpeed;
	public float fastSpeed;
	public float superFastSpeed;

	[Space(10)]
	[Header("UI")]
	public float textDisplayDuration;
	public Text goldText;
	public Text livesText;
	public Text insufficientGoldText;
	public Text cantBuildOnPathText;
	public Text cantBuildOnTowerText;
	public Button pauseButton;
	public Button playButton;
	public Button fastButton;
	public Button superFastButton;
	public Image superFastImage;
	public GameObject gameoverOverlay;
	public GameObject victoryOverlay;

	[Space(10)]
	[Header ("CLOUDS")]
	public List<Transform> cloudHolders;
	public List<float> cloudSpeed;
	public List<GameObject> cloudPrefabs;
	public Vector3 cloudSpawnPos;
	public int cloudZRandom;
	public int cloudMinX;
	public int cloudSpawnDelay;

	void Start () {
		InitManagers();
		StartManagers ();
	}

	void Update ()	{
		UpdateManagers ();
	}
	
	///<summary>Calls _Init method in all managers (used to assign all scene objects on restart)</summary>
	void InitManagers () {
		TimeManager._Init();
		FlowManager._Init();
		CloudManager._Init();
		GoldManager._Init();
		GridManager._Init();
		LivesManager._Init();
		MonsterManager._Init();
		TowerManager._Init();
		UIManager._Init();
		WaveManager._Init();
	}

	///<summary>Calls _Start method in all managers</summary>
	void StartManagers () {
		GoldManager._Start();
		GridManager._Start();
		LivesManager._Start();
		TimeManager._Start();
		TowerManager._Start();
		WaveManager._Start();
	}

	///<summary>Calls _Update method in all managers</summary>
	void UpdateManagers () {
		CloudManager._Update();
		FlowManager._Update();
		WaveManager._Update();
	}

	///<summary>delegate StartCoroutine method that can be used by classes that do not inherit monoBehavior</summary>
	public void _StartCoroutine (IEnumerator target) {
		StartCoroutine(target);
	}

	///<summary>delegate Instantiate method that can be used by classes that do not inherit monoBehavior</summary>
	public GameObject _Instantiate (GameObject original, Vector3 position, Quaternion rotation, Transform parent) {
		return Instantiate(original, position, rotation, parent);
	}

	///<summary>delagate Destroy method that can be used by classes that do not inherit monoBehavior</summary>
	public void _Destroy (GameObject target) {
		Destroy (target);
	}
}
