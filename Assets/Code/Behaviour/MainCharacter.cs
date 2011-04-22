using UnityEngine;
using System.Collections;

/*
NOTE : 
docs	http://unity3d.com/support/documentation/
keys	http://unity3d.com/support/documentation/ScriptReference/Input.GetKey.html
phys	http://unity3d.com/support/documentation/Components/comp-DynamicsGroup.html
	rigid	http://unity3d.com/support/documentation/Components/class-Rigidbody.html
	char	http://unity3d.com/support/documentation/Components/class-CharacterController.html
printf : Debug.Log
*/

public class MainCharacter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
public float speed = 6.0F;
public float jumpSpeed = 8.0F;
public float gravity = 20.0F;
private Vector3 moveDirection = Vector3.zero;

	// Update is called once per frame
	void Update () {
		float s = 2;
		Vector3 vmove = Vector3.zero;
		CharacterController controller = GetComponent<CharacterController>();
		bool bJump = Input.GetKey("w") || Input.GetKey("up");
		if (Input.GetKey("a") || Input.GetKey("left"))	vmove.x -= s;
		if (Input.GetKey("d") || Input.GetKey("right"))	vmove.x += s;
		//if (Input.GetKey("w") || Input.GetKey("up"))	vmove.y -= s;
		//if (Input.GetKey("s") || Input.GetKey("down"))	vmove.y += s;
		// Input.GetAxis("Horizontal")  Input.GetAxis("Vertical");
		// transform.position += new Vector3(-s,0,0);
		// rigidbody.AddForce(Vector3.left * s);
		//float s = 0.1f * Time.deltaTime;
		
	
		if (controller.isGrounded) {
			moveDirection = vmove;
			if (bJump) moveDirection.y = jumpSpeed;
		} else {
			moveDirection.x = vmove.x;
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
		
	}
}
