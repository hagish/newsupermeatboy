using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Animation anim;
	
	// Use this for initialization
	void Start () {
		Respawn();
		
		anim = GetComponentInChildren<Animation>();
		anim.wrapMode = WrapMode.Loop;
		
		Idle();
	}
	
	void Idle()
	{
		// playDirectedAnimation("idle");
	}
	
	private string movementPostfix()
	{
		if (gameObject.GetComponent<MovementDirection>().lastDirection == MovementDirection.Direction.LEFT)
		{
			return "_l";	
		}
		else
		{
			return "_r";
		}
	}
	
	public void playDirectedAnimation(string name)
	{
		anim.CrossFade(name + movementPostfix());
	}
	
	// Update is called once per frame
	void Update () {
		playDirectedAnimation("run");
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
