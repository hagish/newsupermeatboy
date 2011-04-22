using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
	public LinkedList<Vector3> points { get; private set; }
	
	public Shape ()
	{
		points = new LinkedList<Vector3>();
	}
	
	// --------------------------------
	
	public static Shape create2DCircleWithSegmentOutlineSize(Vector3 center, Vector3 centerNormal, float radius, float segmentOutlineSize)
	{
		float outline = 2.0f * (float)Math.PI * radius;
		int numberOfSegments = (int)Math.Floor(outline / segmentOutlineSize);
		return create2DCircleWithSegmentCount(center, centerNormal, radius, numberOfSegments);
	}
	
	public static Shape create2DCircleWithSegmentCount(Vector3 center, Vector3 centerNormal, float radius, int segments)
	{
		Shape circle = new Shape();
		
		Vector3 ortho = MathHelper.getRandomOrthogonalUnitVector(centerNormal);
		
		for (int i = 0; i < segments; ++i)
		{
			float degree = (float)i * (360.0f / (float)segments);
			
			Vector3 rotatedOrtho = MathHelper.rotateVectorAroundAxis(ortho, centerNormal, degree);
			rotatedOrtho = rotatedOrtho.normalized * radius;
			
			circle.points.AddLast(center + rotatedOrtho);	
		}
		
		return circle;
	}
}
