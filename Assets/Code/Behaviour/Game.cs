using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game game;
	
	public Vector3 findSpawnPosition(int playerNr)
	{
		if (GameObject.Find("SpawnPoint" + playerNr) != null) return GameObject.Find("SpawnPoint" + playerNr).transform.position;
		if (GameObject.Find("SpawnPoint") != null) return GameObject.Find("SpawnPoint").transform.position;
		return Vector3.zero;
	}
	
	// Use this for initialization
	void Start () {
		game = this;

        switch (Input.P1InputType)
        {
        	case "Arrow Keys": createLocalPlayer(1, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow); break;
            case "Left/Right Arrow + Space": createLocalPlayer(1, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.Space); break;
            case "IJKL": createLocalPlayer(1, KeyCode.J, KeyCode.L, KeyCode.I); break;
            case "Joystick/Gamepad 1": createLocalPlayer(2, KeyCode.Joystick1Button7, KeyCode.Joystick1Button8, KeyCode.Joystick1Button16); break;
        }

        switch (Input.P2InputType)
        {
        	case "WSAD": createLocalPlayer(2, KeyCode.A, KeyCode.D, KeyCode.W); break;
            case "Joystick/Gamepad 2": createLocalPlayer(2, KeyCode.Joystick2Button7, KeyCode.Joystick2Button8, KeyCode.Joystick2Button16); break;
        }

	}
	
	private void createLocalPlayer(int playerNr, KeyCode keyLeft, KeyCode keyRight, KeyCode keyJump)
	{
		Vector3 position = findSpawnPosition(playerNr);
		GameObject player = GameObjectHelper.createObject(gameObject, "Player", true, position, Quaternion.identity);
		GameObject.Find("Camera").GetComponent<CameraSmoothFollow>().setTarget(playerNr, player);
		player.AddComponent<PlayerLocal>().setKeyBindings(keyLeft, keyRight, keyJump);
		player.GetComponent<Player>().playerNr = playerNr;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Joystick1Button16)) Debug.Log(KeyCode.Joystick1Button16);
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
