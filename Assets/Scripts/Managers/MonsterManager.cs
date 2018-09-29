using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterManager {
	//Reference to GameManager
	static GameManager gm;
	public static List<monsterType> monsters;

	[System.Serializable] public struct monsterType {
		public GameObject prefab;
		public float speed;
		public int maxHp;
		public int reward;
	}

	public enum monsterName {STANDARD, FAST};

	public static void _Init () {
		gm = GameManager.Instance;
		monsters = gm.monsters;
	}
}
