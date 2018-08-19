using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoSingleton <TowerManager>
{
	[Header("Balancing")]
	public int price;
	public float range;
	public float reloadingTime;
	public float damage;
	public float bulletSpeed;

	[Header("Boring Variables")]
	public GameObject bulletPrefab;
	public Vector3 bulletSpawnPoint;
	[SerializeField] Button buildTowerButton; 
	[SerializeField] Button cancelBuildButton;
	[SerializeField] GameObject towerPrefab;

	//Storage
	GameObject towerBuildPreview;

	void Start ()
	{
		buildTowerButton.onClick.AddListener (CreateTower);
		cancelBuildButton.onClick.AddListener (CancelBuild);
	}

	void CreateTower ()
	{
		SetCancelButton (true);
		//Create a preview tower, initiate its purchase sequence
		towerBuildPreview = Instantiate (towerPrefab, transform.position, Quaternion.identity, transform);
		towerBuildPreview.GetComponent<TowerBuild> ().StartPurchase ();
	}

	void CancelBuild ()
	{
		SetCancelButton (false);
		Destroy (towerBuildPreview);
	}

	public void SetCancelButton (bool mode)
	{
		buildTowerButton.gameObject.SetActive (!mode);
	}
}
 