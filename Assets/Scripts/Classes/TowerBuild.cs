using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuild : MonoBehaviour 
{
	//State
	bool purchaseInProgress = false;

	//Storage
	Ray ray;
	RaycastHit hit;

	//References
	GoldManager goldManager;
	TowerManager towerManager = TowerManager.Instance;
	GridManager gridManager = GridManager.Instance;
	TowerShoot _towerShoot;
	//towerShoot can return a value during compilation, even if the gameobject has not been instanciated
	TowerShoot towerShoot 
	{
		get 
		{
			if (_towerShoot==null)
				_towerShoot = GetComponent<TowerShoot> ();
			return _towerShoot;
		}
	}

	static List<Transform> _towerList;
	public static List<Transform> towerList 
	{
		get 
		{
			if (_towerList==null)
				_towerList = new List<Transform> ();
			return _towerList;
		}
	}


	void Start ()
	{
		towerManager = TowerManager.Instance;
		goldManager = GoldManager.Instance;
		gridManager = GridManager.Instance;

		towerList.Add (transform);
		transform.parent = towerManager.transform;
	}

	void Update ()
	{
		if (purchaseInProgress)
			ContinuePurchase();
	}

	void OnDestroy ()
	{
		towerList.Remove (transform);
	}
		
	public void StartPurchase ()
	{
		purchaseInProgress = true;
		towerShoot.purchaseInProgress = true;
		towerManager.SetCancelButton (true);
		SnapUnderCursor ();
	}


	void ContinuePurchase ()
	{
		Vector2Int gridPos = SnapUnderCursor ();

		if (Input.GetMouseButtonUp (0) 						 //If player clicked
			&& gridManager.TileIsFree(gridPos) 			   	 //and there is no tower on this tile
			&& goldManager.AddGold (-towerManager.price))    //and player has enough money (check for money at the end because it also subtracts the money)
		{
			//Set tile and adjacent ones to "tower" in the gridmanager
			gridManager.setTile (gridPos, GridManager.Tile.tower);

			//Complete the purchase
			EndPurchase(); 
		}
	}

	///Snaps the tower on the closest tile under player's mouse position
	Vector2Int SnapUnderCursor ()
	{
		//if mouse is above sth on layer 8 (the floor), move self to under the cursor, snap to grid
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
			transform.position = hit.point;

		return gridManager.SnapToTile (gameObject, towerManager.zOffset);
		
	}

	void EndPurchase ()
	{
		purchaseInProgress = false;
		towerShoot.purchaseInProgress = false;
		towerManager.SetCancelButton (false);
		towerShoot.Reload ();
	}
}
