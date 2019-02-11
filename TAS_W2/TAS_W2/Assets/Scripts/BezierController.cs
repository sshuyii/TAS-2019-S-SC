using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

[ExecuteInEditMode]
public class BezierController : MonoBehaviour
{
	[Header("Public References")]
	public GameObject Bezier;
	public BezierExample BezierExample;

	//private
	private Vector3 startPoint1;
	private Vector3 startTangent1;
	
	[Header("My List of Curves")]
	public List<BezierExample> curveList = new List<BezierExample>();
	
		
	// Use this for initializationz
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < curveList.Count; i++)
		{
			//record the values of the first bezier curve
			if (i == 0)
			{

				startPoint1 = curveList[i].startPoint;
				startTangent1 = curveList[i].startTangent;
				
				print(startPoint1);
				print(startTangent1);

			}
			
			//for the last bezier curve, smoothly connected with the first one
			if (i == curveList.Count - 1)
			{
				curveList[i].endPoint = startPoint1;
				curveList[i].endTangent.Set(2 * startPoint1.x - startTangent1.x, 2 * startPoint1.y - startTangent1.y, 2 * startPoint1.z - startTangent1.z);
			}
			//for bezier curves that are not the first one nor the last one
			else if(i > 0)
			{
				curveList[i].startPoint = curveList[i - 1].endPoint;
				curveList[i].startTangent.Set(2 * curveList[i - 1].endPoint.x - curveList[i - 1].endTangent.x,
					2 * curveList[i - 1].endPoint.y - curveList[i - 1].endTangent.y,
					2 * curveList[i - 1].endPoint.z - curveList[i - 1].endTangent.z);
			}
		}
	}
}
