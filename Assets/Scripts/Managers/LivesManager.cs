using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LivesManager : MonoSingleton<LivesManager> 
{
	[Header("Balancing")]
	[SerializeField] int maxLives;

	[Header("Boring Variables")]
	[SerializeField] Text livesText;

	//State
	int lives;

	void Start ()
	{
		lives = maxLives;
		PrintLives ();
	}

	public void LoseLife ()
	{
		lives--;
		if (lives == 0)
			StartCoroutine (GameManager.Instance.GameOver ());
		PrintLives ();
	}

	void PrintLives ()
	{
		string l = "";
		for (int i=0; i<(lives); i++)
			l += "❤";
		livesText.text = l;
	}

}
