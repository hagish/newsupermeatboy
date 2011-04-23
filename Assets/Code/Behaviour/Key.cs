using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	void OnTriggerEnter(Collider other)
	{
		other.SendMessage("KeyGot", SendMessageOptions.DontRequireReceiver);
		
		GameObjectHelper.visitComponentsDeep<Door>(Game.game.gameObject, (door) => {
			if (door.key == gameObject)
			{
				GameObject.Destroy(door.gameObject);	
			}
		});

		GameObject.Destroy(gameObject);
	}
}
