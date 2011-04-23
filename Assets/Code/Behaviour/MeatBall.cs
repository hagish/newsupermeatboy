using UnityEngine;
using System.Collections;

public class MeatBall : MonoBehaviour {
	static float fTimeToLive = 2f;
	
	// Use this for initialization
	void Start () {
		Destroy(gameObject,fTimeToLive * (0.5f + 0.5f*Random.value));
	}
	
	float nextbloodt = 0f;
	float fBloodInterval = 1f/10f;
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		// blood test 2
		// if (Time.time > nextbloodt) {
			// nextbloodt = Time.time + fBloodInterval;
			// PlayerLocal.SpawnBloodOnContact(hit);
		// }
	}
	
	void OnCollisionEnter(Collision collision) {
		if (Time.time > nextbloodt) {
			if (PlayerLocal.BloodAgainstObjectAllowed(collision.gameObject)) {
				foreach (ContactPoint contact in collision.contacts) {
					PlayerLocal.SpawnBloodOnContact2(contact.point, contact.normal);
					nextbloodt = Time.time + fBloodInterval;
				}
			}
		}
	}

	
	// Update is called once per frame
	void Update () {}
}
