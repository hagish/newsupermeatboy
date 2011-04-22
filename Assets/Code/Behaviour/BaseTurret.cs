using UnityEngine;
using System.Collections;

public class BaseTurret : MonoBehaviour
{
	public bool IsWalkable { get; set; }
	
	private BaseTurretHead _turretHead = null;
	
	public BaseTurretHead TurretHead 
	{
		get
		{
			return _turretHead;
		}
		set
		{
			if(_turretHead == null)
				_turretHead = value;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
		TurretHead = new BaseTurretHead();
	}

	// Update is called once per frame
	void Update()
	{
	}
}

