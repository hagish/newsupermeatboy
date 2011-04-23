using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {
	private string ip;
	private int port;
	
	private string mapToLoad = "level1";
	
	// Use this for initialization
	void Start () {
		ip = "192.168.2.139";
		port = 25000;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LaunchServer () {
		Debug.Log("starting server");
		bool useNat = !Network.HavePublicAddress();
		useNat = false;
		Network.InitializeServer(32, port, useNat);
	}
	
	void OnServerInitialized () {
		Debug.Log("server initialized and ready");
		Application.LoadLevel(mapToLoad);
	}
	
	void OnConnectedToServer () {
		Debug.Log("connected to server");
		Application.LoadLevel(mapToLoad);
	}
	
	void Join () {
		Debug.Log("joining " + ip);
		Network.Connect(ip, port);
	}
	
	void OnGUI () {
		if (GUILayout.Button("start server"))
		{
			if (Network.peerType == NetworkPeerType.Disconnected) {
				LaunchServer();
			}
		}
		
		if(GUILayout.Button("Level 1"))
		{
			mapToLoad = "level1";
		}
		
		if(GUILayout.Button("Level 2"))
		{
			mapToLoad = "level2";
		}
		
		if(GUILayout.Button("Level 3"))
		{
			mapToLoad = "level3";
		}
		
		GUILayout.Label("ip");
		ip = GUILayout.TextField(ip, 20);

		if (GUILayout.Button("join"))
		{
			if (Network.peerType == NetworkPeerType.Disconnected) {
				Join();
			}
		}
	}
}
