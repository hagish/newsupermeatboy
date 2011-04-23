using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
	
	public Vector3 newPosition;
	
	void OnTriggerEnter(Collider other)
	{
		other.transform.position = newPosition;
	}
}
