using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class WaveManager {
	//Reference to GameManager
	static GameManager gm;

	//Timers
	static float timeToNextMonster;
	static float timeToNextWave;
	static bool waitingForNextWave = true;
	static int waveNb = 1;

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

	//Subwave : a subset of a wave, containing only monsters of the same type, spawning from the same place
	[System.Serializable] public struct SubWave {
		public int type;
		public int amount;
		public int spawner;
		public SubWave (int type, int amount, int spawner) {
			this.type = type;
			this.amount = amount;
			this.spawner = spawner;
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
		timeToNextWave -= Time.deltaTime * TimeManager.timeScale;
		if (timeToNextWave <= 1 && waitingForNextWave)
			DisplayWaveMessage (true);
		if (timeToNextWave <= 0 && waitingForNextWave)
			DisplayWaveMessage (false);
		return (timeToNextWave <= 0);
	}

	///True if a monster is ready to be spawned
	static bool NextMonsterReady () {
		timeToNextMonster -= Time.deltaTime * TimeManager.timeScale;
		return (timeToNextMonster <= 0);
	}

	static void SpawnMonster () {
		SubWave sw = waves [0].subWaves[0];
		GameObject mon = gm._Instantiate (gm.monsters[sw.type].prefab, gm.spawners[sw.spawner].position, Quaternion.identity, gm.monsterHolder);
		mon.GetComponent<Monster> ().origin = gm.spawners [sw.spawner];
		DecrementWaves ();
	}

	///Reduces the amount of monsters in (wave zero, subwave zero) by one. Removes empty waves and subwaves.
	static void DecrementWaves () {
		SubWave subwaveZero = waves [0].subWaves[0];
		waves[0].subWaves[0] = new SubWave (subwaveZero.type, subwaveZero.amount - 1, subwaveZero.spawner);//Remove one monster from the first subwave

		if (waves[0].subWaves[0].amount <= 0)//Delete first subwave if it is empty
			waves[0].subWaves.RemoveAt(0);

		if (waves[0].subWaves.Count == 0) {//Delete first wave if it is empty
			waves.RemoveAt (0);
			waitingForNextWave = true;
			timeToNextWave = gm.timeBetweenWaves;
			waveNb ++;
		}
		
		timeToNextMonster += gm.timeBetweenMonsters;
	}

	static void DisplayWaveMessage (bool mode) {
		gm.waveMessage.GetComponent<Text>().text = "Wave " + waveNb;
		gm.waveMessage.SetActive(mode);
	}
}