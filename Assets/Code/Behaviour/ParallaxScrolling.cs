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
	
	public bool bounds = true;
	

    private Vector3 distance;
    private Vector3 lastCamPos;
	
	private static float tolerance = 0.05f;
	private Camera myCamera;
	private Vector3 planeSize;


	// Use this for initialization
	void Start () {
    	// Try default camera
        cameraObject = GameObject.Find("Camera");
		myCamera = (Camera) cameraObject.GetComponent("Camera");
		
		planeSize = myCamera.ViewportToScreenPoint(this.transform.position);
		planeSize.x -= Screen.width;
		planeSize.y -= Screen.height;
	}
	
	void FixedUpdate()
	{
		lastCamPos = cameraObject.transform.position;
	}

	// Update is called once per frame
	void Update () {
		
		Vector3 planeCurPos = myCamera.ViewportToScreenPoint(this.transform.position);
		
		if (!capX)
		{
			if ((bounds) && (planeCurPos.x > 0) && (Screen.width < planeCurPos.x))
			{
		        if (Mathf.Abs(lastCamPos.x - cameraObject.transform.position.x) > tolerance)
		        {
		        	if (lastCamPos.x > cameraObject.transform.position.x) 
						transform.position = (!invertX) ? new Vector3(transform.position.x + (tolerance * speed), transform.position.y, transform.position.z) : new Vector3(transform.position.x - (tolerance * speed), transform.position.y, transform.position.z);
		        	
					if (lastCamPos.x < cameraObject.transform.position.x) 
						transform.position = (!invertX) ? new Vector3(transform.position.x - (tolerance * speed), transform.position.y, transform.position.z) : new Vector3(transform.position.x + (tolerance * speed), transform.position.y, transform.position.z);
				}
			}
		}
		
		if (!capY)
		{
			if ((bounds) && (planeCurPos.y > 0) && (Screen.height < planeCurPos.y))
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
}
