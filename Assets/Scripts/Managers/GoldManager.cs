using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldManager : MonoSingleton<GoldManager> {
	[Header("Balancing")]
	[SerializeField] int startGold;

	[Header("Boring Variables")]
	[SerializeField] Text goldText;
	[SerializeField] GameObject insufficientGoldText;

	//State
	int gold;

	void Start () {
		gold = startGold;
		PrintGold ();
	}

	///return true if player has more than "amount"
	public bool CanAfford (int amount)
	{
		return (gold - amount >= 0);
	}

	///returns true if the player has enough gold, displays error message otherwise;
	public void AddGold (int amount)
	{
		if (CanAfford(-amount)) {
			gold += amount;
			PrintGold ();
		}
		else
			StartCoroutine (DisplayInsufficientGoldText ());
	}

	void PrintGold () {
		goldText.text = "x " + gold;
	}

	public IEnumerator DisplayInsufficientGoldText () {
		insufficientGoldText.SetActive (true);
		yield return new WaitForSeconds (1);
		insufficientGoldText.SetActive (false);
	}
}
