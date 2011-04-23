using UnityEngine;
using System.Collections;

public class NetworkHelper {
	public static NetworkHelper instance {
		get {
			if (instance == null)
			{
				instance = new NetworkHelper();	
			}
			
			return instance;
		}
		private set { instance = value; }
	}
	
	public NetworkHelper ()
	{
		
	}
	
	public bool isServer()
	{
		return Network.peerType == NetworkPeerType.Server || Network.peerType == NetworkPeerType.Disconnected;
	}
}
