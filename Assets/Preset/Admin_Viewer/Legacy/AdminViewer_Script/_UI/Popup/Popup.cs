using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
	private void OnEnable()
	{
		Invoke("FadeOut", 1.2f);
	}

	private void FadeOut()
	{
		gameObject.SetActive(false);
	}
}
