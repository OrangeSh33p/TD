using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {
	[Header ("Boring Variables")]
	[SerializeField] List<GameObject> clouds;
	[SerializeField] Vector3 spawnPos;
	[SerializeField] int speed;
	[SerializeField] int zRandom;
	[SerializeField] int minX;
	[SerializeField] int spawnDelay;
	float timeLeft;

	void Update () {
		MoveCloud ();
		if (timeLeft < 0)
		{
			SpawnCloud ();
			timeLeft = spawnDelay;
		} else
			timeLeft -= Time.deltaTime * TimeManager.Instance.timeScale;
	}

	void SpawnCloud () {
		float zOffset = Random.Range (0, zRandom);
		Instantiate (clouds [Random.Range (0, clouds.Count - 1)], spawnPos + new Vector3 (0, 0, zOffset), Quaternion.identity, transform);
	}

	void MoveCloud () {
		foreach (Transform cloud in transform.GetComponentsInChildren<Transform>()) {
			if (cloud != transform) 	{
				cloud.position += Time.deltaTime * speed * TimeManager.Instance.timeScale * new Vector3 (-1, 0, 0);
				if (cloud.position.x < minX)
					Destroy (cloud.gameObject);
			}
		}
	}

}
