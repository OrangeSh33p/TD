using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoSingleton <Spawner> 
{
	[Header("Balancing")]
	[SerializeField] List<Wave> waves = new List<Wave>();

	[Header("Boring variables")]
	[SerializeField] GameObject monsterPrefab;

	//Arbitrary balancing
	float spawnInterval = 1f;

	//References
	TimeManager timeManager;

	//State
	float timeToNextMonster;
	public int monstersLeftInWave;

	[System.Serializable] public struct Wave
	{
		public float time;
		public int amount;
		public Wave(int amt, float t){
			amount = amt;
			time = t;
		}
	}

	void Start ()
	{
		timeManager = TimeManager.Instance;

		monstersLeftInWave = waves [0].amount;
	}

	void Update ()
	{
		if (!WavesAreOver () && NextWaveReady () && !CurrentWaveIsOver () && NextMonsterReady ())
			SpawnMonster ();
	}

	///True if the last wave has been completely spawned
	public bool WavesAreOver ()
	{
		return (waves.Count == 0);
	}

	///True if a wave is currently being spawned
	bool NextWaveReady ()
	{
		if (waves[0].time <= 0)
			return true;
		else
		{
			waves [0] = new Wave (waves [0].amount, waves [0].time - Time.deltaTime * timeManager.timeScale);
			return false;
		}
	}

	///True if Every monster in current wave has been spawned
	bool CurrentWaveIsOver ()
	{
		if (waves[0].amount == 0)
		{
			waves.RemoveAt (0);
			return true;
		}
		else
			return false;
	}

	///True if a monster is ready to be spawned
	bool NextMonsterReady ()
	{
		if (timeToNextMonster <= 0)
			return true;
		else
		{
			timeToNextMonster -= Time.deltaTime * timeManager.timeScale;
			return false;
		}
	}

	void SpawnMonster ()
	{
		Instantiate (monsterPrefab, transform.position, Quaternion.identity);
		waves [0] = new Wave (waves [0].amount - 1, waves [0].time);
		timeToNextMonster += spawnInterval;
	}
}