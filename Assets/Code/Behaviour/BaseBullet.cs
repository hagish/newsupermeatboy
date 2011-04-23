using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour
{
	//a bullet has a speed
	private float _flightSpeed = 2.0f;
	
	//Target
	public Vector3 TargetPositon;
	
	//flying distance
	private float _flightDistance = 3.0f;
	private float _flown = 0.0f;
	
	private bool _died = false;

	// Use this for initialization
	void Start()
	{
		Debug.Log("I have been created");
		
	}

	// Update is called once per frame
	void Update ()
	{
		if( _died == false )
		{
			float distance = Time.deltaTime * _flightSpeed;
			_flown += distance;
			if( _flown >= _flightDistance)
			{
				Destroy(gameObject, 2.0f);
				_died = true;
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, TargetPositon, _flightSpeed * Time.deltaTime);
			}
		}
	}
}

