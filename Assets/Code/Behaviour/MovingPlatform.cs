using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {
	public Vector3 deltaPos;
	public float speed = 1.0f;


	private Vector3 startPos;
	private bool isMovingUp;
	
	// Use this for initialization
	void Start () {
		
		// Set start position
		startPos = transform.position;
	}
	/*
	// Update is called once per frame
	void Update () {

		// Check if deltaPos has values and then apply interpolation
        transform.position = Vector3.Lerp(startPos, startPos + deltaPos, Mathf.PingPong(Time.time * speed, 1));
        //rigidbody.AddForce(deltaPos * speed);
		
		// TODO: Rotation
		//Quaternion rotation = Quaternion.AngleAxis(3f, Vector3.forward);
		//transform.rotation *= rotation; 
		
	}*/

     //private Vector3 speed = new Vector3(3, 0, 0);
     void FixedUpdate()
     {
        transform.position = Vector3.Lerp(startPos, startPos + deltaPos, Mathf.PingPong(Time.time * speed, 1));
     }
    
	void OnCollisionEnter(Collision collision)
	{
		collision.rigidbody.useGravity = false;
		collision.transform.parent = this.transform;
		
		collision.transform.localPosition = new Vector3(0.0f, transform.localPosition.y, 0.0f);
		//collision.transform.TransformPoint(transform.position.x, collision.transform.position.y
		//collision.transform.parent = this.transform;
		//collision.transform.position = new Vector3(collision.transform.position.x, transform.position.y + transform.localScale.y, collision.transform.position.z);
	}
	
	void OnCollisionExit(Collision collision)
	{
		collision.rigidbody.useGravity = true;
		
		//collision.transform.parent = null;
		
	}
}
