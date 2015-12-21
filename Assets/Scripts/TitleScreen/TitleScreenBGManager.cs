using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleScreenBGManager : MonoBehaviour 
{
	public List<GameObject>		bgGameObjects;
	public GameObject			bgContainer;
	public List<GameObject> 	bgPrefabsList;
	public List<Vector3>		offSetList;

	public Vector3	translateVector;
	public int		selectedPrefab;

	// Use this for initialization
	void Start () 
	{
		return;
		selectedPrefab = Random.Range (0, bgPrefabsList.Count);
		//selectedPrefab = 3;
		int __random = Random.Range (0, 4);
		if (__random == 0)
			translateVector = new Vector3 (1f, -1f, 0f);
		else if (__random == 1)
			translateVector = new Vector3 (-1f, -1f, 0f);
		else if (__random == 2)
			translateVector = new Vector3 (-1f, 1f, 0f);
		else
			translateVector = new Vector3 (1f, 1f, 0f);

		if (offSetList [selectedPrefab].x != offSetList [selectedPrefab].y)
			translateVector.x = 0f;

		bgGameObjects = new List<GameObject> ();
		for (int i = -8; i < 8; i ++)
		{

			GameObject __go = (GameObject)GameObject.Instantiate(bgPrefabsList[selectedPrefab], 
			                                                     new Vector3(offSetList[selectedPrefab].x * translateVector.y* i * translateVector.x, i * offSetList[selectedPrefab].y, 0f),
			                                                     Quaternion.identity);
			if (translateVector.x == 0)
				__go.transform.localPosition = new Vector3(0f,__go.transform.localPosition.y);
			__go.transform.parent = bgContainer.transform;
			bgGameObjects.Add(__go);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		return;
		if (Time.timeSinceLevelLoad < 0.25f)
			return;

		for(int i = 0; i < bgGameObjects.Count; i++)
		{
			bgGameObjects[i].transform.Translate (translateVector  * Time.deltaTime * 0.8f);
		}
		for(int i = 0; i < bgGameObjects.Count; i++)
		{
			if (translateVector.y > 0f && bgGameObjects[i].transform.localPosition.y > 7*offSetList[selectedPrefab].y)
			{
				if (translateVector.x != 0f)
					bgGameObjects[i].transform.localPosition =  GetLowerOrHigher(true) - new Vector3(offSetList[selectedPrefab].x * translateVector.x,1f* offSetList[selectedPrefab].y);
				else
					bgGameObjects[i].transform.localPosition = new Vector3(0f, GetLowerOrHigher(true).y - offSetList[selectedPrefab].y);
			}
			else if (translateVector.y < 0f && bgGameObjects[i].transform.localPosition.y < -7* offSetList[selectedPrefab].y)
			{
				if (translateVector.x != 0f)
					bgGameObjects[i].transform.localPosition = GetLowerOrHigher(false) + new Vector3(offSetList[selectedPrefab].x * -1f* translateVector.x,1f* offSetList[selectedPrefab].y);
				else
					bgGameObjects[i].transform.localPosition = new Vector3(0f, GetLowerOrHigher(false).y + offSetList[selectedPrefab].y);
			}
		}
	}
	private Vector3 GetLowerOrHigher(bool p_lower)
	{
		Vector3 __vec3 = Vector3.zero;
		for (int i = 0; i < bgGameObjects.Count; i++) 
		{
			if (p_lower && bgGameObjects[i].transform.localPosition.y < __vec3.y)
				__vec3 = bgGameObjects[i].transform.localPosition;
			else if (!p_lower && bgGameObjects[i].transform.localPosition.y > __vec3.y)
				__vec3 = bgGameObjects[i].transform.localPosition;
		}
		return __vec3;
	}
}
