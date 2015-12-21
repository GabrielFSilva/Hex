using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBackgroundManager : MonoBehaviour 
{
	public List<GameObject>		bgPrefabs;
	public GameObject			bgTilesContainer;

	public int					tileMapRows;
	public int					tileMapColumns;

	public BackgroundType		bgType;
	public enum BackgroundType
	{
		SQUARE_FLAT,
		SQUARE_POINTY,
		HEXAGON_FLAT,
		HEXAGON_POINTY,
		OCTAGON_FLAT
	}

	public void CreateBackground(BackgroundType p_bgType)
	{
		if (p_bgType == BackgroundType.SQUARE_FLAT)
		{
			for (int i = -3; i < tileMapRows + 3; i ++) 
			{
				for (int j = -4; j < tileMapColumns + 4; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[0]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					__tempGO.transform.localPosition = new Vector3 (j * 2f, i * -2f, 0f);
					
				}
			}
		}
		else if (p_bgType == BackgroundType.SQUARE_POINTY)
		{
			float __2sqrt = Mathf.Sqrt (2f);
			for (int i = -3; i < tileMapRows + 3; i ++) 
			{
				for (int j = -8; j < tileMapColumns + 16; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[0]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.GetChild(0).localRotation = Quaternion.Euler(Vector3.forward * 45f);
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					if ((j + 10) % 2 == 0)
						__tempGO.transform.localPosition = new Vector3 (j * __2sqrt, (i * -2f * __2sqrt), 0f);
					else
						__tempGO.transform.localPosition = new Vector3 ((j * __2sqrt) , (i * -2f * __2sqrt)- __2sqrt, 0f);
					
				}
			}
		}
		else if (p_bgType == BackgroundType.HEXAGON_FLAT)
		{
			for (int i = -3; i < tileMapRows + 3; i ++) 
			{
				for (int j = -8; j < tileMapColumns + 8; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[1]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.GetChild(0).transform.localRotation = Quaternion.identity;
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					
					if ((j + 10) % 2 == 1)
						__tempGO.transform.localPosition = new Vector3 (j * 1.55f, (i * -1.8f) + 0.9f, 0f);
					else
						__tempGO.transform.localPosition = new Vector3 ((j * 1.55f) , i * -1.8f, 0f);
				}
			}
		}
		else if (p_bgType == BackgroundType.HEXAGON_POINTY)
		{
			for (int i = -2; i < tileMapRows + 2; i ++) 
			{
				for (int j = -4; j < tileMapColumns + 4; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[1]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					
					if (i % 2 == 0)
						__tempGO.transform.localPosition = new Vector3 (j * 1.8f, i * -1.55f, 0f);
					else
						__tempGO.transform.localPosition = new Vector3 ((j * 1.8f) + 0.9f, i * -1.55f, 0f);
				}
			}
		}
		else if (p_bgType == BackgroundType.OCTAGON_FLAT)
		{
			for (int i = -3; i < tileMapRows + 3; i ++) 
			{
				for (int j = -3; j < tileMapColumns + 3; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[2]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.localScale = Vector3.one * 1.1f;
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					if ((i+10)% 2 == 0)
						__tempGO.transform.localPosition = new Vector3 (j * 3f, i * -1.5f, 0f);
					else
						__tempGO.transform.localPosition = new Vector3 (j * 3f + 1.5f, i * -1.5f, 0f);
					
				}
			}
			for (int i = -3; i < tileMapRows + 3; i ++) 
			{
				for (int j = -3; j < tileMapColumns + 3; j ++) 
				{
					GameObject __tempGO = (GameObject)GameObject.Instantiate(bgPrefabs[0]);
					__tempGO.transform.parent = bgTilesContainer.transform;
					__tempGO.transform.localScale = Vector3.one * 0.43f;
					__tempGO.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color (1f,1f,1f,0.12f);
					if ((i+10)% 2 == 0)
						__tempGO.transform.localPosition = new Vector3 (j * 3f + 1.5f, i * -1.5f, 0f);
					else
						__tempGO.transform.localPosition = new Vector3 (j * 3f + 3f, i * -1.5f, 0f);
					
				}
			}
		}
	}

}
