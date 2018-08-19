using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoSingleton<TimeManager>
{
	[Header("Balancing")]
	[SerializeField] float pauseSpeed;
	[SerializeField] float playSpeed;
	[SerializeField] float fastSpeed;
	[SerializeField] float superFastSpeed;

	[Header("Boring variables")]
	[SerializeField] Button PauseButton;
	[SerializeField] Button PlayButton;
	[SerializeField] Button FastButton;
	[SerializeField] Button SuperFastButton;
	[SerializeField] Image SuperFastImage;

	[HideInInspector] public float timeScale;

	void Start ()
	{
		timeScale = 1;
		PauseButton.onClick.AddListener (Pause);
		PlayButton.onClick.AddListener (Play);
		FastButton.onClick.AddListener (Fast);
		SuperFastButton.onClick.AddListener (SuperFast);

		Play ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space))
			timeScale = 1 - timeScale;
	}

	void Pause ()
	{
		timeScale = pauseSpeed;
		SetAllToWhite ();
		PauseButton.GetComponent<Image>().color = Color.cyan;
	}

	void Play ()
	{
		timeScale = playSpeed;
		SetAllToWhite ();
		PlayButton.GetComponent<Image>().color = Color.cyan;
	}

	void Fast ()
	{
		timeScale = fastSpeed;
		SetAllToWhite ();
		FastButton.GetComponent<Image>().color = Color.cyan;
	}

	void SuperFast ()
	{
		timeScale = superFastSpeed;
		SetAllToWhite ();
		SuperFastButton.GetComponent<Image>().color = Color.cyan;
		SuperFastImage.color = Color.cyan;
	}

	void SetAllToWhite ()
	{
		PauseButton.GetComponent<Image>().color = Color.white;
		PlayButton.GetComponent<Image>().color = Color.white;
		FastButton.GetComponent<Image>().color = Color.white;
		SuperFastButton.GetComponent<Image>().color = Color.white;
		SuperFastImage.color = Color.white;
	}
}
