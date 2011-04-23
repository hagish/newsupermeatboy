using UnityEngine;
using System.Collections;

public class BandageGirl : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {

		// TODO: Play animation if available
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		other.SendMessage("GameWon", SendMessageOptions.DontRequireReceiver);
		
		// Play Winning animation
	}
}
