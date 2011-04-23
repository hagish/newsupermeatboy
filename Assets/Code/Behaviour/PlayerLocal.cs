using UnityEngine;
using System.Collections;

/*
NOTE : 
docs	http://unity3d.com/support/documentation/
keys	http://unity3d.com/support/documentation/ScriptReference/Input.GetKey.html
phys	http://unity3d.com/support/documentation/Components/comp-DynamicsGroup.html
	rigid	http://unity3d.com/support/documentation/Components/class-Rigidbody.html
	char	http://unity3d.com/support/documentation/Components/class-CharacterController.html
mat		http://unity3d.com/support/documentation/ScriptReference/Material.html
printf : Debug.Log

spawn stuff :
		GameObject obj = GameObjectHelper.createObject(Game.game.gameObject, "Bullet", true, transform.position, transform.rotation);
		obj.GetComponent<BaseBullet>().TargetPositon = Target.transform.posit
		
		GameObject p = new GameObject("Player");
		p.AddComponent<Player>();

send messages
		other.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
*/

public class PlayerLocal : MonoBehaviour {
	
	private Player player;
	
	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
	}
	
	public static Object prefab_bloodball; 
	
	bool	bTouchesWall = false;
	bool	bTouchesWallLeft = false;
	bool	bTouchesWallRight = false;
	int		iTouchesWallXNormal = 0;
		
	public static float speed_ground = 15.0F;
	public static float speed_air    = 20.0F;
	public static float myJumpSpeed = 25.0F;
	public static float slide_keep_speed = 0.1f*speed_air; // move a bit against wall to keep sliding-collisions
	public static float slide_jump_speed_x = 0.8f*speed_air;
	public static float slide_jump_speed_y = 1.1f*myJumpSpeed;
	public static float slide_grav_factor = 0.4f;
	public static float airborne_x_slowdown_factor = 0.1f; // no dir key pressed
	public static float airborne_x_accel_factor = 0.1f; // dir key pressed
	public static float airborne_speed_change = 6f*speed_air;
	public static float grounded_speed_change = 6f*speed_ground;

	public static float gravity = 80.0F;
	private Vector3 moveSpeed = Vector3.zero;
	bool bJumpBlocked = false;
	bool bSlidingLeft = false;
	bool bSlidingRight = false;
	bool bLeftPressKnown = false;
	bool bRightPressKnown = false;
	bool bDirKeyPressedSinceJump = false;
	float nextbloodt = 0f;
	float fBloodInterval = 1f/30f;
	Vector3 vLastBloodPos;

	float time_since_jump = 0f;
	public static float time_since_jump_airmove_slower = 0.1f; // airmove ineffective shortly after jump
	
	/*public string keyLeft = "left";
	public string keyRight = "right";
	public string keyJump = "space";
	
	public void setKeyBindings(string left, string right, string jump)
	{
		this.keyLeft = left;	
		this.keyRight = right;
		this.keyJump = jump;
	}*/
	
	public KeyCode keyLeft = KeyCode.LeftArrow;
	public KeyCode keyRight = KeyCode.RightArrow;
	public KeyCode keyJump = KeyCode.UpArrow;
	
	public void setKeyBindings(KeyCode left, KeyCode right, KeyCode jump)
	{
		this.keyLeft = left;	
		this.keyRight = right;
		this.keyJump = jump;
	}
	
	
	static bool BoundsContainedIn (Bounds inner,Bounds outer) {
		return	inner.min.x >= outer.min.x &&
				inner.min.y >= outer.min.y &&
				inner.min.z >= outer.min.z &&
				inner.max.x <= outer.max.x &&
				inner.max.y <= outer.max.y &&
				inner.max.z <= outer.max.z;
	}
	
	static void SpawnBloodOnContact (ControllerColliderHit hit) {
		Vector3 hit_moveDirection = hit.moveDirection;
		Vector3 hit_point = hit.point;
		Vector3 hit_normal = hit.normal;
	
		// orhto test to avoid misplaced blobs at edges : didn't work well
		// float dx = hit_moveDirection.x;
		// float dy = hit_moveDirection.y;
		// float fMinOrtho = 0.8f;
		// if (Mathf.Abs(dx) <= fMinOrtho && Mathf.Abs(dy) <= fMinOrtho) return;
		
		
		// TODO : network ? GameObject obj = GameObjectHelper.createObject(Game.game.gameObject, "Sphere", true, transform.position+hit.moveDirection, transform.rotation);
		var res = Resources.Load("BloodBall");
		if (res) {
			GameObject g = (GameObject)GameObject.Instantiate(res);
			
			Vector3 pos = hit_point;
			Vector3 forward = hit_normal;
			Vector3 up = Vector3.Cross(forward,Vector3.forward);
			Quaternion rot = Quaternion.LookRotation(up, forward);
			
			g.transform.position = pos;
			g.transform.rotation = rot;
			
			/*
			// check boundingbox to avoid misplaced blobs at edges
			Bounds b1 = hit.collider.bounds;
			Bounds b2 = g.GetComponentInChildren<MeshRenderer>().bounds;
			float e = 4f*Mathf.Min(Mathf.Min(b2.extents.x,b2.extents.y),b2.extents.z);
			b1.Expand(new Vector3(e,e,e));
			if (!BoundsContainedIn(b2,b1)) {
				GameObject.Destroy(g);
			}
			*/
			
			// p.AddComponent<MeshFilter>();
			// p.AddComponent<MeshRenderer>();
			// g.transform.rotation = transform.rotation;
			// g.transform.parent = transform;
		}
	}

	public void	MyMoveInit	() {
		bTouchesWall = false;
		bTouchesWallLeft = false;
		bTouchesWallRight = false;
		iTouchesWallXNormal = 0;
		
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		//Debug.Log("mainchar:OnControllerColliderHit");
		
		// collide blocks speed
		float e = 0.3f;
		if (hit.moveDirection.x < -e) moveSpeed.x = Mathf.Max(0f,moveSpeed.x);
		if (hit.moveDirection.x >  e) moveSpeed.x = Mathf.Min(0f,moveSpeed.x);
		if (hit.moveDirection.y < -e) moveSpeed.y = Mathf.Max(0f,moveSpeed.y);
		if (hit.moveDirection.y >  e) moveSpeed.y = Mathf.Min(0f,moveSpeed.y);
		
		// sideway collision
		if (Mathf.Abs(hit.moveDirection.x) > 0.3f) {
			bTouchesWall = true;
			if (hit.moveDirection.x < 0f) { iTouchesWallXNormal =  1; bTouchesWallLeft = true; }
			if (hit.moveDirection.x > 0f) { iTouchesWallXNormal = -1; bTouchesWallRight = true; }
		}
		
		// blood test 1
		GameObject o = hit.gameObject;
		// if (o) o.renderer.material.color = Color.red;

		// blood test 2
		if (Time.time > nextbloodt) {
			nextbloodt = Time.time + fBloodInterval;
			SpawnBloodOnContact(hit);
			
		}
	}
	
	// never called 
	void OnCollisionEnter(Collision collision) {  }
	
	// never called 
	void OnCollisionStay(Collision collisionInfo) {  }

