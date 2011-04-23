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
		transform.position = GameObject.Find("SpawnPoint").transform.position;
	}
	
	bool	bTouchesWall = false;
	bool	bTouchesWallLeft = false;
	bool	bTouchesWallRight = false;
	int		iTouchesWallXNormal = 0;
	void	MyMoveInit	() {
		bTouchesWall = false;
		bTouchesWallLeft = false;
		bTouchesWallRight = false;
		iTouchesWallXNormal = 0;
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		//Debug.Log("mainchar:OnControllerColliderHit");
		if (Mathf.Abs(hit.moveDirection.x) < 0.3) return;
		bTouchesWall = true;
		if (hit.moveDirection.x < 0) { iTouchesWallXNormal =  1; bTouchesWallLeft = true; }
		if (hit.moveDirection.x > 0) { iTouchesWallXNormal = -1; bTouchesWallRight = true; }
	}
	
	// never called 
	void OnCollisionEnter(Collision collision) { Debug.Log("mainchar:OnCollisionEnter"); }
	
	// never called 
	void OnCollisionStay(Collision collisionInfo) { Debug.Log("mainchar:OnCollisionStay"); }

public static float speed = 8.0F;
public static float myJumpSpeed = 10.0F;
public static float gravity = 20.0F;
private Vector3 moveDirection = Vector3.zero;
bool bJumpBlocked = false;

	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		bool bJump = Input.GetKey("w") || Input.GetKey("up");
		bool bLeft = Input.GetKey("a") || Input.GetKey("left");
		bool bRight = Input.GetKey("d") || Input.GetKey("right");
		
		// bJumpBlocked : bJump is only true for one frame
		if (!bJump) bJumpBlocked = false;
		if (bJumpBlocked) bJump = false;
		if (bJump) bJumpBlocked = true;
		
		moveDirection.x = 0;
		if (bLeft)	moveDirection.x -= speed;
		if (bRight)	moveDirection.x += speed;
		
		if (controller.isGrounded) {
			//if (Input.GetKey("w") || Input.GetKey("up"))	moveDirection.y -= s;
			//if (Input.GetKey("s") || Input.GetKey("down"))	moveDirection.y += s;
			// Input.GetAxis("Horizontal")  Input.GetAxis("Vertical");
			// transform.position += new Vector3(-s,0,0);
			// rigidbody.AddForce(Vector3.left * s);
			
			if (bJump) moveDirection.y = myJumpSpeed;
		}
		
		float fGravFactor = 1;
		
		if (!controller.isGrounded && bTouchesWall) {
			bool bWallSlide = (bTouchesWallLeft && bLeft) || (bTouchesWallRight && bRight);
			if (bJump) {
				moveDirection.x = 0.8f*myJumpSpeed*iTouchesWallXNormal;
				moveDirection.y = 0.7f*myJumpSpeed;
			}
			if (moveDirection.y < 0f && bWallSlide) fGravFactor = 0.3f;
		}
		moveDirection.y -= gravity * fGravFactor * Time.deltaTime;
		MyMoveInit();
		controller.Move(moveDirection * Time.deltaTime);
		if (bTouchesWall) {
			
		}
	}
}
