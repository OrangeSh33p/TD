using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FlowManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;

	//Game State
	public enum gameState {PLAYING, VICTORY, DEFEAT};
	public static gameState state = gameState.PLAYING;

	//State
	static float timeToNextScreen = 0;
	static float timeToSceneChange = 0;
	static bool waiting = false;

	public static void _Update () {
		//Count time left before showing endgame screen
		if (timeToNextScreen > 0)
			timeToNextScreen -= Time.deltaTime * TimeManager.timeScale;
		if (waiting && timeToNextScreen <= 0) {
			timeToSceneChange = 3;
			if (state == gameState.VICTORY)
				gm.victoryOverlay.SetActive (true);
			if (state == gameState.DEFEAT)
				gm.gameoverOverlay.SetActive (true);
			waiting = false;
		}

		//Count time left before reloading scene
		if (timeToSceneChange > 0)
			timeToSceneChange -= Time.deltaTime * TimeManager.timeScale;
		if (timeToSceneChange < 0)
			RestartScene ();
	}

	//Called by LivesManager when last life is lost
	public static void GameOver () {
		state = gameState.DEFEAT;
		timeToNextScreen = 1;
		waiting = true;
	}

	//Called from each monster when it dies
	public static void Victory () {
		state = gameState.VICTORY;
		timeToNextScreen = 1;
		waiting = true;
	}

	static void RestartScene () {
		Monster.monsterList.Clear ();
		Building.towerList.Clear ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
