using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
	public float maxHP;

	[Header("HP Bar")]
	[SerializeField] float totalWidth;
	[SerializeField] float totalHeight;
	[SerializeField] float yPosition;

	[Header("Chunks")]
	[SerializeField] Slider chunkPrefab;
	[SerializeField] float chunkHP;
	[SerializeField] float spaceWidth;

	[Header("Background")]
	[SerializeField] Image backGroundPrefab;
	[SerializeField] float BGXOffset;
	[SerializeField] float BGYOffset;
	[SerializeField] float BGLeftMargin;
	[SerializeField] float BGTopMargin;

	float chunkWidth;
	float chunkAmount;
	int spaceAmount;

	void Start () {
		if (chunkHP == 0) chunkHP = 10; //Prevents infinite loops
		maxHP = transform.parent.GetComponent<Monster>().type.maxHp;

		chunkAmount = maxHP/chunkHP;
		spaceAmount = Mathf.CeilToInt(chunkAmount)-1;
		chunkWidth = (totalWidth - spaceAmount*spaceWidth)/chunkAmount;

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
			Instantiate(backGroundPrefab, transform).GetComponent<RectTransform>(), 
			BGXOffset, yPosition + BGYOffset, 0, 
			totalWidth+BGLeftMargin, totalHeight+BGTopMargin
		);

		//Set up every full bar
		for (int i=0;i<Mathf.FloorToInt(chunkAmount);i++) {
			Slider currentChunk = Instantiate(chunkPrefab, transform);
			currentChunk.maxValue = chunkHP;

			setPosScale ( 
				currentChunk.GetComponent<RectTransform>(), 
				chunkWidth/2 + i*(chunkWidth+spaceWidth) - totalWidth/2, yPosition, 0, 
				chunkWidth, totalHeight
			);
		}

		//Set up the final incomplete bar (if needed)
		if (chunkAmount != Mathf.Floor(chunkAmount)) {
			float xfinal = spaceAmount*(chunkWidth+spaceWidth) + (((float)maxHP%(float)chunkHP) * totalWidth) / (maxHP*2);
			Slider finalChunk = Instantiate(chunkPrefab, transform);
			float finalChunkHP = (maxHP % chunkHP);
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
			float currentChunkValue = Mathf.Min(chunkHP, hpLeft);
			children[i].value = currentChunkValue;
			hpLeft -= currentChunkValue;
		}
	}
}
