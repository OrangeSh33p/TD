using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GoldManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;

	//State
	static int gold;
	static float textTimeLeft;

	public static void _Start () {
		gold = gm.startGold;
		PrintGold ();
	}

	public static void _Update () {
		if (textTimeLeft > 0)
			textTimeLeft -= Time.deltaTime;
		else if (gm.insufficientGoldText.activeSelf)
			gm.insufficientGoldText.SetActive (false);
	}

	///return true if player has more gold than "amount"
	public static bool CanAfford (int amount) {
		return (gold - amount >= 0);
	}

	///returns true if the player has enough gold, displays UI message otherwise;
	public static void AddGold (int amount) {
		if (CanAfford (-amount)) {
			gold += amount;
			PrintGold ();
		} else
			DisplayInsufficientGoldText ();
	}

	static void PrintGold () {
		gm.goldText.text = "x " + gold;
	}

	public static void DisplayInsufficientGoldText () {
		gm.insufficientGoldText.SetActive (true);
		textTimeLeft = gm.textDisplayDuration;
	}
}
