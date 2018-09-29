using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class LivesManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;

	//State
	static int lives;

	public static void _Init () {
		gm = GameManager.Instance;
		lives = gm.maxLives;
	}

	public static void _Start () {
		PrintLives ();
	}

	public static void LoseLife () {
		lives--;
		if (lives == 0)
			FlowManager.GameOver();
		PrintLives ();
	}

	static void PrintLives () {
		gm.livesText.text = "x " + lives;
	}
}
