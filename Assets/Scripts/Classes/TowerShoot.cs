using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour 
{
	//State
	[HideInInspector] public bool loaded = true;

	//Storage
	List<Transform> monstersInRange = new List<Transform>();

	//References
	MonsterManager monsterManager = MonsterManager.Instance;
	TowerManager towerManager = TowerManager.Instance;

	void Start ()
	{
		towerManager = TowerManager.Instance; 
		monsterManager = MonsterManager.Instance;
	}

	void Update ()
	{
		if (loaded && AcquireTarget())
			Shoot ();
	}

	/// Returns true if target has been acquired
	bool AcquireTarget ()
	{
		List<Transform> monsters =	monsterManager.MonsterList();

		if (monsters.Count == 0)
			return false;

		monstersInRange.Clear ();
		foreach (Transform m in monsters)
			if (Vector3.Distance (transform.position, m.position) < towerManager.range)
				monstersInRange.Add (m);

		return (monstersInRange.Count > 0);
	}

	void Shoot ()
	{
		//Spawn bullet, set its target
		Instantiate (towerManager.bulletPrefab, transform.position + towerManager.bulletSpawnPoint, Quaternion.identity, transform.GetChild(1)).GetComponent<Bullet> ().Initialize(monstersInRange [0]);
		StartCoroutine (Reload ());
	}


	public IEnumerator Reload ()
	{
		loaded = false;
		yield return new WaitForSeconds (towerManager.reloadingTime);
		loaded = true;
		yield return null;
	}
}
