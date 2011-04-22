using UnityEngine;
using System.Collections;

/*
NOTE : keys	http://unity3d.com/support/documentation/ScriptReference/Input.GetKey.html
*/

public class MainCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = GameObject.Find("SpawnPoint").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float s = 0.1f;
		if (Input.GetKey("left"))	transform.position += new Vector3(-s,0,0);
		if (Input.GetKey("right"))	transform.position += new Vector3( s,0,0);
	
		if (Input.GetKey("up"))		transform.position += new Vector3(0, s,0);
		if (Input.GetKey("down"))	transform.position += new Vector3(0,-s,0);
	}
}
