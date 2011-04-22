using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game game;

	// Use this for initialization
	void Start () {
		game = this;
		
		// GameObjectHelper.createObject(gameObject, "Prefabs/Player", true, Vector3.zero, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
