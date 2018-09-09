using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour {
	[Header("Balancing")]
	public int typeNumber;
	[SerializeField] TargetPriority targetPriority;

	//State
	[HideInInspector] public TowerManager.TowerType type;
	[HideInInspector] public bool purchaseInProgress = false;
	float remainingReloadTime;
	public Transform target;

	//Storage
	List<Transform> monstersInRange = new List<Transform>();

	//Declarations
	public enum TargetPriority {First, Last, LowHP, Random};


	void Start () {
		type = TowerManager.towers [typeNumber];
	}

	void Update () {
		if (!purchaseInProgress && Loaded() && AcquireTarget())
			Shoot ();
	}

	/// Returns true if target has been acquired
	bool AcquireTarget () {
		if (Monster.monsterList.Count == 0)
			return false;

		monstersInRange.Clear ();
		foreach (Transform m in Monster.monsterList)
			if (Vector3.Distance (transform.position, m.position) < type.range && m.GetComponent<Monster>().predictiveHP > 0)
				monstersInRange.Add (m);

		if (monstersInRange.Count != 0)
			PickTarget();

		return (monstersInRange.Count > 0);
	}

	//Selects target depending on selected priority mode
	void PickTarget () {
		switch (targetPriority) {
		case TargetPriority.First:
			target = monstersInRange [0];
			foreach (Transform t in monstersInRange)
				if (t.GetComponent<Monster> ().distanceWalked > target.GetComponent<Monster> ().distanceWalked)
					target = t;
			break;
		case TargetPriority.Last:
			target = monstersInRange [0];
			foreach (Transform t in monstersInRange)
				if (t.GetComponent<Monster> ().distanceWalked < target.GetComponent<Monster> ().distanceWalked)
					target = t;
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

	void Shoot () {
		//Spawn bullet, set its target
		GameObject bullet = Instantiate (type.bulletPrefab, transform.position + type.bulletSpawnPoint, Quaternion.identity, transform.GetChild(1));
		bullet.GetComponent<Bullet> ().targetTransform = target;
		bullet.GetComponent<Bullet> ().type = type;
		target.GetComponent<Monster> ().PredictiveDamage (type.damage);
		Reload();
	}

	///Return true if tower is loaded, reload otherwise
	bool Loaded () {
		if (remainingReloadTime <= 0)
			return true;
		else {
			remainingReloadTime -= Time.deltaTime * TimeManager.timeScale;
			return false;
		}
	}

	///Restart reloading sequence
	public void Reload () {
		remainingReloadTime = type.reloadingTime;
	}
}