float  Interpolate ( float a, float b,float t) {
	if (t < 0f) return a;
	if (t > 1f) return b;
	return (1f-t)*a + t*b;
}

float	ChangeValueWithSpeed	(float old,float target,float change_speed) {
	float dist = (target > old) ? (target - old) : (old - target);
	if (dist < change_speed) {
		return target;
	} else {
		if (target > old) 
				return old + change_speed;
		else	return old - change_speed;
	}
}

	// Update is called once per frame
	void Update () {
		CharacterController controller = GetComponent<CharacterController>();
		bool bJump = Input.GetKey(keyJump);
		bool bLeft = Input.GetKey(keyLeft);
		bool bRight = Input.GetKey(keyRight);
		if (!bLeft) bLeftPressKnown = false;
		if (!bRight) bRightPressKnown = false;
		if ((bLeft && !bLeftPressKnown) || (bRight && !bRightPressKnown)) bDirKeyPressedSinceJump = true;
		
		time_since_jump += Time.deltaTime;
		if (controller.isGrounded) time_since_jump = 0f;
		
		// bJumpBlocked : bJump is only true for one frame
		if (!bJump) bJumpBlocked = false;
		if (bJumpBlocked) bJump = false;
		if (bJump) bJumpBlocked = true;
		
		if (controller.isGrounded) {
			float target_x_speed = 0;
			if (bLeft)	target_x_speed = -speed_ground;
			if (bRight)	target_x_speed =  speed_ground;
			// Debug.Log("ground:"+moveSpeed.x+","+target_x_speed+","+ChangeValueWithSpeed(moveSpeed.x,target_x_speed,grounded_speed_change * Time.deltaTime));
			moveSpeed.x = ChangeValueWithSpeed(moveSpeed.x,target_x_speed,grounded_speed_change * Time.deltaTime);
			// moveSpeed.x = target_speed;
			//if (Input.GetKey("w") || Input.GetKey("up"))	moveSpeed.y -= s;
			//if (Input.GetKey("s") || Input.GetKey("down"))	moveSpeed.y += s;
			// Input.GetAxis("Horizontal")  Input.GetAxis("Vertical");
			// transform.position += new Vector3(-s,0,0);
			// rigidbody.AddForce(Vector3.left * s);
			if (bJump) { // jump from ground
				bDirKeyPressedSinceJump = false;
				moveSpeed.y = myJumpSpeed;
				time_since_jump = 0f;
			}
		} else {
			// airborne, not instant velocity set.
			// if (bDirKeyPressedSinceJump) {
				float target_x_speed = 0f;
				if (bLeft)	target_x_speed = -speed_air;
				if (bRight)	target_x_speed =  speed_air;
				// Debug.Log("airborne:"+moveSpeed.x+","+target_x_speed+","+ChangeValueWithSpeed(moveSpeed.x,target_x_speed,airborne_speed_change * Time.deltaTime));
				float change_speed = airborne_speed_change;
				if (time_since_jump < time_since_jump_airmove_slower)
					change_speed *= time_since_jump/time_since_jump_airmove_slower;
				moveSpeed.x = ChangeValueWithSpeed(moveSpeed.x,target_x_speed,change_speed * Time.deltaTime);
			// }
				
			/*
			if (target_x_speed == 0f) {
				if (bDirKeyPressedSinceJump) // don't break if slide-jump and no other key pressed yet
					moveSpeed.x *= 1f-airborne_x_slowdown_factor;
			} else {
				float f = airborne_x_accel_factor;
				float fi = 1f - f;
				moveSpeed.x = fi*moveSpeed.x + f*target_x_speed;
			}
			*/
		}
		
		
		
		/*
		if (!controller.isGrounded && bTouchesWall) {
			bool bWallSlide = (bTouchesWallLeft && bLeft) || (bTouchesWallRight && bRight);
			if (bJump) {
				moveSpeed.x = slide_jump_speed_x*iTouchesWallXNormal;
				moveSpeed.y = slide_jump_speed_y;
			}
			if (moveSpeed.y < 0f && bWallSlide) fGravFactor = 0.3f;
		}
		*/
		
		float fGravFactor = 1;
		
		// when sliding move a bit in the direction of the wall, so we get new collision messages
		if (!controller.isGrounded) {
			if (bSlidingLeft) moveSpeed.x -= slide_keep_speed * Time.deltaTime;
			if (bSlidingRight) moveSpeed.x += slide_keep_speed * Time.deltaTime;
		}
		
		// when sliding, slow down y movement (reduce gravity)
		if ((bSlidingLeft || bSlidingRight) && moveSpeed.y < 0f) { fGravFactor = slide_grav_factor; }
		
		// jump while sliding
		bool bSlideJump = !controller.isGrounded && bJump && (bSlidingLeft || bSlidingRight);
		if (bSlideJump) {
			bDirKeyPressedSinceJump = false;
			time_since_jump = 0f;
			if (bSlidingLeft) {
				moveSpeed.x =  slide_jump_speed_x;
				moveSpeed.y =  slide_jump_speed_y;
			}
			if (bSlidingRight) {
				moveSpeed.x = -slide_jump_speed_x;
				moveSpeed.y =  slide_jump_speed_y;
			}
		}
		moveSpeed.y -= gravity * fGravFactor * Time.deltaTime;
		
		bSlidingLeft = false;
		bSlidingRight = false;
		MyMoveInit();
		controller.Move(moveSpeed * Time.deltaTime);
		
		if (bTouchesWallLeft) bSlidingLeft = true;
		if (bTouchesWallRight) bSlidingRight = true;
		
		
		// animation handling
		if (bSlidingLeft) {
			player.playAnimation("slide_l", false);
		} else if (bSlidingRight) {
			player.playAnimation("slide_r", false);
		} else if (!controller.isGrounded) {
			player.playDirectedAnimation("jump", false);
		} else if (	moveSpeed.sqrMagnitude > 0.01f ){
			player.playDirectedAnimation("run", true);
		} else {
			player.playDirectedAnimation("idle", true);	
		}
		
		player.onGround = controller.isGrounded;
	}
}
