using UnityEngine;
using System.Collections;

public class BaseTurretHead : MonoBehaviour
{
	//this value indicates, when tower is ready to fire a new bullet
	private float TimeTillNextShot = 0.0f;
	
	//the turret checks if there is any player in its range
	private float AttackRange = 30.0f;
	
	//if the turret has found a player it follows the player
	private MainCharacter _target = null;
	
	public MainCharacter Target
	{
		get { return _target; }
		set { _target = value; }
	}
	
	//the turret fires all 'value' seconds a bullet
	private int AttackSpeed = 3;
	
	
	
	// Use this for initialization
	void Start ()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		if(Target == null)
			CheckForPlayersInRange();
		else
		{
			if(IsPlayerStillInRange())
			{
				Debug.Log("True");
			}
			else
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
		MainCharacter[] players = Game.game.GetComponentsInChildren<MainCharacter>();
			
		if(players.Length == 0)
			return; // as no player has been found
		
		foreach(var item in players)
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
	
	private bool IsPlayerStillInRange()
	{
		float distance = Vector3.Distance(transform.position, Target.transform.position);
		if( distance <= AttackRange)
			return true;
		
		return false;
	}
	
	private void RotateHead()
	{
		//i need current position of the target, and my own position
		//next i have to calculate the 
		Ray r = new Ray(transform.position, transform.rotation);
		Debug.Log(r);
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
		
		return false;
	}
	
	private void Fire()
	{
		//spawn bullet
	}
}

