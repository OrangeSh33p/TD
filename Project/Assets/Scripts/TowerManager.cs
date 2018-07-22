using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoSingleton <TowerManager>
{
	//Diff between hideininspector public and serializable private ?
	//[HideInInspector]
	public List<GameObject> towers;

	void Start ()
	{
		towers = new List<GameObject>();
	}

	public List<GameObject> GetTowers () 
	{
		for (int i = 0; i < transform.childCount; i ++) 
			towers.Add (transform.GetChild (i).gameObject);
		return towers;
	}
}
