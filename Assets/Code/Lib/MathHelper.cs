using UnityEngine;
using System.Collections;

public static class MathHelper {

	public static Vector3 projectOntoPlane( Vector3 vec, Vector3 planeNormal )
	{
	    return vec - Vector3.Dot(vec, planeNormal) * planeNormal;
	}
	
	public static bool isSameDirection(Vector3 dir1, Vector3 dir2){
		return Vector3.Dot(dir1, dir2) > 0.0f;	
	}
	
	/*
	 * circles hit <=> distance < 0
	 */
	public static float calculateCircleMinDistance(Vector3 posA, float radiusA, Vector3 posB, float radiusB){
		Vector3 d = posA - posB;
		return d.magnitude - radiusA - radiusB;
	}
	
	public static Vector3 getOrthogonalUnitVector(Vector3 normal)
	{
		Vector3 other = Vector3.zero;
		
		if (normal.x == 0.0f) {
			other.x = 1.0f;	
		} else {
			other.x = -normal.y;
			other.y = normal.x;
		}
		
		Vector3 ortho = projectOntoPlane(other.normalized, normal);
		
		return ortho;
	}
	
	public static Vector3 rotateVectorAroundAxis(Vector3 v, Vector3 axis, float rotationDegree)
	{
		Quaternion randomRotation = Quaternion.AngleAxis(rotationDegree, axis);
		return randomRotation * v;
	}
	
	public static Vector3 getRandomOrthogonalUnitVector(Vector3 normal)
	{
		Vector3 ortho = getOrthogonalUnitVector(normal);
		Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), normal);
		return randomRotation * ortho;
	}
	
	public static float mapIntoRange(float srcValue, float srcMin, float srcMax, float dstMin, float dstMax)
	{
		float r =  (Mathf.Clamp(srcValue, srcMin, srcMax) - srcMin) / (srcMax - srcMin);
		return dstMin + (dstMax - dstMin) * r;
	}
}
