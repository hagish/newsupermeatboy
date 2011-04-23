using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game game;
	
	private int winningPlayerNr = 0;
	
	public Vector3 findSpawnPosition(int playerNr)
	{
		if (GameObject.Find("SpawnPoint" + playerNr) != null) return GameObject.Find("SpawnPoint" + playerNr).transform.position;
		if (GameObject.Find("SpawnPoint") != null) return GameObject.Find("SpawnPoint").transform.position;
		return Vector3.zero;
	}
	
	// Use this for initialization
	void Start () {
		game = this;
		
		// keynames see http://unity3d.com/support/documentation/Manual/Input.html
		createLocalPlayer(1, "left", "right", "up");
		createLocalPlayer(2, "a", "d", "w");
	}
	
	public void playerFinished(int playerNr, Player player)
	{
		if (winningPlayerNr == 0)
		{
			winningPlayerNr = playerNr;	
		}
	}
	
	private void createLocalPlayer(int playerNr, string keyLeft, string keyRight, string keyJump)
	{
		Vector3 position = findSpawnPosition(playerNr);
		GameObject player = GameObjectHelper.createObject(gameObject, "Player", true, position, Quaternion.identity);
		GameObject.Find("Camera").GetComponent<CameraSmoothFollow>().setTarget(playerNr, player);
		player.AddComponent<PlayerLocal>().setKeyBindings(keyLeft, keyRight, keyJump);
		player.GetComponent<Player>().playerNr = playerNr;
		
		/*
		Material mat = (Material)Resources.Load("player" + playerNr);
		
		GameObjectHelper.visitComponentsDeep<SkinnedMeshRenderer>(player, (r) => {
			r.material = mat;
		});
		*/
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
	
	void OnGUI() {
		if (GUI.Button(new Rect(5, 5, 100, 25), "back to menu"))
	    {
	    	Application.LoadLevel(0);
	    }
	
		int x = (int)((float)Screen.width * 0.45f);
		int y = (int)((float)Screen.height * 0.45f);
		
		if (winningPlayerNr > 0)
		{
			GUI.Label(new Rect(x, y, 200, 50), "player " + winningPlayerNr + " won!");
			
		}
	}
}
