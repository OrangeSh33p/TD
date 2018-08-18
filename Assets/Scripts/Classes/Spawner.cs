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

	//State
	[HideInInspector] public bool wavesAreOver = false;

	//Arbitrary balancing
	float spawnInterval = 1f;

	[System.Serializable] public struct Wave
	{
		public int time;
		public int amount;
	}

	void Start ()
	{
		StartCoroutine (StartWaves ());
	}

	IEnumerator StartWaves ()
	{
		//Start wave zero, then destroy it. Rinse and repeat
		while (waves.Count > 0)
		{
			Wave w = waves [0];
			yield return new WaitForSeconds (w.time);
			for (int i=0; i<w.amount; i++)
			{
				Instantiate (monsterPrefab, transform.position, Quaternion.identity);
				yield return new WaitForSeconds (spawnInterval);
			}
			waves.RemoveAt (0);
		}
		wavesAreOver = true;
	}
}