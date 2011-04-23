using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour
{
	//a bullet has a speed
	private float _flightSpeed = 4.0f;
	
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
		Fly();
	}
	
	void OnCollisionEnter(Collision otherObject)
	{
	    /*var value = otherObject.gameObject.GetComponent<Player>();
		
		if(value != null)
		{
			value.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
		}
		
		if(otherObject.gameObject.GetComponent<BaseBullet>() == null)
		{
		}*/
	}
	
	void OnTriggerEnter(Collider otherObject)
	{
		var value = otherObject.gameObject.GetComponent<Player>();
		
		if(value != null)
		{
			value.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
			Destroy(gameObject);
		}
	}

	private void Fly()
	{
		float distance = Time.deltaTime * _flightSpeed;
		_flown += distance;
		if( _flown >= _flightDistance)
		{
			Destroy(gameObject);
		}
		else
		{
			transform.position = Vector3.MoveTowards(transform.position, TargetPositon, _flightSpeed * Time.deltaTime);
		}
	}
}

