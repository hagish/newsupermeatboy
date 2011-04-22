using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour {
	public Vector3 deltaPos;
	public float speed = 1.0f;
	
	private Vector3 animPos;
	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		
		// Set start positions
		startPos = transform.position;
		
		animPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Check if deltaPos has values and then apply interpolation
		if (deltaPos.x != 0.0f)
			animPos.x = Mathf.Lerp(startPos.x, startPos.x + deltaPos.x, Mathf.PingPong(Time.time * speed, 1));
			
		if (deltaPos.y != 0.0f)
			animPos.y = Mathf.Lerp(startPos.y, startPos.y + deltaPos.y, Mathf.PingPong(Time.time * speed, 1));
		
		if (deltaPos.z != 0.0f)
			animPos.z = Mathf.Lerp(startPos.z, startPos.z + deltaPos.z, Mathf.PingPong(Time.time * speed, 1));
		
		// Apply new position
		transform.position = new Vector3(animPos.x, animPos.y, animPos.z);
		
		// TODO: Rotation
		//Quaternion rotation = Quaternion.AngleAxis(3f, Vector3.forward);
		//transform.rotation *= rotation; 
		
	}
}
