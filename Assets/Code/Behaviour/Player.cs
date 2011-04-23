using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private Animation anim;
	private float deepHole = 0.0f;
	
	
	// PlayerLocals writes into this
	public bool onGround = true;
	public int playerNr = 0;
	
	public static Player playerA = null;
	public static Player playerB = null;
	
	// Use this for initialization
	void Start () {
		if (playerA == null) playerA = this; else playerB = this;
		
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
	
	void SpawnGibs ()
	{
		var res = Resources.Load("MeatBall");
		if (res) for (int i=0;i<10;++i) {
			GameObject g = (GameObject)GameObject.Instantiate(res);
			Vector3 vel = Random.insideUnitSphere * 1f;
			Vector3 off = Random.onUnitSphere * 0.3f;
			vel.z *= 0.2f;
			off.z *= 0.2f;
			g.rigidbody.velocity = vel;
			g.rigidbody.position = transform.position + off;
		}
	}
	
	public void Die ()
	{
		SpawnGibs();
		Respawn();
	}
}
