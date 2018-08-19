using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float TweenFunc(float t);

public static class Tweens 
{
	public static float LinearTween(float t)
	{
		return t;
	}

	public static float QuadTween(float t)
	{
		return t*t;
	}

	public static void Linear(float start, float end, float time, Action<float> callback) 
	{
		GameObject obj = new GameObject();
		obj.AddComponent<TweenBehaviour>().StartCoroutine(DoTween(start, end, time, callback, LinearTween, ()=>{
			GameObject.Destroy(obj);
		}));
	}

	public static void Quad(float start, float end, float time, Action<float> callback) 
	{
		GameObject obj = new GameObject();
		obj.AddComponent<TweenBehaviour>().StartCoroutine(DoTween(start, end, time, callback, QuadTween, ()=>{
			GameObject.Destroy(obj);
		}));
	}

	public static void Free(float start, float end, float time, Action<float> callback, TweenFunc func) 
	{
		GameObject obj = new GameObject();
		obj.AddComponent<TweenBehaviour>().StartCoroutine(DoTween(start, end, time, callback, func, ()=>{
			GameObject.Destroy(obj);
		}));
	}

	static IEnumerator DoTween(float start, float end, float time, Action<float> callback, TweenFunc func, Action after)
	{
		float t = 0;
		do
		{
			callback (Mathf.Lerp (start, end, func (t / time)));
			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		} while (t < time);
		callback (Mathf.Lerp (start, end, 1));
	}

}

public class TweenBehaviour : MonoBehaviour 
{
}
