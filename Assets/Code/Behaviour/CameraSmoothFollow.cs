using UnityEngine;
using System.Collections;

public class CameraSmoothFollow : MonoBehaviour {
	public GameObject target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			Vector3 viewPos = camera.WorldToViewportPoint(target.transform.position);
			
			float border = 0.1f;
			float speed = 1.0f;
				
			// out of visible 
			if (
			    viewPos.x < 0.0f + border || viewPos.x > 1.0f - border || 
			    viewPos.y < 0.0f + border || viewPos.y > 1.0f - border
			)
			{
				Vector3 fromTargetToCam = target.transform.position - transform.position;
				fromTargetToCam *= speed * Time.deltaTime;
				
				// keep z position
				fromTargetToCam.z = 0.0f;
				
				transform.position += fromTargetToCam;
			}	
		}
	}
}
