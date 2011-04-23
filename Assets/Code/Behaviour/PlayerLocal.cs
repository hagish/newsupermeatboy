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

public class PlayerLocal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	bool	bTouchesWall = false;
	bool	bTouchesWallLeft = false;
	bool	bTouchesWallRight = false;
	int		iTouchesWallXNormal = 0;
	
	public void	MyMoveInit	() {
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
	void OnCollisionEnter(Collision collision) {  }
	
	// never called 
	void OnCollisionStay(Collision collisionInfo) {  }

public static float speed_ground = 15.0F;
public static float speed_air    = 20.0F;
public static float myJumpSpeed = 25.0F;
public static float slide_keep_speed = 0.1f*speed_air; // move a bit against wall to keep sliding-collisions
public static float slide_jump_speed_x = 0.6f*speed_air;
public static float slide_jump_speed_y = 0.8f*myJumpSpeed;
public static float slide_grav_factor = 0.4f;
public static float airborne_x_slowdown_factor = 0.1f; // no dir key pressed
public static float airborne_x_accel_factor = 0.1f; // dir key pressed
public static float airborne_speed_change = 6f*speed_air;

public static float gravity = 80.0F;
private Vector3 moveDirection = Vector3.zero;
bool bJumpBlocked = false;
bool bSlidingLeft = false;
bool bSlidingRight = false;
bool bLeftPressKnown = false;
bool bRightPressKnown = false;
bool bDirKeyPressedSinceJump = false;

float  ApplyX ( float x0,float dt, float x1) {
	float f = Mathf.Min(1f,dt * 2f);
	float fi = 1f - f;
	return fi*x0 + f*x1;
}
	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		bool bJump = Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("space");
		bool bLeft = Input.GetKey("a") || Input.GetKey("left");
		bool bRight = Input.GetKey("d") || Input.GetKey("right");
		if (!bLeft) bLeftPressKnown = false;
		if (!bRight) bRightPressKnown = false;
		if ((bLeft && !bLeftPressKnown) || (bRight && !bRightPressKnown)) bDirKeyPressedSinceJump = true;
		
		// bJumpBlocked : bJump is only true for one frame
		if (!bJump) bJumpBlocked = false;
		if (bJumpBlocked) bJump = false;
		if (bJump) bJumpBlocked = true;
		
		if (controller.isGrounded) {
			moveDirection.x = 0;
			//if (Input.GetKey("w") || Input.GetKey("up"))	moveDirection.y -= s;
			//if (Input.GetKey("s") || Input.GetKey("down"))	moveDirection.y += s;
			// Input.GetAxis("Horizontal")  Input.GetAxis("Vertical");
			// transform.position += new Vector3(-s,0,0);
			// rigidbody.AddForce(Vector3.left * s);
			if (bLeft)	moveDirection.x = -speed_ground;
			if (bRight)	moveDirection.x =  speed_ground;
			
			if (bJump) { // jump from ground
				bDirKeyPressedSinceJump = false;
				moveDirection.y = myJumpSpeed;
			}
		} else {
			// airborne, not instant velocity set.
			// if (bDirKeyPressedSinceJump) {
				float target_x_speed = 0f;
				if (bLeft)	target_x_speed = -speed_air;
				if (bRight)	target_x_speed =  speed_air;
				
				float my_speed_change = airborne_speed_change * Time.deltaTime;
				if (Mathf.Abs(moveDirection.x-target_x_speed) < my_speed_change) {
					moveDirection.x = target_x_speed;
				} else {
					if (target_x_speed > moveDirection.x) 
							moveDirection.x += my_speed_change;
					else	moveDirection.x -= my_speed_change;
				}
			// }
				
			/*
			if (target_x_speed == 0f) {
				if (bDirKeyPressedSinceJump) // don't break if slide-jump and no other key pressed yet
					moveDirection.x *= 1f-airborne_x_slowdown_factor;
			} else {
				float f = airborne_x_accel_factor;
				float fi = 1f - f;
				moveDirection.x = fi*moveDirection.x + f*target_x_speed;
			}
			*/
		}
		
		
		
		/*
		if (!controller.isGrounded && bTouchesWall) {
			bool bWallSlide = (bTouchesWallLeft && bLeft) || (bTouchesWallRight && bRight);
			if (bJump) {
				moveDirection.x = slide_jump_speed_x*iTouchesWallXNormal;
				moveDirection.y = slide_jump_speed_y;
			}
			if (moveDirection.y < 0f && bWallSlide) fGravFactor = 0.3f;
		}
		*/
		
		float fGravFactor = 1;
		
		// when sliding move a bit in the direction of the wall, so we get new collision messages
		if (bSlidingLeft) moveDirection.x -= slide_keep_speed;
		if (bSlidingRight) moveDirection.x += slide_keep_speed;
		
		// when sliding, slow down y movement (reduce gravity)
		if ((bSlidingLeft || bSlidingRight) && moveDirection.y < 0f) { fGravFactor = slide_grav_factor; }
		
		// jump while sliding
		bool bSlideJump = !controller.isGrounded && bJump && (bSlidingLeft || bSlidingRight);
		if (bSlideJump) {
			bDirKeyPressedSinceJump = false;
			if (bSlidingLeft) {
				moveDirection.x =  slide_jump_speed_x;
				moveDirection.y =  slide_jump_speed_y;
			}
			if (bSlidingRight) {
				moveDirection.x = -slide_jump_speed_x;
				moveDirection.y =  slide_jump_speed_y;
			}
		}
		moveDirection.y -= gravity * fGravFactor * Time.deltaTime;
		
		bSlidingLeft = false;
		bSlidingRight = false;
		MyMoveInit();
		controller.Move(moveDirection * Time.deltaTime);
		
		if (bTouchesWallLeft) bSlidingLeft = true;
		if (bTouchesWallRight) bSlidingRight = true;
	}
}
