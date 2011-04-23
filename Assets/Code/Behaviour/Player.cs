using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Animation anim;
	private float deepHole = 0.0f;
	// Use this for initialization
	void Start () {
		Respawn();
		
		anim = GetComponentInChildren<Animation>();
		anim.wrapMode = WrapMode.Loop;
		Idle();

		GameObject[] myObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];		
		
		foreach (GameObject go in myObjects)
		{
			if (go.transform.position.y < deepHole) deepHole = go.transform.position.y;
		}
		
		deepHole = deepHole - 10.0f; //< Tolerance
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
