using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private float deepHole = 0.0f;
	
	// Use this for initialization
	void Start () {
		Respawn();
		
		Animation anim = GetComponentInChildren<Animation>();
		anim.CrossFade("run");
		anim.wrapMode = WrapMode.Loop;
		
		GameObject[] myObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		
		foreach (GameObject go in myObjects)
		{
			if (go.transform.position.y < deepHole) deepHole = go.transform.position.y;
		}
		
		deepHole = deepHole - 10.0f; //< Tolerance
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < deepHole) Die();
	}
	
	void Respawn () {
		transform.position = GameObject.Find("SpawnPoint").transform.position;
		
		PlayerLocal local = GetComponent<PlayerLocal>();
		
		if (local != null)
		{
			local.MyMoveInit();	
		}
	}
	
	void Die ()
	{
		Respawn();
	}
}
