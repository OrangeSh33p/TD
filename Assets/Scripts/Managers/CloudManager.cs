using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;
	static Transform ch = gm.cloudHolder;

	static float timeLeft;

	public static void _Update () {
		MoveCloud ();
		if (timeLeft < 0) {
			SpawnCloud ();
			timeLeft = gm.cloudSpawnDelay;
		} else
			timeLeft -= Time.deltaTime * TimeManager.timeScale;
	}

	static void SpawnCloud () {
		float zOffset = Random.Range (0, gm.cloudZRandom);
		gm._Instantiate (gm.cloudPrefabs [Random.Range (0, gm.cloudPrefabs.Count - 1)], gm.cloudSpawnPos + new Vector3 (0, 0, zOffset), Quaternion.identity, ch.transform);
	}

	static void MoveCloud () {
		foreach (Transform cloud in ch.GetComponentsInChildren<Transform>()) {
			if (cloud != ch) 	{
				cloud.position += Time.deltaTime * gm.cloudSpeed * TimeManager.timeScale * new Vector3 (-1, 0, 0);
				if (cloud.position.x < gm.cloudMinX)
					gm._Destroy (cloud.gameObject);
			}
		}
	}
}
