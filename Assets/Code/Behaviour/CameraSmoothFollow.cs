using UnityEngine;
using System.Collections;

public class CameraSmoothFollow : MonoBehaviour {
	public GameObject target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 viewPos = camera.WorldToViewportPoint(target.transform.position);
		
		/*
		float border = 0.1f;
		float speed = 1.0f;
			
		// out of visible 
		if (
		    viewPos.x < 0.4f + border || viewPos.x > 0.6f - border || 
		    viewPos.y < 0.4f + border || viewPos.y > 0.6f - border
		)
		{
			Vector3 fromTargetToCam = target.transform.position - transform.position;
			fromTargetToCam *= speed * Time.deltaTime;
			
			// keep z position
			fromTargetToCam.z = 0.0f;
			
			transform.position += fromTargetToCam;
		}	
		*/
		float f = 0.04f;
		float fi = 1f - f;
			Vector3 newcampos = fi* transform.position + f* target.transform.position;
			newcampos.z = transform.position.z;
			transform.position = newcampos;
	}
}
