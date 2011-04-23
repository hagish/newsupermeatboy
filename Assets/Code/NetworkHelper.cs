using UnityEngine;
using System.Collections;

public class NetworkHelper {

	public static bool isServer()
	{
		return Network.peerType == NetworkPeerType.Server || Network.peerType == NetworkPeerType.Disconnected;
	}
}
