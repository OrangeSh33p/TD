using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour 
{
	//State
	[HideInInspector] public bool purchaseInProgress = false;
	float remainingReloadTime;

	//Storage
	List<Transform> monstersInRange = new List<Transform>();

	//References
	MonsterManager monsterManager = MonsterManager.Instance;
	TowerManager towerManager = TowerManager.Instance;
	TimeManager timeManager = TimeManager.Instance;

	void Start ()
	{
		towerManager = TowerManager.Instance; 
		monsterManager = MonsterManager.Instance;
	}

	void Update ()
	{
		if (!purchaseInProgress && Loaded() && AcquireTarget())
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
		Instantiate (towerManager.bulletPrefab, transform.position + towerManager.bulletSpawnPoint, Quaternion.identity, transform.GetChild(1)).GetComponent<Bullet> ().targetTransform = monstersInRange[0];
		Reload();
	}

	///Returns true if tower is loaded, reloads otherwise
	bool Loaded ()
	{
		if (remainingReloadTime <= 0)
			return true;
		else
		{
			remainingReloadTime -= Time.deltaTime * timeManager.timeScale;
			return false;
		}
	}

	///Restarts reloading sequence
	public void Reload ()
	{
		remainingReloadTime = towerManager.reloadingTime;
	}
}
