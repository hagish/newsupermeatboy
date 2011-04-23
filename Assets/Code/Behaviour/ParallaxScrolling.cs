using UnityEngine;
using System.Collections;

public class ParallaxScrolling : MonoBehaviour {
    public GameObject cameraObject = null;
	public float speed = 1.0f;
	
	public bool invertX = false;
	public bool invertY = false;
	
	public bool inverted 
	{ 
		get { return (invertX && invertY); } 
		set { invertX = true; invertY = true; } 
	}
	
	
	public bool capX = false;
	public bool capY = false;

    private Vector3 distance;
    private Vector3 lastCamPos;
	
	private static float tolerance = 0.05f;


	// Use this for initialization
	void Start () {
    	// Try default camera
        cameraObject = GameObject.Find("Camera");
	}
	
	void FixedUpdate()
	{
		lastCamPos = cameraObject.transform.position;
	}

	// Update is called once per frame
	void Update () {	
		
		if (!capX)
		{
	        if (Mathf.Abs(lastCamPos.x - cameraObject.transform.position.x) > tolerance)
	        {
	        	if (lastCamPos.x > cameraObject.transform.position.x) 
					transform.position = (!invertX) ? new Vector3(transform.position.x + (tolerance * speed), transform.position.y, transform.position.z) : new Vector3(transform.position.x - (tolerance * speed), transform.position.y, transform.position.z);
	        	
				if (lastCamPos.x < cameraObject.transform.position.x) 
					transform.position = (!invertX) ? new Vector3(transform.position.x - (tolerance * speed), transform.position.y, transform.position.z) : new Vector3(transform.position.x + (tolerance * speed), transform.position.y, transform.position.z);
			}
		}
		
		if (!capY)
		{
			if (Mathf.Abs(lastCamPos.y - cameraObject.transform.position.y) > tolerance)
	        {
	        	if (lastCamPos.y > cameraObject.transform.position.y) 
					transform.position = (!invertY) ? new Vector3(transform.position.x, transform.position.y + (tolerance * speed), transform.position.z) : new Vector3(transform.position.x, transform.position.y - (tolerance * speed), transform.position.z);
	        	
				if (lastCamPos.y < cameraObject.transform.position.y) 
					transform.position = (!invertY) ? new Vector3(transform.position.x, transform.position.y - (tolerance * speed), transform.position.z) : new Vector3(transform.position.x, transform.position.y + (tolerance * speed), transform.position.z);
			}
		}
	}
}
