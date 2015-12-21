using UnityEngine;
using System.Collections;

public class ReloadSceneButton : MonoBehaviour 
{

	// Use this for initialization
	void OnMouseDown()
	{
		if (name == "TitleScreenButton")
			Application.LoadLevel ("TitleScreen");
		else
			Application.LoadLevel (Application.loadedLevelName);
	}
}
