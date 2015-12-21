using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap_HexPointyTopOddR : TileMap 
{

	public override void SetUp () 
	{
		base.SetUp ();
		CreateTileMap ();
		CreatePaths ();
	}
	
	public override void CreateTileMap()
	{
		base.CreateTileMap ();
		rows = Random.Range (3, 6);
		columns = Random.Range (rows, 8);
		int __index = 0;
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				GameObject __tempGO = (GameObject)GameObject.Instantiate(tilePrefabs[0]);
				Tile __tempHex = __tempGO.GetComponent<Hexagon>();
				__tempHex.positionOnGrid = new Vector2(j,i);
				__tempHex.indexOnList = __index;
				tiles.Add(__tempHex);
				__tempGO.transform.parent = tilesContainer.transform;
				__tempGO.name = "Hexagon (" + j.ToString() + "," + i.ToString() + ")";
				
				if (i % 2 == 0)
					__tempGO.transform.localPosition = new Vector3 (j * 1.8f, i * -1.55f, 0f);
				else
					__tempGO.transform.localPosition = new Vector3 ((j * 1.8f) + 0.9f, i * -1.55f, 0f);
				
				__index ++;
				
				__tempHex.OnRotationChanged += delegate() 
				{
					if (CheckCompletion())
						TileMapCompleted();
				};
			}
		}
		mainCamera.transform.position = new Vector3 ((tiles[0].transform.position.x + tiles[tiles.Count-1].transform.position.x)/2f , 
		                                             (tiles[0].transform.position.y + tiles[tiles.Count-1].transform.position.y)/2f, -12f);
	}

	public override void CreatePaths()
	{
		base.CreatePaths ();
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				Tile __hex = tiles[(i * columns) + j];
				List<bool> __conections = new List<bool>();
				
				//UP-RIGHT
				__conections.Add ((__hex.positionOnGrid.y == 0 || (__hex.positionOnGrid.x == columns - 1 && __hex.positionOnGrid.y % 2 == 1)) ? false : GetRandomConection());
				//RIGHT
				__conections.Add ((__hex.positionOnGrid.x == (columns - 1)) ? false : GetRandomConection());
				//BOTTOM-RIGHT
				__conections.Add ((__hex.positionOnGrid.y == (rows - 1) || (__hex.positionOnGrid.x == columns - 1 && __hex.positionOnGrid.y % 2 == 1)) ? false : GetRandomConection());
				//BOTTOM-LEFT
				__conections.Add ((__hex.positionOnGrid.y == (rows - 1) || (__hex.positionOnGrid.x == 0 && __hex.positionOnGrid.y % 2 == 0)) ? false : GetRandomConection());
				//LEFT
				__conections.Add ((__hex.positionOnGrid.x == 0) ? false : GetRandomConection());
				//UP-LEFT
				__conections.Add ((__hex.positionOnGrid.y == 0 || (__hex.positionOnGrid.x == 0 && __hex.positionOnGrid.y % 2 == 0)) ? false : GetRandomConection());
				
				__hex.SetConnectionsList(__conections);
			}
		}
	}

	
	public override Tile GetTileByPositionOnGrid(Vector2 p_pos)
	{
		base.GetTileByPositionOnGrid (p_pos);
		foreach (Hexagon hex in tiles)
			if (hex.positionOnGrid == p_pos)
				return hex;
		return null;
	}
	

	
	public override void ConectWithNeighbors(Tile p_hex, int p_tileMap)
	{
		base.ConectWithNeighbors(p_hex, p_tileMap);
		List<Tile> __neighbors = new List<Tile> ();
		Tile __hex = p_hex;
		//TOP-RIGHT
		if (p_hex.positionOnGrid.y % 2 == 0)
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (0f, -1f));
		else
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (1f, -1f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [3])
			p_hex.connectionsList [0] = true;
		//RIGHT
		__hex =  GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (1f, 0f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [4])
			p_hex.connectionsList [1] = true;
		//BOTTOM-RIGHT
		if (p_hex.positionOnGrid.y % 2 == 0)
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (0f, 1f));
		else
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (1f, 1f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [5])
			p_hex.connectionsList [2] = true;
		
		//BOTTOM-LEFT
		if (p_hex.positionOnGrid.y % 2 == 0)
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (-1f, 1f));
		else
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (0f, 1f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [0])
			p_hex.connectionsList [3] = true;
		
		//LEFT
		__hex =  GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (-1f, 0f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [1])
			p_hex.connectionsList [4] = true;
		
		//TOP-LEFT
		if (p_hex.positionOnGrid.y % 2 == 0)
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (-1f, -1f));
		else
			__hex = GetTileByPositionOnGrid (p_hex.positionOnGrid + new Vector2 (0f, -1f));
		__neighbors.Add (__hex);
		if (__hex != null && __hex.connectionsList [2])
			p_hex.connectionsList [5] = true;
		
		p_hex.SetNeighborsList (__neighbors);
	}
	public override bool GetRandomConection()
	{
		base.GetRandomConection ();
		return Random.Range (0, 100) < 20 ? true : false;
	}
	public override void CalcCameraPosition ()
	{
		float xOffset = columns * -0.15f;
		Vector3 __tempVec3 = new Vector3 (((tiles[0].transform.position.x + tiles[tiles.Count-1].transform.position.x)/2f) + xOffset, 
		                                  (tiles[0].transform.position.y + tiles[tiles.Count-1].transform.position.y)/2f, -12f);
		if (rows % 2 == 1)
			__tempVec3 = new Vector3 (((tiles[0].transform.position.x + tiles[tiles.Count-1 - columns].transform.position.x)/2f) + xOffset, 
			                          (tiles[0].transform.position.y + tiles[tiles.Count-1].transform.position.y)/2f, -12f);
		mainCamera.transform.localPosition = __tempVec3;
		
		float __sizeRows = rows * 1.9f / mainCamera.aspect;
		float __sizeColumn = columns * 1.4f / mainCamera.aspect;

		mainCamera.orthographicSize = __sizeRows >= __sizeColumn ? __sizeRows : __sizeColumn;
	}
}
