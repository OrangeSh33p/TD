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
	[SerializeField] Button buildTowerButton; 
	[SerializeField] Button cancelBuildButton;
	[SerializeField] GameObject towerPrefab;

	//Storage
	List<Transform> towers = new List<Transform>();
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
		towerBuildPreview.GetComponent<Tower> ().StartPurchase ();
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

	public List<Transform> GetTowers () 
	{
		towers.Clear ();
		for (int i = 0; i < transform.childCount; i ++) 
			towers.Add (transform.GetChild (i));
		return towers;
	}
}
 