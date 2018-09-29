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
	static bool waitingForNextWave;
	static int waveNb;

	///<summary>Checks wether every wave has been spawned</summary>
	public static bool wavesAreOver {
		get  {
			return (waves.Count == 0);
		}
	}

	///<summary>The list of everything that needs to be spawned this level</summary>
	static List<Wave> waves;
	///<summary>A group of monsters spawning at the same time. Contains Subwaves that are instanciated one by one in order</summary>
	[System.Serializable] public struct Wave {
		public List<SubWave> subWaves;

		public Wave (List<SubWave> subWaves) {
			this.subWaves = subWaves;
		}
	}

	///<summary>A subset of a wave, containing only monsters of the same type, spawning from the same place</summary>
	[System.Serializable] public struct SubWave {
		public int type;
		public int amount;
		public int spawner;

		public SubWave (int type, int amount, int spawner) {
			this.type = type;
			this.amount = amount;
			this.spawner = spawner;
		}

		public void SetAmount (int newAmount) {
			this.amount = newAmount;
		}
	}

	public static void _Init () {
		waitingForNextWave = true;
		waveNb = 1;
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

	///<summary>True if a wave is currently being spawned</summary>
	static bool NextWaveReady () {
		timeToNextWave -= TimeManager.scaledDeltaTime;
		if (timeToNextWave <= 1 && waitingForNextWave)
			DisplayWaveMessage (true);
		if (timeToNextWave <= 0 && waitingForNextWave)
			DisplayWaveMessage (false);
		return (timeToNextWave <= 0);
	}

	///<summary>True if a monster is ready to be spawned</summary>
	static bool NextMonsterReady () {
		timeToNextMonster -= TimeManager.scaledDeltaTime;
		return (timeToNextMonster <= 0);
	}

	static void SpawnMonster () {
		SubWave sw = waves[0].subWaves[0];
		GameObject monster = gm._Instantiate (gm.monsters[sw.type].prefab, gm.spawners[sw.spawner].position, Quaternion.identity, gm.monsterHolder);
		monster.GetComponent<Monster> ().origin = gm.spawners [sw.spawner];
		DecrementWaves ();
	}

	///<summary>Reduces the amount of monsters in (wave zero, subwave zero) by one. Removes empty waves and subwaves</summary>
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