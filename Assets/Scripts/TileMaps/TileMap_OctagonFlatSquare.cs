using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap_OctagonFlatSquare : TileMap 
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
		rows = Random.Range (3, 7);
		if (rows % 2 == 0)
			rows ++;
		columns = Random.Range (4, 7);
		int __index = 0;
		
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				if (i % 2 == 1 && j == columns -1)
				{
					tiles.Add(null);
					continue;
				}

				GameObject __tempGO = (GameObject)GameObject.Instantiate(tilePrefabs[0]);
				Tile __tempOct = __tempGO.GetComponent<Octagon>();
				__tempOct.positionOnGrid = new Vector2(j,i);
				__tempOct.indexOnList = __index;
				__tempOct.bgSprite.transform.localRotation = Quaternion.identity;
				tiles.Add(__tempOct);
				
				__tempGO.transform.parent = tilesContainer.transform;
				__tempGO.transform.localScale = Vector3.one * 1.1f;
				__tempGO.name = "Octagon (" + j.ToString() + "," + i.ToString() + ")";

				if ((i+10)% 2 == 0)
					__tempGO.transform.localPosition = new Vector3 (j * 3f, i * -1.5f, 0f);
				else
					__tempGO.transform.localPosition = new Vector3 (j * 3f + 1.5f, i * -1.5f, 0f);
				
				__tempOct.OnRotationChanged += delegate() 
				{
					if (CheckCompletion())
						TileMapCompleted();
				};
				__tempOct.SetCustomOppositeSide (new List<int>{4,2,6,3,0,0,2,1});
				__index ++;

			}
		}
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				if (i % 2 == 0 && j == columns -1)
				{
					extraTiles0.Add(null);
					continue;
				}
				GameObject __tempGO = (GameObject)GameObject.Instantiate(tilePrefabs[1]);
				Tile __tempSqr = __tempGO.GetComponent<Square>();
				__tempSqr.positionOnGrid = new Vector2(j,i);
				__tempSqr.indexOnList = __index;
				__tempSqr.bgSprite.transform.localRotation = Quaternion.identity;
				__tempSqr.transform.localScale = Vector3.one * 0.45f;
				extraTiles0.Add(__tempSqr);
				
				__tempGO.transform.parent = tilesContainer.transform;
				__tempGO.name = "Square (" + j.ToString() + "," + i.ToString() + ")";

				if ((i+10)% 2 == 0)
					__tempGO.transform.localPosition = new Vector3 (j * 3f + 1.5f, i * -1.5f, 0f);
				else
					__tempGO.transform.localPosition = new Vector3 (j * 3f, i * -1.5f, 0f);
				
				__tempSqr.OnRotationChanged += delegate() 
				{
					if (CheckCompletion())
						TileMapCompleted();
				};
				__tempSqr.SetCustomOppositeSide (new List<int>{5,7,1,3});
				__index ++;
			}
		}
		mainCamera.transform.position = new Vector3 ((tiles[1].transform.position.x + tiles[tiles.Count-2].transform.position.x)/2f , 
		                                             (tiles[1].transform.position.y + tiles[tiles.Count-2].transform.position.y)/2f, -12f);
	}

	public override void CreatePaths()
	{
		base.CreatePaths ();
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				if (i % 2 == 1 && j == columns -1)
					continue;

				Tile __oct = tiles[(i * columns) + j];
				if (__oct == null)
					continue;
				List<bool> __conections = new List<bool>();

				//TOP-RIGHT
				__conections.Add (__oct.positionOnGrid.x == columns - 1 || __oct.positionOnGrid.y == 0 ? false : GetRandomConection());
				//RIGHT
				__conections.Add (__oct.positionOnGrid.x == columns - 1 ? false : GetRandomConection());
				//BOTTOM-RIGHT
				__conections.Add (__oct.positionOnGrid.x == columns - 1 || __oct.positionOnGrid.y == rows - 1 ? false : GetRandomConection());
				//BOTTOM
				__conections.Add (__oct.positionOnGrid.y == rows - 1 ? false : GetRandomConection());
				//BOTTOM-LEFT
				__conections.Add ((__oct.positionOnGrid.x == 0 && __oct.positionOnGrid.y % 2 == 0) || __oct.positionOnGrid.y == rows - 1 ? false : GetRandomConection());
				//LEFT
				__conections.Add (__oct.positionOnGrid.x == 0 && __oct.positionOnGrid.y % 2 == 0 ? false : GetRandomConection());
				//TOP-LEFT
				__conections.Add ((__oct.positionOnGrid.x == 0 && __oct.positionOnGrid.y % 2 == 0) || __oct.positionOnGrid.y == 0 ? false : GetRandomConection());
				//TOP
				__conections.Add (__oct.positionOnGrid.y == 0 ? false : GetRandomConection());
				__oct.SetConnectionsList(__conections);
			}
		}
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				if (i % 2 == 0 && j == columns -1)
					continue;
				Tile __sqr = extraTiles0[(i * columns) + j];
				if (__sqr == null)
					continue;
				List<bool> __conections = new List<bool>();
				
				//RIGHT
				__conections.Add (__sqr.positionOnGrid.x == columns - 1 ? false : GetRandomConection());
				//BOTTOM
				__conections.Add (__sqr.positionOnGrid.y == rows - 1 ? false : GetRandomConection());
				//LEFT
				__conections.Add (__sqr.positionOnGrid.x == 0 && __sqr.positionOnGrid.y % 2 == 1 ? false : GetRandomConection());
				//TOP
				__conections.Add (__sqr.positionOnGrid.y == 0 ? false : GetRandomConection());
				__sqr.SetConnectionsList(__conections);
			}
		}
	}
	
	
	public override Tile GetTileByPositionOnGrid(Vector2 p_pos)
	{
		base.GetTileByPositionOnGrid (p_pos);
		foreach (Octagon oct in tiles)
		{
			if (oct == null)
				continue;
			if (oct.positionOnGrid == p_pos)
				return oct;
		}
		return null;
	}

	
	public override void ConectWithNeighbors(Tile p_tile, int p_tileSet)
	{
		base.ConectWithNeighbors(p_tile, p_tileSet);
		if (p_tile == null)
			return;
		List<Tile> __neighbors = new List<Tile> ();
		Tile __oct = p_tile;

		if (p_tileSet == 0) 
		{
			//TOP-RIGHT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, -1f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (1f, -1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [4])
				p_tile.connectionsList [0] = true;
			//RIGHT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 0f), 0);
			else
				__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (1f, 0f), 0);
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [2])
				p_tile.connectionsList [1] = true;
			//BOTTOM-RIGHT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 1f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (1f, 1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [6])
				p_tile.connectionsList [2] = true;
			//BOTTOM
			__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 1f),0);
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [3])
				p_tile.connectionsList [3] = true;
			//BOTTOM-LEFT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (-1f, 1f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [0])
				p_tile.connectionsList [4] = true;
			//LEFT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (-1f, 0f), 0);
			else
				__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 0f), 0);
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [0])
				p_tile.connectionsList [5] = true;
			//TOP-LEFT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (-1f, -1f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, -1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [2])
				p_tile.connectionsList [6] = true;
			//TOP
			__oct = GetExtraTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, -1f),0);
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [1])
				p_tile.connectionsList [7] = true;
		}
		else if (p_tileSet == 1) 
		{
			//RIGHT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (1f, 0f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 0f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [5])
				p_tile.connectionsList [0] = true;
			//BOTTOM
			__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [7])
				p_tile.connectionsList [1] = true;
			//LEFT
			if (p_tile.positionOnGrid.y % 2 == 0)
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, 0f));
			else
				__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (-1f, 0f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [1])
				p_tile.connectionsList [2] = true;
			//TOP
			__oct = GetTileByPositionOnGrid (p_tile.positionOnGrid + new Vector2 (0f, -1f));
			__neighbors.Add (__oct);
			if (__oct != null && __oct.connectionsList [3])
				p_tile.connectionsList [3] = true;
		}
		p_tile.SetNeighborsList (__neighbors);
	}
	public override bool GetRandomConection()
	{
		base.GetRandomConection ();
		return Random.Range (0, 100) < 20 ? true : false;
	}
}
