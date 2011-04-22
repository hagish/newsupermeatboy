using UnityEngine;
using System.Collections;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void LaunchServer() {
		Debug.Log("starting server");
		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(32, 25000, useNat);
	}
	
	void OnServerInitialized() {
		Debug.Log("Server initialized and ready");
		Application.LoadLevel(1);
	}
	
	void Join () {
		Debug.Log("joining");	
	}
	
	void OnGUI () {
		if (GUILayout.Button("start server"))
		{
			if (Network.peerType == NetworkPeerType.Disconnected) {
				LaunchServer();
			}
		}

		if (GUILayout.Button("join"))
		{
			if (Network.peerType == NetworkPeerType.Disconnected) {
				Join();	
			}
		}
	}
}
