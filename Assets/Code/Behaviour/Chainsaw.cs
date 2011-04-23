using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour {
	public Vector3 deltaPos;
	public float speed = 1.0f;

	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		
		// Set start position
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		// Check if deltaPos has values and then apply interpolation
        transform.position = Vector3.Lerp(startPos, startPos + deltaPos, Mathf.PingPong(Time.time * speed, 1));
		
		// TODO: Rotation
		//Quaternion rotation = Quaternion.AngleAxis(3f, Vector3.forward);
		//transform.rotation *= rotation; 
		
	}
	
	void OnTriggerEnter(Collider other) {
		other.SendMessage("Die");
	}
}
