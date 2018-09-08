using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;
	public static List<monsterType> monsters = gm.monsters;

	[System.Serializable] public struct monsterType {
		public GameObject prefab;
		public float speed;
		public int maxHp;
		public int reward;
	}

	public enum monsterName {standard, fast};
}
