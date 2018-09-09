using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TowerManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;
	public static Transform th = gm.towerHolder;
	public static List<TowerType> towers = gm.towers;

	//Storage
	static GameObject towerBuildPreview;

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

	public enum towerName {standard, fast};

	public static void _Start () {
		//Set each button to the apporpriate tower
		for (int i=0; i<towers.Count; i++) {
			int j = i;
			towers[i].buildButton.onClick.AddListener(delegate {CreateTower(j); });
			towers[i].cancelButton.onClick.AddListener(delegate {CancelTower(j); });
		}
	}

	static void CreateTower (int towerNumber) {
		SetCancelButton (true, towerNumber);
		towerBuildPreview = gm._Instantiate (towers[towerNumber].prefab, th.position, Quaternion.identity, th);
		towerBuildPreview.GetComponent<TowerBuild> ().StartPurchase ();
	}

	static void CancelTower (int towerNumber) {
		SetCancelButton (false, towerNumber);
		gm._Destroy (towerBuildPreview);
	}

	public static void SetCancelButton (bool mode, int towerNumber) {
		towers[towerNumber].cancelButton.gameObject.SetActive (mode);
	}
}
 