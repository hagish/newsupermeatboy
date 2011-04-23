using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game game;
	
	public Transform playerPrefab;
	
	// Use this for initialization
	void Start () {
		game = this;
		
		Vector3 position = GameObject.Find("SpawnPoint").transform.position;
		GameObject player = GameObjectHelper.createObject(gameObject, "Player", true, position, Quaternion.identity);
		GameObject.Find("Camera").GetComponent<CameraSmoothFollow>().target = player;
		player.AddComponent<PlayerLocal>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void createNetworkPlayer ()
	{
		GameObject p = new GameObject("Player");
		p.AddComponent<Player>();
	}
	
	void OnPlayerConnected (NetworkPlayer player) {
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
		createNetworkPlayer();
	}
	
	void OnPlayerDisconnected (NetworkPlayer player) {
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
		Debug.Log("Player left from " + player.ipAddress + ":" + player.port);
	}
}
