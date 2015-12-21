using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Tile : MonoBehaviour 
{
	public event Action				OnRotationChanged;
	public SpriteRenderer			bgSprite;
	public SpriteRenderer			centerCircleSprite;
	
	public List<Tile>				neighborsList;
	public List<int>				oppositeSides;
	public List<SpriteRenderer> 	connectors;
	public GameObject				connectorContainer;
	public GameObject				connectorPrefab;
	
	public TileType			tileType;
	public int				indexOnList;
	public Vector2			positionOnGrid;
	
	public List<bool>		connectionsList;

	public Vector3			baseRotation;
	public int				rotantionIndex = 0;

	public int				sidesCount;
	public float			rotationsCount;
	
	public bool				rotating = false;
	public bool				locked = false;
	public bool				completed = false;
	public bool				ignoreClick = false;

	public enum TileType
	{
		SQUARE,
		HEXAGON,
		OCTAGON
	}
	public enum ConnectionState
	{
		TO_VOID,
		UNCONNECTED,
		CONNECTED
	}
	void Awake () 
	{
		SetStandardOppositeSide ();
	}
	void OnMouseDown()
	{
		if (ignoreClick)
			return;
		if (locked)
			return;
		if (!rotating)
			StartCoroutine (RotateConnectors(false));
	}
	public void SetLocked (bool p_locked, bool p_setToOriginalRotation)
	{
		locked = p_locked;
		if (p_locked)
			bgSprite.color = Color.gray;
		else
			bgSprite.color = Color.white;

		if (p_setToOriginalRotation)
		{
			StopCoroutine(RotateConnectors(false));
			StartCoroutine (RotateConnectors(true));
		}
	}
	//public void 
	IEnumerator RotateConnectors(bool p_setToOritinalRotation)
	{
		rotating = true;
		Quaternion __oldQuat = connectorContainer.transform.localRotation;
		Quaternion __newQuat = Quaternion.Euler(__oldQuat.eulerAngles + (Vector3.back * (360f/rotationsCount)));

		float __t = 0f;
		
		//Increase RotationIndex
		rotantionIndex ++;
		if (rotantionIndex < 0)
			rotantionIndex += sidesCount;
		if (rotantionIndex == sidesCount)
			rotantionIndex -= sidesCount;
		
		if (p_setToOritinalRotation) {
			__newQuat = Quaternion.Euler (Vector3.zero);
			rotantionIndex = 0;
		}
		
		while (__t < 1f)
		{
			__t += Time.deltaTime * 8f;
			connectorContainer.transform.localRotation = Quaternion.Lerp(__oldQuat, __newQuat, __t);
			yield return null;
		}

		UpdateCompleted ();
		UpdateNeighborsCompleted ();
		
		if (OnRotationChanged != null)
			OnRotationChanged ();
		connectorContainer.transform.localRotation = __newQuat;
		rotating = false;
		yield break;
	}
	public void FadeBGSprite()
	{
		StartCoroutine (FadeBG ());
	}

	IEnumerator FadeBG()
	{
		yield return new WaitForSeconds (0.8f);
		Color __oldColor = bgSprite.color;
		Color __newColor = new Color (1f, 1f, 1f, 0f);
		float __t = 0f;
		
		while (__t < 1f)
		{
			__t += Time.deltaTime * 0.5f;
			bgSprite.color = Color.Lerp(__oldColor, __newColor, __t);
			yield return null;
		}
		bgSprite.color = __newColor;
		yield break;
	}
	public virtual void CreateConnectors()
	{
	}
	public virtual void RecreateConnectors()
	{
	}
	public void RandomRotation()
	{
		if (locked)
			return;
		int __random = UnityEngine.Random.Range (0, (int)rotationsCount);
		rotantionIndex = __random;
		connectorContainer.transform.localRotation = Quaternion.Euler (Vector3.back * (360f/rotationsCount) * __random);
	}
	public void UpdateNeighborsCompleted()
	{
		foreach (Tile neighHex in neighborsList)
			if (neighHex != null)
				neighHex.UpdateCompleted ();
	}
	public virtual void UpdateCompleted()
	{

	}
	
	public bool GetCompleted()
	{
		return completed;
	}
	
	public virtual int GetOppositeConnectionSide(int p_side)
	{
		return oppositeSides [p_side];
	}
	public virtual void SetStandardOppositeSide()
	{
	}
	public void SetCustomOppositeSide(List<int> p_list)
	{
		oppositeSides = new List<int> ();
		oppositeSides = p_list;
	}
	public void SetConnectorState(ConnectionState p_state, SpriteRenderer p_connector)
	{
		if (p_state == ConnectionState.TO_VOID)
			p_connector.color = Color.red;
		else if (p_state == ConnectionState.UNCONNECTED)
			p_connector.color = Color.white;
		else if (p_state == ConnectionState.CONNECTED)
			p_connector.color = Color.cyan;
	}
	public bool HasConectionOnSide(int p_side)
	{
		int __temp = p_side - rotantionIndex;
		if (__temp >= sidesCount)
			__temp -= sidesCount;
		if (__temp < 0)
			__temp += sidesCount;
		return connectionsList[__temp];
	}
	
	public void SetConnectionsList(List<bool> p_connectionsList)
	{
		connectionsList = new List<bool> ();
		connectionsList = p_connectionsList;
	}
	public void SetNeighborsList(List<Tile> p_neighborsList)
	{
		neighborsList = new List<Tile> ();
		neighborsList = p_neighborsList;
	}
}
