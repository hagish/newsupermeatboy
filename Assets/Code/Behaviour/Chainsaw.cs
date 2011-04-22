using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion rotation = Quaternion.AngleAxis(3f, Vector3.forward);
		transform.rotation *= rotation;
	}
}
