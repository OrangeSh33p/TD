using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager> 
{
	[Header("Boring Variables")]
	[SerializeField] GameObject gameoverOverlay;
	[SerializeField] GameObject victoryOverlay;

	[HideInInspector] public float timeScale;

	void Start ()
	{
		timeScale = 1;
	}

	//Why is all dark when restart ?
	//Called by LivesManager when one life is lost
	public IEnumerator GameOver ()
	{
		yield return new WaitForSeconds (1);
		gameoverOverlay.SetActive (true);
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	//Why no trigger ?
	//Called from each monster when it dies
	public IEnumerator Victory ()
	{
		Debug.Log ("Victory sequence");
		yield return new WaitForSeconds (1);
		Debug.Log ("Waited one second");
		victoryOverlay.SetActive (true);
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
