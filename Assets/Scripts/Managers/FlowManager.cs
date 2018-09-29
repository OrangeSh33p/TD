using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class FlowManager {
	//Reference to GameManager
	static GameManager gm;

	//Game State
	public enum gameState {PLAYING, VICTORY, DEFEAT};
	public static gameState state;

	//State
	static float timeToNextScreen ;
	static float timeToSceneChange ;
	static bool waiting;

	public static void _Init() {
		gm = GameManager.Instance;
		state = gameState.PLAYING;
		timeToNextScreen = 0;
		timeToSceneChange = 0;
		waiting = false;
	}

	public static void _Update () {
		//Count time left before showing endgame screen
		if (timeToNextScreen > 0)
			timeToNextScreen -= TimeManager.scaledDeltaTime;
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
			timeToSceneChange -= TimeManager.scaledDeltaTime;
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
