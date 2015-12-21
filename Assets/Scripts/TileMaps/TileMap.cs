using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMap : MonoBehaviour 
{
	public event Action		OnGameCompleted;
	public bool				tileMapCompleted = false;
	public List<Tile>		allTiles;
	public List<Tile>		tiles;

	public GameObject		tilesContainer;

	public List<GameObject>	tilePrefabs;
	
	public Camera		mainCamera;
	
	public int 				rows;
	public int				columns;

	public GameBackgroundManager.BackgroundType	bgType;

	//Hint
	public bool				hasHint = true;
	public float			hintCooldown = 3f;

	//COMPLEX TILE_MAP INFO
	public List<Tile>		extraTiles0;
	public List<Tile>		extraTiles1;
	public List<Tile>		extraTiles2;

	// Use this for initialization
	public virtual void SetUp () 
	{
	}
	
	public virtual void CreateTileMap()
	{
	}
	
	public virtual void CreatePaths()
	{
	}

	public void LockRandomTile()
	{
		if (GameSceneManager.tileMapCompleted)
			return;

		Tile __tempTile = allTiles [UnityEngine.Random.Range (0, allTiles.Count)];
		if (__tempTile == null || __tempTile.locked || __tempTile.connectionsList.Count == 0)
			LockRandomTile ();
		else
			__tempTile.SetLocked (true, true);
	}
	public void TileMapCompleted()
	{
		GameSceneManager.tileMapCompleted = true;
		LockAllTiles ();
		FadeAllTilesBG ();

		StartCoroutine (IncreaseConnectorSize ());
		if (OnGameCompleted != null)
			OnGameCompleted ();
	}
	public void FadeAllTilesBG()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.FadeBGSprite();
	}
	public void LockAllTiles()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.locked = true;
	}
	public void LoadAllTilesList()
	{
		foreach (Tile tile in tiles)
			allTiles.Add (tile);
		foreach (Tile tile in extraTiles0)
			allTiles.Add (tile);
		foreach (Tile tile in extraTiles1)
			allTiles.Add (tile);
		foreach (Tile tile in extraTiles2)
			allTiles.Add (tile);
	}
	public void CreateAllConectors()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.CreateConnectors ();
	}
	public void RereateAllConectors()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.RecreateConnectors ();
	}
	public void RandomRotateAllTiles()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.RandomRotation ();
	}
	public void UpdateAllTilesCompletion()
	{
		foreach (Tile tile in allTiles)
			if (tile != null)
				tile.UpdateCompleted ();
	}
	public void ConnectAllTilesWithNeighbors()
	{
		foreach (Tile tile in tiles)
			ConectWithNeighbors (tile,0);
		foreach (Tile tile in extraTiles0)
			ConectWithNeighbors (tile,1);
		foreach (Tile tile in extraTiles1)
			ConectWithNeighbors (tile,2);
		foreach (Tile tile in extraTiles2)
			ConectWithNeighbors (tile,3);
	}
	public virtual Tile GetTileByPositionOnGrid(Vector2 p_pos)
	{
		return null;
	}
	public virtual Tile GetExtraTileByPositionOnGrid(Vector2 p_pos, int p_extraTileSet)
	{
		if (p_extraTileSet == 0)
			foreach (Tile tile in extraTiles0)
			{
				if (tile == null)
					continue;
				if (tile.positionOnGrid == p_pos)
					return tile;
			}
		else if (p_extraTileSet == 1)
			foreach (Tile tile in extraTiles1)
			{
				if (tile == null)
					continue;
				if (tile.positionOnGrid == p_pos)
					return tile;
			}
		else if (p_extraTileSet == 2)
			foreach (Tile tile in extraTiles2)
			{
				if (tile == null)
					continue;
				if (tile.positionOnGrid == p_pos)
					return tile;
			}
		return null;
	}
	
	public virtual bool CheckCompletion()
	{
		foreach (Tile tile in allTiles)
		{
			if (tile == null)
				continue;
			if (!tile.completed)
				return false;
		}
		return true;
	}
	
	public virtual void ConectWithNeighbors(Tile p_tile, int p_tileSet)
	{
	}
	public virtual bool GetRandomConection()
	{
		return false;
	}
	public virtual void CalcCameraPosition()
	{
	}
	IEnumerator IncreaseConnectorSize()
	{
		yield return new WaitForSeconds (0.8f);
		float __t = 0f;
		
		while (__t <= 1f)
		{
			__t += Time.deltaTime * 0.5f;
			foreach(Tile tile in allTiles)
				if (tile != null)
					foreach(SpriteRenderer connector in tile.connectors)
				{
						connector.transform.localScale = Vector3.Lerp(new Vector3(1f/tile.transform.localScale.x,0f,1f),
						                                              new Vector3(1f/tile.transform.localScale.x,1.3f,1f), __t);
				}
			yield return null;
		}
		yield break;
	}
}
