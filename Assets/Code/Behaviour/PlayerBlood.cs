using UnityEngine;
using System.Collections;

public class PlayerBlood : MonoBehaviour {
	private GameObject blood;
	private MovementDirection direction;
	
	// Use this for initialization
	void Start () {
		blood = GameObjectHelper.findChildBySubstringInName(gameObject, "blood");
		direction = GetComponent<MovementDirection>();
	}
	
	// Update is called once per frame
	void Update () {
		Quaternion rot = Quaternion.identity;
		
		if (direction.lastDirection == MovementDirection.Direction.RIGHT)
		{
			rot = Quaternion.AngleAxis(180.0f, Vector3.up);
		}
		
		blood.transform.rotation = rot;
		
		bool particlesEnabled = direction.hasPositionChange();
		
		GameObjectHelper.visitComponentsInDirectChildren<ParticleEmitter>(blood, (e) => {
			e.emit = particlesEnabled;
		});
	}
}
