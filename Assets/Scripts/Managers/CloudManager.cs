using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudManager {
	//Reference to GameManager
	static GameManager gm;

	static float timeBeforeNextCloud;
	static List<Transform> ch;

	public static void _Init() {
		gm = GameManager.Instance;
		ch = gm.cloudHolders;
	}

	public static void _Update () {
		MoveCloud ();
		if (timeBeforeNextCloud < 0) {
			SpawnCloud ();
			timeBeforeNextCloud = gm.cloudSpawnDelay;
		} else
			timeBeforeNextCloud -= TimeManager.scaledDeltaTime;
	}

	static void SpawnCloud () {
		float zOffset = Random.Range (0, gm.cloudZRandom);
		gm._Instantiate (
			gm.cloudPrefabs [Random.Range (0, gm.cloudPrefabs.Count - 1)], 
			gm.cloudSpawnPos + new Vector3 (0, 0, zOffset), 
			Quaternion.identity, 
			ch[Random.Range(0,ch.Count)].transform
		);
	}

	static void MoveCloud () {
		for(int i=0;i<ch.Count;i++) {
			foreach (Transform cloud in ch[i]) {
				if (cloud != ch[i]) {
					cloud.position += gm.cloudSpeed[i]  * new Vector3 (-1, 0, 0) * TimeManager.scaledDeltaTime;
					if (cloud.position.x < gm.cloudMinX)
						gm._Destroy (cloud.gameObject);
				}
			}
		}
	}
}
