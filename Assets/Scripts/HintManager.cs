using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class HintManager : MonoBehaviour 
{
	public event Action		OnHintClicked;
	public float			hintCooldown;
	public float			hintTimerCount;

	public Image			hintButton;
	public Image			hintButtonOverlay;

	public GameObject		fade;

	void Start()
	{
		hintButtonOverlay.fillAmount = 1f;
	}

	void Update () 
	{
		if (fade.gameObject.activeSelf)
			return;
		hintTimerCount += Time.deltaTime;
		hintButtonOverlay.fillAmount = 1f - (hintTimerCount / hintCooldown);
	}

	public void EnableHintButton (bool p_enable)
	{
		hintButton.gameObject.SetActive (p_enable);
	}
	public void HintButtonClicked()
	{
		if (GameSceneManager.tileMapCompleted)
			return;
		if (fade.gameObject.activeSelf)
			return;
		if (hintTimerCount < hintCooldown)
			return;

		hintTimerCount = 0f;
		if (OnHintClicked != null)
			OnHintClicked();
	}
}
