using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CloudManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;
	static List<Transform> ch = gm.cloudHolders;

	/*[System.Serializable] public struct cloudGenerator {
		public Vector3 cloudSpawnPos;
		public int cloudSpeed;
		public int cloudZRandom;
		public int cloudMinX;
		public int cloudSpawnDelay;
	}*/

	static float timeBeforeNextCloud;

	public static void _Update () {
		MoveCloud ();
		if (timeBeforeNextCloud < 0) {
			SpawnCloud ();
			timeBeforeNextCloud = gm.cloudSpawnDelay;
		} else
			timeBeforeNextCloud -= Time.deltaTime * TimeManager.timeScale;
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
			foreach (Transform cloud in ch[i].GetComponentsInChildren<Transform>()) {
				if (cloud != ch[i]) 	{
					cloud.position += Time.deltaTime * gm.cloudSpeed[i] * TimeManager.timeScale * new Vector3 (-1, 0, 0);
					if (cloud.position.x < gm.cloudMinX)
						gm._Destroy (cloud.gameObject);
				}
			}
		}
	}
}
