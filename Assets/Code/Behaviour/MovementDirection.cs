using UnityEngine;
using System.Collections;

public class MovementDirection : MonoBehaviour {
	private Vector3 lastOtherPosition;
	private float lastOtherPositionTime;
	private float lastOtherSqrDistanceTreshold = 0.1f;
	private float lastOtherTimeout = 1.0f;
	private bool positionChanged = false;
	
	public enum Direction { LEFT, RIGHT };
	public Direction lastDirection;
	
	private Player player;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool hasPositionChange(){
		return positionChanged;	
	}
		
	private void updateOtherPosition(){
		lastOtherPosition = transform.position;
		lastOtherPositionTime = Time.time;		
	}
	
	private void notifyIdle(){
		updateOtherPosition();
		gameObject.SendMessage( "Idle", SendMessageOptions.DontRequireReceiver);
	}
		
	void FixedUpdate()
	{
		// detect last movement direction
		Vector3 direction = transform.position - lastOtherPosition;
		if (direction.sqrMagnitude > 0.01f)
		{
			if ( Vector3.Dot(direction, Vector3.right) > 0 )
			{
				lastDirection = MovementDirection.Direction.RIGHT;	
			}
			else
			{
				lastDirection = MovementDirection.Direction.LEFT;	
			}
		}
		
		positionChanged = false;
		// position changed?
		if ((lastOtherPosition - transform.position).sqrMagnitude > lastOtherSqrDistanceTreshold){
			updateOtherPosition();
			positionChanged = true;
		}
		
		// idle?
		if (Time.time - lastOtherPositionTime > lastOtherTimeout){
			notifyIdle();
		}
	}
}
