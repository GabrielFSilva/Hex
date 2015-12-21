using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameSceneManager : MonoBehaviour 
{
	public GameBackgroundManager	backgroundManager;
	public HintManager				hintManager;

	public static bool			tileMapCompleted = false;
	public List<TileMap>		tileMapsList;
	public TileMap				tileMap;
	
	public GameObject			tilesContainer;
	public GameObject			bgTilesContainer;

	public Camera			mainCamera;

	public int 					rows;
	public int					columns;

	[SerializeField]
	public static GameMode 		gameMode = GameMode.FIXED;
	public int					fixedNumber;

	public Image				fade;
	public bool					fading = false;

	public enum GameMode
	{
		SQUARE,
		HEXAGON,
		OCTAGON,
		ALL,
		FIXED
	}
	void Start () 
	{
		tileMapCompleted = false;
		fade.gameObject.SetActive (true);
		if (gameMode == GameMode.SQUARE)
			tileMap = tileMapsList [Random.Range (0, 7)];
		else if (gameMode == GameMode.HEXAGON)
			tileMap = tileMapsList [Random.Range (7, 11)];
		else if (gameMode == GameMode.OCTAGON)
			tileMap = tileMapsList [11];
		else if (gameMode == GameMode.ALL)
			tileMap = tileMapsList [Random.Range (0, 12)];
		else if (gameMode == GameMode.FIXED)
			tileMap = tileMapsList [fixedNumber];

		tileMap.tilesContainer = tilesContainer;
		tileMap.mainCamera = mainCamera;

		hintManager.EnableHintButton (tileMap.hasHint);
		hintManager.hintCooldown = tileMap.hintCooldown;
		hintManager.fade = fade.gameObject;
		hintManager.OnHintClicked += delegate() 
		{
			tileMap.LockRandomTile();
		};

		tileMap.SetUp ();
		tileMap.LoadAllTilesList ();
		tileMap.CreateAllConectors ();
		tileMap.CalcCameraPosition ();

		backgroundManager.tileMapRows = tileMap.rows;
		backgroundManager.tileMapColumns = tileMap.columns;
		backgroundManager.CreateBackground (tileMap.bgType);

		/*
		 * tileMap.ConnectAllTilesWithNeighbors();
		tileMap.RereateAllConectors();
		tileMap.UpdateAllTilesCompletion();
		tileMap.RandomRotateAllTiles();
		tileMap.UpdateAllTilesCompletion();
		 */
		tileMap.OnGameCompleted += delegate() 
		{
			StartCoroutine (Fade (false));
		};
		Debug.Log (gameMode + " / " + tileMap.name);
		StartCoroutine (Fade (true));

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			tileMap.ConnectAllTilesWithNeighbors();
			tileMap.RereateAllConectors();;
			tileMap.UpdateAllTilesCompletion();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{	
			tileMap.RandomRotateAllTiles();
			tileMap.UpdateAllTilesCompletion();
		}
	}

	public void UIButtonClicked (int p_index)
	{
		if (p_index == 1)
			Application.LoadLevel ("TitleScreen");
		else
			Application.LoadLevel (Application.loadedLevelName);
	}
	IEnumerator Fade(bool p_fadeIn)
	{
		if (p_fadeIn)
			yield return new WaitForSeconds (0.15f);
		else
			yield return new WaitForSeconds (5.0f);
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


}
