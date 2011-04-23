using UnityEngine;
using System.Collections;

public class BandageGirl : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		Animation anim = transform.GetChild(0).GetComponentInChildren<Animation>();		
		anim.wrapMode = WrapMode.Loop;
		anim.Play();
	}
	
	// Update is called once per frame
	void Update () {

		// TODO: Play animation if available
		
	}
	
	void OnTriggerEnter(Collider other) 
	{
		Player p = other.gameObject.GetComponent<Player>();
		
		if (p != null)
		{
			// Play Winning animation
			Game.game.playerFinished(p.playerNr, p);
		}
	}
}
