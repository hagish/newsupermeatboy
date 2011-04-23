using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Respawn();
		
		Animation anim = GetComponentInChildren<Animation>();
		anim.CrossFade("run");
		anim.wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
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
