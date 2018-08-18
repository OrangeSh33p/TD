using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoSingleton<GoldManager> 
{
	[Header("Balancing")]
	[SerializeField] int startGold;

	[Header("Boring Variables")]
	[SerializeField] Text goldText;
	[SerializeField] GameObject insufficientGoldText;

	//State
	int gold;

	void Start ()
	{
		gold = startGold;
		PrintGold ();
	}

	/// returns true if the player has enough gold
	public bool AddGold (int amount)
	{
		if (gold + amount < 0)
		{
			StartCoroutine (DisplayInsufficientGoldText ());
			return false;
		}
		else
		{
			gold += amount;
			PrintGold ();
			return true;
		}
	}

	void PrintGold ()
	{
		goldText.text = "x " + gold;
	}

	public IEnumerator DisplayInsufficientGoldText ()
	{
		insufficientGoldText.SetActive (true);
		yield return new WaitForSeconds (1);
		insufficientGoldText.SetActive (false);
	}
}
