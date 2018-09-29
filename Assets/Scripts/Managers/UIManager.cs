using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager {
	//Reference to GameManager
	static GameManager gm;

	public static void _Init () {
		gm = GameManager.Instance;
	}
	
	public static void DisplayText (Text target) {
		gm._StartCoroutine(_DisplayText(target));
	}

	static IEnumerator _DisplayText (Text target) {
		GameObject go = target.gameObject;
		go.SetActive(true);
		yield return new WaitForSeconds(gm.textDisplayDuration);
		go.SetActive(false);
	}
}
