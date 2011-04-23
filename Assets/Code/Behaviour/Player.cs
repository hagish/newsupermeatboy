using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Animation anim;
	private float deepHole = 0.0f;
	
	
	// PlayerLocals writes into this
	public bool onGround = true;
	public int playerNr = 0;
	
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
		playDirectedAnimation("idle", true);
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
	
	public void playAnimation(string name, bool looped)
	{
		anim.CrossFade(name);
		
		if (looped)
		{
			anim.wrapMode = WrapMode.Loop;
		}
		else
		{
			anim.wrapMode = WrapMode.Once;
		}
	}
	
	public void playDirectedAnimation(string name, bool looped)
	{
		playAnimation(name + movementPostfix(), looped);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < deepHole) Die();
	}
	
	void Respawn () {
		transform.position = Game.game.findSpawnPosition(playerNr);
		
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
