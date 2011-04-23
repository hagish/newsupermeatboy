using UnityEngine;
using System.Collections;

public class CameraSmoothFollow : MonoBehaviour {
	private GameObject player1;
	private GameObject player2;
	
	// Use this for initialization
	void Start () {
	
	}
	
	public void setTarget(int playerNr, GameObject target)
	{
		if (playerNr == 1)
		{
			player1 = target;	
		}
		if (playerNr == 2)
		{
			player2 = target;	
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Vector3 viewPos = camera.WorldToViewportPoint(target.transform.position);
		
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
		
		Vector3 pos = player1.transform.position;
		
		float z = transform.position.z;
		
		if (player2 != null)
		{
			pos += player2.transform.position;
			pos *= 0.5f;
			
			z = -10 - (player1.transform.position - player2.transform.position).magnitude;
		}
		
		float f = 0.04f;
		float fi = 1f - f;
			Vector3 newcampos = fi* transform.position + f* pos;
			newcampos.z = z;
			transform.position = newcampos;
	}
}
