using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoSingleton <MonsterManager>
{
	//Diff between hideininspector public and serializable private ?
	//[HideInInspector]
	public List<GameObject> monsters;

	void Init ()
	{
		monsters = new List<GameObject>();
	}

	public List<GameObject> GetMonsters () 
	{
		monsters.Clear();
		for (int i = 0; i < transform.childCount; i ++) 
			monsters.Add (transform.GetChild (i).gameObject);
		return monsters;
	}
}
