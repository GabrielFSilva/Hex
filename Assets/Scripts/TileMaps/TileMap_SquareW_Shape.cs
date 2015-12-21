using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap_SquareW_Shape  : TileMap 
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
		rows = Random.Range (2, 6);
		columns = Random.Range (rows + 2, rows + 8);
		if (columns % 2 == 0)
			columns++;

		int __index = 0;
		float __2sqrt = Mathf.Sqrt (2f);
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				GameObject __tempGO = (GameObject)GameObject.Instantiate(tilePrefabs[0]);
				Tile __tempSqr = __tempGO.GetComponent<Square>();
				__tempSqr.positionOnGrid = new Vector2(j,i);
				__tempSqr.indexOnList = __index;
				__tempSqr.transform.localRotation = Quaternion.Euler(Vector3.forward * 45f);
				tiles.Add(__tempSqr);
				
				__tempGO.transform.parent = tilesContainer.transform;
				__tempGO.name = "Square (" + j.ToString() + "," + i.ToString() + ")";

				if (j % 2 == 0)
					__tempGO.transform.localPosition = new Vector3 (j * __2sqrt, (i * -2f * __2sqrt), 0f);
				else
					__tempGO.transform.localPosition = new Vector3 ((j * __2sqrt) , (i * -2f * __2sqrt)- __2sqrt, 0f);

				__tempSqr.OnRotationChanged += delegate() 
				{
					if (CheckCompletion())
						TileMapCompleted();
				};
				__index ++;
			}
		}
		//mainCamera.transform.position = new Vector3 ((tiles[0].transform.position.x + tiles[tiles.Count-2].transform.position.x)/2f , 
		//                                             (tiles[0].transform.position.y + tiles[tiles.Count-2].transform.position.y)/2f, -12f);
	}

	public override void CreatePaths()
	{
		base.CreatePaths ();
		for (int i = 0; i < rows; i ++) 
		{
			for (int j = 0; j < columns; j ++) 
			{
				Tile __sqr = tiles[(i * columns) + j];
				List<bool> __conections = new List<bool>();
				
				//TOP-RIGHT
				__conections.Add ( __sqr.positionOnGrid.x == columns - 1 || (__sqr.positionOnGrid.y == 0 && __sqr.positionOnGrid.x % 2 == 0) ? false : GetRandomConection());
				//BOTTOM-RIGHT
				__conections.Add (__sqr.positionOnGrid.x == columns - 1  || (__sqr.positionOnGrid.y == rows - 1  && __sqr.positionOnGrid.x % 2 == 1) ? false : GetRandomConection());
				//BOTTOM-LEFT
				__conections.Add (__sqr.positionOnGrid.x == 0  || (__sqr.positionOnGrid.y == rows - 1  && __sqr.positionOnGrid.x % 2 == 1) ? false : GetRandomConection());
				//TOP-LEFT
				__conections.Add (__sqr.positionOnGrid.x == 0 || (__sqr.positionOnGrid.y == 0 && __sqr.positionOnGrid.x % 2 == 0)? false : GetRandomConection());
				
				__sqr.SetConnectionsList(__conections);
			}
		}
	}
	
	
	public override Tile GetTileByPositionOnGrid(Vector2 p_pos)
	{
		base.GetTileByPositionOnGrid (p_pos);
		foreach (Square sqr in tiles)
			if (sqr.positionOnGrid == p_pos)
				return sqr;
		return null;
	}
	
	public override void ConectWithNeighbors(Tile p_sqr, int p_tileMap)
	{
		base.ConectWithNeighbors(p_sqr, p_tileMap);
		List<Tile> __neighbors = new List<Tile> ();
		Tile __sqr = p_sqr;
		
		//TOP-RIGHT
		if (p_sqr.positionOnGrid.x % 2 == 0)
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (1f, -1f));
		else
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (1f, 0f));
		__neighbors.Add (__sqr);
		if (__sqr != null && __sqr.connectionsList [2])
			p_sqr.connectionsList [0] = true;

		//BOTTOM-RIGHT
		if (p_sqr.positionOnGrid.x % 2 == 0)
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (1f, 0f));
		else
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (1f, 1f));
		__neighbors.Add (__sqr);
		if (__sqr != null && __sqr.connectionsList [3])
			p_sqr.connectionsList [1] = true;
		//BOTTOM-LEFT
		if (p_sqr.positionOnGrid.x % 2 == 0)
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (-1f, 0f));
		else
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (-1f, 1f));
		__neighbors.Add (__sqr);
		if (__sqr != null && __sqr.connectionsList [0])
			p_sqr.connectionsList [2] = true;
		//TOP-LEFT
		if (p_sqr.positionOnGrid.x % 2 == 0)
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (-1f, -1f));
		else
			__sqr = GetTileByPositionOnGrid (p_sqr.positionOnGrid + new Vector2 (-1f, 0f));
		__neighbors.Add (__sqr);
		if (__sqr != null && __sqr.connectionsList [1])
			p_sqr.connectionsList [3] = true;
		
		p_sqr.SetNeighborsList (__neighbors);
	}
	public override bool GetRandomConection()
	{
		base.GetRandomConection ();
		return Random.Range (0, 100) < 30 ? true : false;
	}

	public override void CalcCameraPosition ()
	{
		Vector3 __tempVec3 = new Vector3 ((tiles[1].transform.position.x + tiles[tiles.Count-2].transform.position.x)/2f, 
		                                  ((tiles[1].transform.position.y + tiles[tiles.Count-2].transform.position.y)/2f) + 0.7f, -12f);
		mainCamera.transform.localPosition = __tempVec3;
		
		float __sizeRows = rows * 3.0f / mainCamera.aspect;
		float __sizeColumn = columns * 1.3f / mainCamera.aspect;

		mainCamera.orthographicSize = __sizeRows >= __sizeColumn ? __sizeRows : __sizeColumn;
	}
}
