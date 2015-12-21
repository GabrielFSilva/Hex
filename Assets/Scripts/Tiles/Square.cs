using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Square : Tile
{
	public override void CreateConnectors()
	{
		base.CreateConnectors ();
		connectors = new List<SpriteRenderer> ();
		for (int i = 0; i < connectionsList.Count; i ++)
		{
			if (connectionsList[i])
			{
				GameObject __go = (GameObject)GameObject.Instantiate (connectorPrefab);
				__go.transform.parent = connectorContainer.transform;
				__go.transform.localPosition = Vector3.zero;
				__go.transform.localRotation = Quaternion.Euler((Vector3.back * (360f/rotationsCount) * i) + baseRotation);
				__go.transform.localScale = new Vector3(1f/transform.localScale.x, 1.08f,1f);
				connectors.Add(__go.GetComponent<SpriteRenderer>());
			}
		}
		if (connectors.Count == 0)
		{
			bgSprite.color = Color.gray;
			centerCircleSprite.gameObject.SetActive(false);
			SetLocked(true, false);
		}
		else
		{
			bgSprite.color = Color.white;
			centerCircleSprite.gameObject.SetActive(true);
			SetLocked(false, false);;
		}
	}
	public override void RecreateConnectors()
	{
		base.RecreateConnectors ();
		for (int i = 0; i < connectors.Count; i++)
			Destroy (connectors [i].gameObject);
		
		connectors.Clear ();
		CreateConnectors ();
		
	}

	public override void SetStandardOppositeSide()
	{
		base.SetStandardOppositeSide ();
		oppositeSides = new List<int> {2,3,0,1,};
	}
	
	public override void UpdateCompleted()
	{
		base.UpdateCompleted ();
		int __temp = 0;
		int __connectorCount = -1;
		completed = true;
		for (int i = 0; i < connectionsList.Count; i++)
		{
			if (connectionsList[i])
			{
				__connectorCount ++;
				__temp = i + rotantionIndex;
				if (__temp >= sidesCount)
					__temp -= sidesCount;
				if (__temp < 0)
					__temp += sidesCount;
				if (neighborsList[__temp] != null)
				{
					if(neighborsList[__temp].HasConectionOnSide(GetOppositeConnectionSide(__temp)))
						SetConnectorState(ConnectionState.CONNECTED, connectors[__connectorCount]);
					else if (neighborsList[__temp].connectors.Count == 0)
					{
						SetConnectorState(ConnectionState.TO_VOID, connectors[__connectorCount]);
						completed = false;
					}
					else
					{
						SetConnectorState(ConnectionState.UNCONNECTED, connectors[__connectorCount]);
						completed = false;
					}
				}
				else
				{
					SetConnectorState(ConnectionState.TO_VOID, connectors[__connectorCount]);
					completed = false;
				}
			}
		}
	}
	
}
