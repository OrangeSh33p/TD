using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterManager : MonoSingleton <MonsterManager> {
	[Header ("Balancing")]
	public float speed;
	public int maxHp;
	public int reward;
}
