using UnityEngine;
using System.Collections;

public class BaseTurretHead : MonoBehaviour
{
	//this value indicates, when tower is ready to fire a new bullet
	private float TimeTillNextShot = 0.0f;
	
	//the turret checks if there is any player in its range
	private float AttackRange = 30.0f;
	
	//if the turret has found a player it follows the player
	private Player _target = null;
	
	public Player Target
	{
		get { return _target; }
		set { _target = value; }
	}
	
	//the turret fires all 'value' seconds a bullet
	public int AttackSpeed = 3;
	
	private Vector3 _spawnPoint;
	
	// Use this for initialization
	void Start ()
	{
		_spawnPoint = transform.position + (transform.localScale * 0.5f);
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
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		
		obj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
		obj.transform.rotation = transform.rotation;
		obj.transform.Rotate(90.0f, 0.0f, 0.0f);
		obj.transform.localScale = new Vector3(0.25f,0.25f,0.25f);
		obj.name = "Bullet";
		obj.collider.isTrigger = true;
		BaseBullet bullet = obj.AddComponent<BaseBullet>();
		bullet.TargetPositon = Target.transform.position;
		
		
	}
}

