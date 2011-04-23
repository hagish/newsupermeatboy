using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		if(GUI.Button(new Rect(25,200,200,25), "Level 1"))
		{
			Application.LoadLevel(1);
		}
		
		if(GUI.Button(new Rect(25,230,200,25), "Level 2"))
		{
			Application.LoadLevel(2);
		}
		
		if(GUI.Button(new Rect(25,260,200,25), "Level 3"))
		{
			Application.LoadLevel(3);
		}
	}
}
