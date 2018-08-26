using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuild : MonoBehaviour  {
	
	[Header("Boring Variables")]
	[SerializeField] List<GameObject> visuals;
	[SerializeField] Material transparent;
	[SerializeField] Material transparentRed;
	[SerializeField] Material opaque;

	//State
	bool purchaseInProgress = false;
	Material currentMaterial;

	//Storage
	Ray ray;
	RaycastHit hit;

	//References
	GoldManager goldManager;
	TowerManager towerManager = TowerManager.Instance;
	GridManager gridManager = GridManager.Instance;
	TowerShoot _towerShoot;

	//towerShoot can return a value during compilation, even if the gameobject has not been instanciated
	TowerShoot towerShoot  {
		get  {
			if (_towerShoot==null)
				_towerShoot = GetComponent<TowerShoot> ();
			return _towerShoot;
		}
	}

	static List<Transform> _towerList;
	public static List<Transform> towerList  {
		get  {
			if (_towerList==null)
				_towerList = new List<Transform> ();
			return _towerList;
		}
	}

	void Start () {
		towerManager = TowerManager.Instance;
		goldManager = GoldManager.Instance;
		gridManager = GridManager.Instance;

		towerList.Add (transform);
		transform.parent = towerManager.transform;

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
		towerShoot.purchaseInProgress = true;
		towerManager.SetCancelButton (true, towerShoot.typeNumber);
		SnapUnderCursor ();
	}

	///
	void ContinuePurchase () {
		Vector2Int gridPos = SnapUnderCursor ();

		if (gridManager.TileIsFree(gridPos)) {
			if (Input.GetMouseButtonUp (0)) {
				if (goldManager.CanAfford (towerShoot.type.price))
					EndPurchase ();
				else
					StartCoroutine (goldManager.DisplayInsufficientGoldText ());
			}
			else {
				if (goldManager.CanAfford (towerShoot.type.price))
					SetMaterial (transparent);
				else
					SetMaterial (transparentRed);
			}
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
		goldManager.AddGold (-towerShoot.type.price);
		purchaseInProgress = false;
		towerShoot.purchaseInProgress = false;
		towerManager.SetCancelButton (false, towerShoot.typeNumber);
		towerShoot.Reload ();
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
