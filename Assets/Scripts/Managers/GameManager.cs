using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager> 
{
	[Header("Boring Variables")]
	[SerializeField] GameObject gameoverOverlay;
	[SerializeField] GameObject victoryOverlay;

	//State
	[HideInInspector] public bool victory;
	[HideInInspector] public bool defeat;

	void Update ()
	{
		if (victory)
		{
			StartCoroutine (Victory ());
			victory = false;
		}
		if (defeat)
		{
			StartCoroutine (GameOver ());
			defeat = false;
		}
	}

	//Why is all dark when restart ?
	//Called by LivesManager when last life is lost
	public IEnumerator GameOver ()
	{
		yield return new WaitForSeconds (1);
		gameoverOverlay.SetActive (true);
		yield return new WaitForSeconds (3);
		RestartScene ();
	}

	//Why no trigger ?
	//Called from each monster when it dies
	public IEnumerator Victory ()
	{
		yield return new WaitForSeconds (1);
		victoryOverlay.SetActive (true);
		yield return new WaitForSeconds (3);
		RestartScene ();
	}

	void RestartScene ()
	{
		Monster.monsterList.Clear ();
		TowerBuild.towerList.Clear ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
