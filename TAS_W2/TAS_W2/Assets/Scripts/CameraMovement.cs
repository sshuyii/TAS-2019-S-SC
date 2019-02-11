using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

	public BezierController bezierController;
	public GameObject Bezier;
	
	private float Timer;
	private List<float> curveLengthList = new List<float>();
	
	private int i = 0;	
	private float speed;
	private float smoothing = 5f;

	
	// Use this for initialization
	void Start()
	{
		
		transform.position = Bezier.transform.position;
		float length = CalculateLength(bezierController.curveList[1]);
		
		
		//calculate the length of each curve and add them to the same list
		for (int i = 0; i < bezierController.curveList.Count; i++)
		{
			float curveLength = CalculateLength(bezierController.curveList[i]);
			curveLengthList.Add(curveLength);
			print("length =" + curveLength);
		}
		
	}
	
	
	
	void Update()
	{
		Timer += Time.deltaTime * speed * 50f;
		_PutPointsOnCurve();
		
		
		print("Timer = " + Timer);
		
		//float length = CalculateLength(bezierController.curveList[1]);
		//print(length);
	}

	
	
	
	private void _PutPointsOnCurve()
	{
	
		if(i < bezierController.curveList.Count)
		{
			if (Timer < 1)
			{

				speed = 1 / curveLengthList[i];
				print("speed =" + speed);
				
				
				//look at the next spot on the bezier curve
				Vector3 relativePos = CalculateBezier(bezierController.curveList[i], Timer + Time.deltaTime * speed) - transform.position;
				Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
				transform.rotation = rotation;
				
				//where camera should be this frame
				transform.position = CalculateBezier(bezierController.curveList[i], Timer);
				//transform.position = Vector3.Lerp(relativePos, CalculateBezier(bezierController.curveList[i], Timer), smoothing * Time.deltaTime);

			}
			
			else
			{	 
				i++;
				Timer = 0;
			}      
		}
		else
		{
			i = 0;
		}
		
	}

	float CalculateLength(BezierExample curveData)
	{
		Vector3 positionThisFrame = CalculateBezier(curveData, 0.5f);
		Vector3 positionNextFrame = CalculateBezier(curveData, 0.51f);

		Vector3 distancePerFrame = positionNextFrame - positionThisFrame;

		float Length = distancePerFrame.magnitude / 0.01f;

		return Length;
	}
	
	Vector3 CalculateBezier(BezierExample curveData, float t)
	{
		Vector3 a = curveData.startPoint;
		Vector3 b = curveData.startTangent;
		Vector3 c = curveData.endTangent;
		Vector3 d = curveData.endPoint;

		Vector3 ab = Vector3.Lerp(a, b, t);
		Vector3 bc = Vector3.Lerp(b, c, t);
		Vector3 cd = Vector3.Lerp(c, d, t);

		Vector3 abc = Vector3.Lerp(ab, bc, t);
		Vector3 bcd = Vector3.Lerp(bc, cd, t);

		Vector3 final = Vector3.Lerp(abc, bcd, t);

		return final;
	}
}


