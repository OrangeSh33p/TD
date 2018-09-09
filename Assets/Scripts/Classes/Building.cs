using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour  {
	[SerializeField] List<GameObject> visuals;

	//Visuals
	static Material transparent;
	static Material transparentRed;
	static Material opaque;

	//State
	bool purchaseInProgress = false;
	Material currentMaterial;

	//Storage
	Ray ray;
	RaycastHit hit;

	//References
	GridManager gridManager = GridManager.Instance;

	//tower : the tower script of the gameObject
	Tower _tower;
	Tower tower  {
		get  {
			if (_tower==null)
				_tower = GetComponent<Tower> ();
			return _tower;
		}
	}

	//TowerList : A list of all towers
	static List<Transform> _towerList;
	public static List<Transform> towerList  {
		get  {
			if (_towerList==null)
				_towerList = new List<Transform> ();
			return _towerList;
		}
	}

	void Start () {
		transparent = TowerManager.transparent;
		transparentRed = TowerManager.transparentRed;
		opaque = TowerManager.opaque;

		gridManager = GridManager.Instance;

		towerList.Add (transform);
		transform.parent = TowerManager.th;

		currentMaterial = opaque;
	}

	void Update () {
		if (purchaseInProgress)
			ContinuePurchase();
	}

	void OnDestroy () {
		towerList.Remove (transform);
	}
		
	public void StartPurchase () {
		purchaseInProgress = true;
		tower.purchaseInProgress = true;
		TowerManager.SetCancelButton (true, tower.typeNumber);
		SnapUnderCursor ();
	}

	void ContinuePurchase () {
		Vector2Int gridPos = SnapUnderCursor ();

		if (gridManager.TileIsFree(gridPos)) {
			if (Input.GetMouseButtonUp (0)) {
				if (GoldManager.CanAfford (tower.type.price))
					EndPurchase ();
				else
					GoldManager.DisplayInsufficientGoldText ();
			}
			else
				SetMaterial ((GoldManager.CanAfford (tower.type.price)) ? transparent : transparentRed);
		}
		else {
			SetMaterial(transparentRed);
			if (Input.GetMouseButtonUp (0))
				gridManager.TryBuildOnTile (gridPos);
		}
	}

	void EndPurchase () {
		SetMaterial(opaque);
		gridManager.SetAdjacentTiles (SnapUnderCursor(), GridManager.Tile.tower);
		GoldManager.AddGold (-tower.type.price);
		purchaseInProgress = false;
		tower.purchaseInProgress = false;
		TowerManager.SetCancelButton (false, tower.typeNumber);
		tower.Reload ();
	}

	///Snaps the tower on the closest tile under player's mouse position
	Vector2Int SnapUnderCursor () {
		//if mouse is above sth on layer 8 (the floor), move self to under the cursor, snap to grid
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		int layerMask = 1 << 8;
		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask))
			transform.position = hit.point;

		return gridManager.SnapToTile (gameObject);
		
	}

	void SetMaterial (Material material) {
		if (currentMaterial != material) {
			currentMaterial = material;
			foreach (GameObject go in visuals)
				go.GetComponent<MeshRenderer>().material = material;
		}
	}
}
