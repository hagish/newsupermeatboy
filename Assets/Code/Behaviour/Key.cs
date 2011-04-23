using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	
	public bool UseAnimation = true;
	private float deltaY = 0.2f;
	
	private Vector3 startPos;
	private Vector3 endPos;
	
	private bool isNetworkStub = false;

	void OnNetworkInstantiate(NetworkMessageInfo info) {
		isNetworkStub = true;
	}
	
	// Use this for initialization
	void Start () {
		
		// Set start & end position
		startPos = transform.position;
		
		endPos = new Vector3(transform.position.x, transform.position.y + deltaY, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (isNetworkStub)return;

		// Check if deltaPos has values and then apply interpolation
        if (UseAnimation)
		{
			transform.position = Vector3.Lerp(startPos, endPos, Mathf.PingPong(Time.time * 0.5f, 1));
		
			Quaternion rotation = Quaternion.AngleAxis(Time.deltaTime * 50, Vector3.up);
			transform.rotation *= rotation;
		}
	}
	
	void OnTriggerEnter(Collider other)
	{
		other.SendMessage("KeyGot", SendMessageOptions.DontRequireReceiver);
		this.renderer.enabled = false;
		
		GameObjectHelper.visitComponentsInDirectChildren<Door>(Game.game.gameObject, (door) => {
			if (door.key == gameObject)
			{
				GameObject.Destroy(door.gameObject);	
			}
		});

		GameObject.Destroy(this);
	}
}
