using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TimeManager {
	//Reference to GameManager
	static GameManager gm = GameManager.Instance;
	
	//References to variables
	static float pauseSpeed = gm.pauseSpeed;
	static float playSpeed = gm.playSpeed;
	static float fastSpeed = gm.fastSpeed;
	static float superFastSpeed = gm.superFastSpeed;
	static Button PauseButton = gm.PauseButton;
	static Button PlayButton = gm.PlayButton;
	static Button FastButton = gm.FastButton;
	static Button SuperFastButton = gm.SuperFastButton;
	static Image SuperFastImage = gm.SuperFastImage;

	//State
	public static float timeScale;

	public static void _Start () {
		timeScale = 1;
		PauseButton.onClick.AddListener (Pause);
		PlayButton.onClick.AddListener (Play);
		FastButton.onClick.AddListener (Fast);
		SuperFastButton.onClick.AddListener (SuperFast);

		Play ();
	}

	public static void _Update () {
		if (Input.GetKeyDown (KeyCode.Space))
			timeScale = 1 - timeScale;
	}

	static void Pause () {
		timeScale = pauseSpeed;
		SetAllToWhite ();
		PauseButton.GetComponent<Image>().color = Color.cyan;
	}

	static void Play () {
		timeScale = playSpeed;
		SetAllToWhite ();
		PlayButton.GetComponent<Image>().color = Color.cyan;
	}

	static void Fast () {
		timeScale = fastSpeed;
		SetAllToWhite ();
		FastButton.GetComponent<Image>().color = Color.cyan;
	}

	static void SuperFast () {
		timeScale = superFastSpeed;
		SetAllToWhite ();
		SuperFastButton.GetComponent<Image>().color = Color.cyan;
		SuperFastImage.color = Color.cyan;
	}

	static void SetAllToWhite () {
		PauseButton.GetComponent<Image>().color = Color.white;
		PlayButton.GetComponent<Image>().color = Color.white;
		FastButton.GetComponent<Image>().color = Color.white;
		SuperFastButton.GetComponent<Image>().color = Color.white;
		SuperFastImage.color = Color.white;
	}
}
