using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour
{
	//a bullet has a speed
	private float _flightSpeed = 8.0f;
	
	private Vector3 _flightVector;
	
	public Vector3 FlightVector
	{
		get { return _flightVector; }
		set { _flightVector = value - transform.position; }
	}
	
	//flying distance
	private float _flightDistance = 12.0f;
	private float _flown = 0.0f;
	
	public float HitRadius = 0.25f;
	
	public GameObject Parent;
	
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update ()
	{
		Fly();
	}
	
	void OnTriggerEnter(Collider otherObject)
	{
		if(otherObject.transform.parent.gameObject != Parent)
		{
			var value = otherObject.gameObject.GetComponent<Player>();
			if(value != null)
			{
				Kill(value);
			}
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
			transform.position = Vector3.MoveTowards(transform.position, transform.position + FlightVector, _flightSpeed * Time.deltaTime);
		}
	}
	
	private void Kill(Player target)
	{
		target.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
	}
}

