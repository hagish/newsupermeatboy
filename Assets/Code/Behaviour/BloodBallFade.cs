using UnityEngine;
using System.Collections;

public class BloodBallFade : MonoBehaviour {
	static float fTimeToLive = 2f;
	
	float fDeathTime;
	// Use this for initialization
	void Start () {
		// fDeathTime = Time.time + fTimeToLive;
		Destroy(gameObject,fTimeToLive);
		// Debug.Log("BloodBallFade"+Time.time);
	}
	
	// Update is called once per frame
	void Update () {
		// if (Time.time > fDeathTime) Object.Destroy(this);
	}
}
