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
	[SerializeField] GameObject _timeManager;

	//Arbitrary balancing
	float spawnInterval = 1f;

	//References
	TimeManager timeManager = TimeManager.Instance;

	//State
	float timeToNextWave;
	float timeToNextMonster;
	public int monstersLeftInWave;

	[System.Serializable] public struct Wave
	{
		public int time;
		public int amount;
	}

	void Start ()
	{
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
		if (timeToNextWave <= 0)
			return true;
		else
		{
			timeToNextWave -= Time.deltaTime * _timeManager.GetComponent<TimeManager>().timeScale;
			return false;
		}
	}

	///True if Every monster in current wave has been spawned
	bool CurrentWaveIsOver ()
	{
		if (monstersLeftInWave == 0)
		{
			waves.RemoveAt (0);
			if (!WavesAreOver())
			{
				monstersLeftInWave = waves [0].amount;
				timeToNextWave += waves [0].time;
			}
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
			timeToNextMonster -= Time.deltaTime * _timeManager.GetComponent<TimeManager>().timeScale;
			return false;
		}
	}

	void SpawnMonster ()
	{
		Instantiate (monsterPrefab, transform.position, Quaternion.identity);
		monstersLeftInWave--;
		timeToNextMonster += spawnInterval;
	}
}