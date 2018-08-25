using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoSingleton <TowerManager>
{
	public ListOfTowerTypes towers;

	[Header("Boring Variables")]

	[Header("Standard Tower")]
	[SerializeField] Button buildStandardTowerButton; 
	[SerializeField] Button cancelStandardTowerButton;
	[SerializeField] GameObject standardTowerPrefab;

	[Header("Fast Tower")]
	[SerializeField] Button buildFastTowerButton; 
	[SerializeField] Button cancelFastTowerButton;
	[SerializeField] GameObject fastTowerPrefab;

	//Storage
	GameObject towerBuildPreview;

	//Declarations
	[System.Serializable] public struct ListOfTowerTypes
	{
		public TowerType standard;
		public TowerType fast;
	}

	[System.Serializable] public struct TowerType
	{
		public int price;
		public float range;
		public float reloadingTime;
		public float damage;
		public float bulletSpeed;
		public GameObject bulletPrefab;
		public Vector3 bulletSpawnPoint;
	}

	public enum towerName {standard, fast};

	void Start ()
	{
		buildStandardTowerButton.onClick.AddListener (CreateStandardTower);
		cancelStandardTowerButton.onClick.AddListener (CancelStandardTower);

		buildFastTowerButton.onClick.AddListener (CreateFastTower);
		cancelFastTowerButton.onClick.AddListener (CancelFastTower);
	}

	void CreateStandardTower ()
	{
		SetCancelButton (true, towers.standard);
		//Create a preview tower, initiate its purchase sequence
		towerBuildPreview = Instantiate (standardTowerPrefab, transform.position, Quaternion.identity, transform);
		towerBuildPreview.GetComponent<TowerBuild> ().StartPurchase ();
	}

	void CancelStandardTower ()
	{
		SetCancelButton (false, towers.standard);
		Destroy (towerBuildPreview);
	}

	void CreateFastTower ()
	{
		SetCancelButton (true, towers.fast);
		//Create a preview tower, initiate its purchase sequence
		towerBuildPreview = Instantiate (fastTowerPrefab, transform.position, Quaternion.identity, transform);
		towerBuildPreview.GetComponent<TowerBuild> ().StartPurchase ();
	}

	void CancelFastTower ()
	{
		SetCancelButton (false, towers.fast);
		Destroy (towerBuildPreview);
	}

	public void SetCancelButton (bool mode, TowerType type)
	{
		if (Equals(towers.standard, type))
			buildStandardTowerButton.gameObject.SetActive (!mode);
		if (Equals(towers.fast, type))
			buildFastTowerButton.gameObject.SetActive (!mode);
	}
}
 