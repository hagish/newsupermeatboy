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
		
		if (GUI.Button(new Rect(75, 25, 300, 25), "Player One: " + InputConfig.P1InputType))
		{
			switch (InputConfig.P1InputType)
			{
				case "Arrow Keys": InputConfig.P1InputType = "Left/Right Arrow + Space"; break;
				case "Left/Right Arrow + Space": InputConfig.P1InputType = "IJKL"; break; 
				case "IJKL": InputConfig.P1InputType = "Joystick 1"; break; 
				case "Joystick 1": InputConfig.P1InputType = "Arrow Keys"; break;
			}
		}
		
		if (GUI.Button(new Rect(75, 75, 300, 25), "Player Two: " + InputConfig.P2InputType))
		{
			switch (InputConfig.P2InputType)
			{
				case "WSAD": InputConfig.P2InputType = "Joystick 2"; break;
				case "Joystick 2": InputConfig.P2InputType = "WSAD"; break; 
			}
		}
	}
}
