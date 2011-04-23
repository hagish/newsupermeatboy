using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour
{
	//a bullet has a speed
	private float _flightSpeed = 2.0f;
	
	//Target
	public Vector3 TargetPositon;
	
	//flying distance
	private float _flightDistance = 18.0f;
	private float _flown = 0.0f;
	
	// Use this for initialization
	void Start()
	{
		gameObject.AddComponent<NetworkView>();
	}

	// Update is called once per frame
	void Update ()
	{
		//Check if a player has been hit
		if(CollisionCheck() == false)
			Fly();
		else
		{
			
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		
		if( collision.gameObject is Player)
			Debug.Log("Hit");
	}

	
	private bool CollisionCheck()
	{
		
		
		return false;
	}
	
	private void Fly()
	{
		float distance = Time.deltaTime * _flightSpeed;
		_flown += distance;
		if( _flown >= _flightDistance)
		{
			Destroy(gameObject, 2.0f);
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, TargetPositon, _flightSpeed * Time.deltaTime);
		}
	}
}

