using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WaveManager {
	//Reference to GameManager
	static GameManager gm;

	//Timers
	static float timeToNextMonster;
	static float timeToNextWave;

	//Checks wether every wave has been spawned
	public static bool wavesAreOver {
		get  {
			return (waves.Count == 0);
		}
	}

	static List<Wave> waves;
	//Wave : a group of monsters spawning at the same time. Spawns subwaves one by one in the given order
	[System.Serializable] public struct Wave {
		public List<SubWave> subWaves;
		public Wave (List<SubWave> subWaves) {
			this.subWaves = subWaves;
		}
	}

	//Subwave : a subset of a wave, containing only monsters of the same type
	[System.Serializable] public struct SubWave {
		public int type;
		public int amount;
		public SubWave (int type, int amount) {
			this.type = type;
			this.amount = amount;
		}
	}

	public static void _Start () {
		gm = GameManager.Instance;
		waves = gm.waves;
		timeToNextWave = gm.timeBeforeFirstWave;
	}

	public static void _Update () {
		if (!wavesAreOver && NextWaveReady () && NextMonsterReady ())
			SpawnMonster ();
	}

	///True if a wave is currently being spawned
	static bool NextWaveReady () {
		if (timeToNextWave <= 0)
			return true;
		else {
			timeToNextWave -= Time.deltaTime * TimeManager.timeScale;
			return false;
		}
	}

	///True if a monster is ready to be spawned
	static bool NextMonsterReady () {
		if (timeToNextMonster <= 0)
			return true;
		else {
			timeToNextMonster -= Time.deltaTime * TimeManager.timeScale;
			return false;
		}
	}

	static void SpawnMonster () {
		gm._Instantiate (gm.monsters[waves[0].subWaves[0].type].prefab, gm.spawner.position, Quaternion.identity, gm.monsterHolder);
		DecrementWaves ();
	}

	///Reduces the amount of monsters in (wave zero, subwave zero) by one. Removes empty waves and subwaves.
	static void DecrementWaves () {
		waves[0].subWaves[0] = new SubWave (waves[0].subWaves[0].type, waves[0].subWaves[0].amount - 1);//Remove one monster from the first subwave

		if (waves[0].subWaves[0].amount <= 0)//Delete first subwave if it is empty
			waves[0].subWaves.RemoveAt(0);

		if (waves[0].subWaves.Count == 0) {//Delete first wave if it is empty
			waves.RemoveAt (0);
			timeToNextWave = gm.timeBetweenWaves;
		}
		
		timeToNextMonster += gm.timeBetweenMonsters;
	}
}