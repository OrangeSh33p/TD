using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour 
{
	//State
	[HideInInspector] public bool purchaseInProgress = false;
	float remainingReloadTime;
	Transform target;

	//Storage
	List<Transform> monstersInRange = new List<Transform>();

	//References
	MonsterManager monsterManager = MonsterManager.Instance;
	TowerManager towerManager = TowerManager.Instance;
	TimeManager timeManager = TimeManager.Instance;

	public enum TargetPriority {First, Last, LowHP, Random};

	public TargetPriority targetPriority;

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
			if (Vector3.Distance (transform.position, m.position) < towerManager.range && m.GetComponent<Monster>().predictiveHP > 0)
				monstersInRange.Add (m);

		if (monstersInRange.Count != 0)
			PickTarget();

		return (monstersInRange.Count > 0);
	}

	void PickTarget ()
	{
		switch (targetPriority)
		{
		case TargetPriority.First:
			target = monstersInRange [0];
			break;
		case TargetPriority.Last:
			target = monstersInRange [monstersInRange.Count-1];
			break;
		case TargetPriority.Random:
			target = monstersInRange [Random.Range (0, monstersInRange.Count)];
			break;
		case TargetPriority.LowHP:
			target = monstersInRange [0];
			foreach (Transform t in monstersInRange)
				if (t.GetComponent<Monster> ().predictiveHP < target.GetComponent<Monster> ().predictiveHP)
					target = t;
			break;
		}
	}

	void Shoot ()
	{
		//Spawn bullet, set its target
		GameObject bullet = Instantiate (towerManager.bulletPrefab, transform.position + towerManager.bulletSpawnPoint, Quaternion.identity, transform.GetChild(1));
		bullet.GetComponent<Bullet> ().targetTransform = target;
		target.GetComponent<Monster> ().PredictiveDamage (towerManager.damage);
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
