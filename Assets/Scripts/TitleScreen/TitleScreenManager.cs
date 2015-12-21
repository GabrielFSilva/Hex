using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour 
{
	public Image	fade;
	public bool		fading = false;
	// Use this for initialization
	void Start () 
	{
		fade.gameObject.SetActive (true);
		StartCoroutine (Fade (true));
	}
	IEnumerator Fade(bool p_fadeIn)
	{
		yield return new WaitForSeconds (0.15f);
		fade.gameObject.SetActive (fade);
		fading = true;
		Color __oldColor = fade.color;
		Color __newColor = new Color (0f, 0f, 0f, 0f);

		if (!p_fadeIn) 
			__newColor = Color.black;
		
		float __t = 0f;
		
		while (__t < 1f) 
		{
			__t += Time.deltaTime * 1.0f;
			fade.color = Color.Lerp (__oldColor, __newColor, __t);
			yield return null;
		}
		fade.gameObject.SetActive (!p_fadeIn);
		fading = false;
		if (!p_fadeIn)
			Application.LoadLevel("GameScene");
		yield break;
	}
	public void ShapeButtonClicked (int p_index)
	{
		if (fading)
			return;

		if (p_index == 0)
			GameSceneManager.gameMode = GameSceneManager.GameMode.SQUARE;
		else if (p_index == 1)
			GameSceneManager.gameMode = GameSceneManager.GameMode.HEXAGON;
		else if (p_index == 2)
			GameSceneManager.gameMode = GameSceneManager.GameMode.OCTAGON;
		else if (p_index == 3)
			GameSceneManager.gameMode = GameSceneManager.GameMode.ALL;
		StopCoroutine (Fade (true));
		StartCoroutine (Fade (false));
	}
}
