using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TimeManager {
	//Reference to GameManager
	static GameManager gm;

	//State
	static float timeScale;
	public static float scaledDeltaTime {
		get {
			return Time.deltaTime * timeScale;
		}
	}

	public static void _Init() {
		gm = GameManager.Instance;
		Play ();
	}

	public static void _Start () {
		gm.pauseButton.onClick.AddListener (Pause);
		gm.playButton.onClick.AddListener (Play);
		gm.fastButton.onClick.AddListener (Fast);
		gm.superFastButton.onClick.AddListener (SuperFast);
	}

	static void Pause () {
		timeScale = gm.pauseSpeed;
		SetAllToWhite ();
		gm.pauseButton.GetComponent<Image>().color = Color.cyan;
	}

	static void Play () {
		timeScale = gm.playSpeed;
		SetAllToWhite ();
		gm.playButton.GetComponent<Image>().color = Color.cyan;
	}

	static void Fast () {
		timeScale = gm.fastSpeed;
		SetAllToWhite ();
		gm.fastButton.GetComponent<Image>().color = Color.cyan;
	}

	static void SuperFast () {
		timeScale = gm.superFastSpeed;
		SetAllToWhite ();
		gm.superFastButton.GetComponent<Image>().color = Color.cyan;
		gm.superFastImage.color = Color.cyan;
	}

	static void SetAllToWhite () {
		gm.pauseButton.GetComponent<Image>().color = Color.white;
		gm.playButton.GetComponent<Image>().color = Color.white;
		gm.fastButton.GetComponent<Image>().color = Color.white;
		gm.superFastButton.GetComponent<Image>().color = Color.white;
		gm.superFastImage.color = Color.white;
	}
}
