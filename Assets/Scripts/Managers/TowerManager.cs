using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TowerManager {
	//Reference to GameManager
	static GameManager gm;
	public static List<TowerType> towers;

	//Visuals
	public static Material transparent;
	public static Material transparentRed;
	public static Material opaque;

	//Storage
	public static GameObject towerBuildPreview;

	//Tower types
	public enum towerName {STANDARD, FAST};
	[System.Serializable] public struct TowerType {
		public GameObject prefab;
		public int price;
		public float range;
		public float reloadingTime;
		public float damage;
		public float bulletSpeed;
		public GameObject bulletPrefab;
		public Vector3 bulletSpawnPoint;
		public Button buildButton;
		public Button cancelButton;
	}

	public static void _Init () {
		gm = GameManager.Instance;
		towers = gm.towers;
		transparent = gm.transparent;
		transparentRed = gm.transparentRed;
		opaque = gm.opaque;
	}

	public static void _Start () {	
		//Set each button to the appropriate tower
		for (int i=0; i<towers.Count; i++) {
			int j = i;
			towers[i].buildButton.onClick.AddListener(delegate {CreateTower(j); });
			towers[i].cancelButton.onClick.AddListener(delegate {CancelTower(j); });
		}
	}

	static void CreateTower (int towerNumber) {
		CancelAllTowers();
		SetCancelButton (true, towerNumber);
		Transform th = gm.towerHolder;
		towerBuildPreview = gm._Instantiate (towers[towerNumber].prefab, th.position, Quaternion.identity, th);
		towerBuildPreview.GetComponent<Building> ().StartPurchase ();
	}

	static void CancelAllTowers () {
		for (int i=0;i<towers.Count;i++)
			CancelTower(i);
	}

	static void CancelTower (int towerNumber) {
		SetCancelButton (false, towerNumber);
		gm._Destroy (towerBuildPreview);
	}

	public static void SetCancelButton (bool mode, int towerNumber) {
		towers[towerNumber].cancelButton.gameObject.SetActive (mode);
	}
}
 