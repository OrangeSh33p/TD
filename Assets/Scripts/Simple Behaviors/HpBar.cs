using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
	[Header("HP Bar")]
	[SerializeField] float totalWidth;
	[SerializeField] float totalHeight;
	[SerializeField] float yPosition;

	//Reference to GameManager
	static GameManager gm;

	//State
	public float maxHP;

	//Intermediate variables
	float chunkWidth;
	float chunkAmount;
	int spaceAmount;

	void Start () {
		gm = GameManager.Instance;

		if (gm.chunkHP == 0) gm.chunkHP = 10; //Prevents infinite loops
		maxHP = transform.parent.GetComponent<Monster>().type.maxHp;

		chunkAmount = maxHP/gm.chunkHP;
		spaceAmount = Mathf.CeilToInt(chunkAmount)-1;
		chunkWidth = (totalWidth - spaceAmount*gm.spaceWidth)/chunkAmount;

		setUpHPBar();
		setHP (maxHP);
	}

	///Sets position and scale of a rect transform
	void setPosScale (RectTransform target, float posX, float posY, float posZ, float scaleX, float scaleY) {
		target.anchoredPosition3D = new Vector3 (posX, posY, posZ);
		target.sizeDelta = new Vector2 (scaleX, scaleY);
	}

	void setUpHPBar () {
		foreach (Transform t in transform)
			Destroy(t.gameObject);

		//Set up background
		setPosScale ( 
			Instantiate(gm.backGroundPrefab, transform).GetComponent<RectTransform>(), 
			gm.BGXOffset, yPosition + gm.BGYOffset, 0, 
			totalWidth+gm.BGLeftMargin, totalHeight+gm.BGTopMargin
		);

		//Set up every full bar
		for (int i=0;i<Mathf.FloorToInt(chunkAmount);i++) {
			Slider currentChunk = Instantiate(gm.chunkPrefab, transform);
			currentChunk.maxValue = gm.chunkHP;

			setPosScale ( 
				currentChunk.GetComponent<RectTransform>(), 
				chunkWidth/2 + i*(chunkWidth+gm.spaceWidth) - totalWidth/2, yPosition, 0, 
				chunkWidth, totalHeight
			);
		}

		//Set up the final incomplete bar (if needed)
		if (chunkAmount != Mathf.Floor(chunkAmount)) {
			float xfinal = spaceAmount*(chunkWidth+gm.spaceWidth) + (((float)maxHP%(float)gm.chunkHP) * totalWidth) / (maxHP*2);
			Slider finalChunk = Instantiate(gm.chunkPrefab, transform);
			float finalChunkHP = (maxHP % gm.chunkHP);
			finalChunk.maxValue = finalChunkHP;
			setPosScale ( 
				finalChunk.GetComponent<RectTransform>(), 
				xfinal - totalWidth/2, yPosition, 0, 
				finalChunkHP * totalWidth / maxHP, totalHeight
			);
		}
	}

	public void setHP (float hp) {
		Slider[] children = GetComponentsInChildren<Slider>();
		float hpLeft = hp;
		for (int i=0;i<children.Length;i++) {
			float currentChunkValue = Mathf.Min(gm.chunkHP, hpLeft);
			children[i].value = currentChunkValue;
			hpLeft -= currentChunkValue;
		}
	}
}
