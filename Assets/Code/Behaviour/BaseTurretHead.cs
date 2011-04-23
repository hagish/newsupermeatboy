using UnityEngine;
using System.Collections;

public class BaseTurretHead : MonoBehaviour
{
	//this value indicates, when tower is ready to fire a new bullet
	private float TimeTillNextShot = 0.0f;
	
	//the turret checks if there is any player in its range
	private float AttackRange = 10.0f;
	
	//if the turret has found a player it follows the player
	private Player _target = null;
	
	public Player Target
	{
		get { return _target; }
		set { _target = value; }
	}
	
	public Rigidbody Bullet;
	
	//the turret fires all 'value' seconds a bullet
	public int AttackSpeed = 1;
	
	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		//RotateHead();
		if(Target == null)
			CheckForPlayersInRange();
		else
		{
			if(IsPlayerOutOfRange())
			{
				//the player is out of range, so we have no target
				Target = null;
				return;
			}
			
			RotateHead();
			
			//can tower fire
			if( ReadyToFire() )
				Fire();
		}
	}
	
	
	private void CheckForPlayersInRange()
	{
		Player[] players = Game.game.GetComponentsInChildren<Player>();
			
		if(players.Length == 0)
			return; // as no player has been found
		
		foreach(var item in players)
		{
			if(item is Player)
			{
				//calculate distance
				float distance = Vector3.Distance(transform.position, item.transform.position);
				
				if(distance <= AttackRange)
				{
					//there is a player in range
					Target = item;
					break;
				}
			}
		}
	}
	
	private bool IsPlayerOutOfRange()
	{
		float distance = Vector3.Distance(transform.position, Target.transform.position);
		if( distance <= AttackRange)
			return false;
		
		return true;
	}
	
	private void RotateHead()
	{
		transform.LookAt(Target.transform.position);
		
	}
	
	private bool ReadyToFire()
	{
		if( TimeTillNextShot < 0.0f )
		{
			TimeTillNextShot = AttackSpeed;
			return true;
		}
		else
		{
			TimeTillNextShot -= Time.deltaTime;
			return false;
		}
	}
	
	private void Fire()
	{
		//spawn bullet
		GameObject obj = GameObjectHelper.createObject(Game.game.gameObject, "Bullet", true, transform.position, transform.rotation);
		obj.GetComponent<BaseBullet>().TargetPositon = Target.transform.position;
	}
}

