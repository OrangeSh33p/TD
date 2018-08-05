using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
	//State
	bool loaded = true;
	bool purchaseInProgress = false;

	//Storage
	Ray ray;
	RaycastHit hit;
	List<Transform> monstersInRange = new List<Transform>();

	//References
	GoldManager goldManager = GoldManager.Instance;
	MonsterManager monsterManager = MonsterManager.Instance;
	TowerManager towerManager = TowerManager.Instance;
	GridManager gridManager = GridManager.Instance;


	void Start ()
	{
		towerManager = TowerManager.Instance; 
		monsterManager = MonsterManager.Instance;//Why need assignation and not gamemanager ?

		transform.parent = towerManager.transform;
	}

	void Update ()
	{

		if (purchaseInProgress)
			ContinuePurchase();

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

	#region shooting sequence
	void Shoot ()
	{
		//Spawn bullet, set its target
		Instantiate (towerManager.bulletPrefab, transform.position, Quaternion.identity, transform.GetChild(1)).GetComponent<Bullet> ().Initialize(monstersInRange [0]);
		StartCoroutine (Reload ());
	}


	IEnumerator Reload ()
	{
		loaded = false;
		yield return new WaitForSeconds (towerManager.reloadingTime);
		loaded = true;
		yield return null;
	}
	#endregion

	#region purchasing sequence
	public void StartPurchase ()
	{
		purchaseInProgress = true;
		towerManager.SetCancelButton (true);
		loaded = false;

		//if mouse is above sth on layer 8 (the floor), move self to under the cursor
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
			transform.position = hit.point;
	}


	void ContinuePurchase ()
	{

		//if mouse is above sth on layer 8 (the floor), move self to under the cursor, snap to grid
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
		{
			transform.position = hit.point;
		}

		Vector2Int gridPos = gridManager.Snap (gameObject);
		//If player clicked and there is no tower on this tile and player has enough money, complete the purchase (check for money at the end because it also subtracts the money)
		if (Input.GetMouseButtonUp (0) 
			&& gridManager.TileIsFree(gridPos)
			&& goldManager.AddGold (-towerManager.price))
		{
			gridManager.grid[gridPos.x, gridPos.y] = GridManager.Tile.tower;
			EndPurchase(); 
		}
	}	


	void EndPurchase ()
	{
		purchaseInProgress = false;
		towerManager.SetCancelButton (false);
		StartCoroutine (Reload ());
	}
	#endregion
}
