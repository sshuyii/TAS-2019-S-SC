using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BezierController))]
public class ButtonEditor : Editor
{
	public override void OnInspectorGUI()
	{
		BezierController controller = (BezierController)target;

		DrawDefaultInspector();

		if (GUILayout.Button("Make new curve"))
		{
			Debug.Log("Button pressed.");

			BezierExample newBE = controller.Bezier.gameObject.AddComponent<BezierExample>();

			if (controller.curveList.Count > 0)
			{
				BezierExample lastBE = controller.curveList[controller.curveList.Count - 1];

				newBE.startPoint = lastBE.endPoint;
				//newBE.endPoint.Set(-lastBE.endPoint.x + 20, -lastBE.endPoint.y + 20, -lastBE.endPoint.z + 20);
				//newBE.startTangent = lastBE.endTangent;
				//in order to connect the two bezier curves smoothly
				newBE.startTangent.Set(2 * lastBE.endPoint.x -lastBE.endTangent.x, 2 * lastBE.endPoint.y -lastBE.endTangent.y, 2 * lastBE.endPoint.z -lastBE.endTangent.z);
				
				//newBE.endTangent.Set(-lastBE.endTangent.x + 20, -lastBE.endTangent.y + 20, -lastBE.endTangent.z + 20);
			}
			

			controller.curveList.Add(newBE);
		}
	}
}
