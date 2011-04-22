using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugHelper
{
	public static void assert(bool assertion, String message)
	{
		if (!assertion)
		{
			Debug.LogError("ASSERT: " + message);
		}
	}
	
	public static void drawCylinder(Vector3 bottomCenter, Vector3 topCenter, float radius, int segments, Color color)
	{
		Vector3 backbone = topCenter - bottomCenter;
		Vector3 ortho = MathHelper.getOrthogonalUnitVector(backbone);
		
		Vector3 lastRotatedOrtho = Vector3.zero;
		
		for (int i = 0; i <= segments; ++i)
		{
			float degree = (float)i * (360.0f / segments);
			
			Vector3 rotatedOrtho = MathHelper.rotateVectorAroundAxis(ortho, backbone, degree);
			rotatedOrtho = rotatedOrtho.normalized * radius;
			
			Debug.DrawLine(bottomCenter, bottomCenter + rotatedOrtho, color);
			Debug.DrawLine(topCenter, topCenter + rotatedOrtho, color);
			
			Debug.DrawLine(bottomCenter + rotatedOrtho, topCenter + rotatedOrtho, color);
			
			if (i > 0)
			{
				Debug.DrawLine(bottomCenter + rotatedOrtho, bottomCenter + lastRotatedOrtho, color);
				Debug.DrawLine(topCenter + rotatedOrtho, topCenter + lastRotatedOrtho, color);
			}
			
			lastRotatedOrtho = rotatedOrtho;			
		}
	}
	
	public static void drawPath(LinkedList<Vector3> points, Vector3 translation, Color color)
	{
		bool first = true;
		Vector3 last = Vector3.zero;
		
		foreach(Vector3 point in points)
		{
			if (first)
			{
				first = false;
				last = point + translation;
			}
			else
			{
				Debug.DrawLine(last, point + translation, color);
				last = point + translation;
			}
		}
	}
	
	public static bool areGizmosVisible()
	{
#if UNITY_EDITOR
		Assembly asm = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		Type type = asm.GetType("UnityEditor.GameView");
		if (type != null)
		{
			EditorWindow window = EditorWindow.GetWindow(type);
			FieldInfo gizmosField = type.GetField("m_Gizmos", BindingFlags.NonPublic | BindingFlags.Instance);
			if(gizmosField != null)
				return (bool)gizmosField.GetValue(window);
		}
		return false;
#else
		return false;	
#endif
	}

}
