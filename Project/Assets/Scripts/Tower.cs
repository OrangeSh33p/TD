using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
	public float range;
	public float shootingRate;
	public float damage;
	public float bulletSpeed;
	public GameObject bulletPrefab;

	List<GameObject> monstersInRange = new List<GameObject>();
	bool loaded;


	void Start ()
	{
		loaded = true;
	}


	void Update ()
	{
		if (loaded && AcquireTarget())
		{
				Shoot ();
				StartCoroutine (Reload ());
		}
	}


	bool AcquireTarget ()
	{
		List<GameObject> monsters =	MonsterManager.Instance.GetMonsters();

		if (monsters.Count == 0)
			return false;

		monstersInRange.Clear ();

		foreach (GameObject m in monsters)
		{
			if (Vector3.Distance (transform.position, m.transform.GetChild(0).position) < range)
				monstersInRange.Add (m.transform.GetChild(0).gameObject);
		}

		return (monstersInRange.Count > 0);
	}


	void Shoot ()
	{
		Bullet b = Instantiate (bulletPrefab, transform.position, Quaternion.identity, transform).GetComponent<Bullet> ();

		b.target = monstersInRange [0];
		b.damage = damage;
		b.speed = bulletSpeed;
	}


	IEnumerator Reload ()
	{
		loaded = false;
		yield return new WaitForSeconds (shootingRate);
		loaded = true;
		yield return null;
	}
}
