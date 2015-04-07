using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	//public float speed = 6.0F;
	public float jumpSpeed = 17.0F;
	public float gravity = 50.0F;
	public float RunSpeed = 13.0f;
	public float WalkSpeed = 7.5f;
	public float friction = -32.0f;
	public float acceleration = 6.0f;
	public float Airspeed = 12.5f;
	public float AirControl = 24.0f;
	public float AirDodgeSpeed = 8.0f;
	public float smooth = 5f;
	bool HasFallen = false;
	public bool FacingRight = true;
	bool HasDoubleJumped = false;
	private float jumpTimer = 0;
	bool startShorthopTimer = false;
	CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 targetAngles;
	private Vector3 right = new Vector3(0,0,1);
	private Vector3 left = new Vector3(0,0,-1);
	int voidLayer;
	public float MaxFallSpeed = -17.0f;
	public bool IsOnRightLedge = false;
	public bool IsOnLeftLedge = false;
	public int LedgeGrabCount = 0;
	public float TimeSinceLastGrab = 0.0000f;

	bool walking = false;
	bool running = false;
	bool HasAirDodged = false;
	bool falling = false;
	bool crouching = false;

	float DeltaY = 0;
	float LastY = 0;
	float LastXAxis = 0;

	float ControllerXAxis;
	float ControllerYAxis;
	float DeltaYAxis = 0;
	float LastYAxis = 0;
	float DeltaXAxis = 0;

	bool inAttack = false;

	public float jumpBufferTimer = 0;
	bool startJumpBufferTimer = false;

	public float DodgeBufferTimer = 0;
	bool startDodgeBufferTimer = false;

	public float movementBuffer = 0;
	bool movementBufferTimer = false;
	//public Transform character;

	private Animator anim = null;

	void Start()
	{	
		gameObject.SetActive (true);
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator> ();
		voidLayer = 31;
		anim.SetBool("Jump", false);
		//print ("started");
		//anim.Play ("Idle", 0);

	}

	void ResetCombatBool()
	{
		anim.SetBool ("FSmash", false);
	}

	void Awake()
	{
		print ("AWAKE");
	}

	void LateUpdate() 
	{
		ResetCombatBool ();
		ControllerXAxis = Input.GetAxis ("Mouse X") * 100;
		ControllerYAxis = Input.GetAxis ("Mouse Y") * 100;
		DeltaYAxis = ControllerYAxis - LastYAxis;
		DeltaXAxis = ControllerXAxis - LastXAxis;
		anim.SetBool("DoubleJump",false);
		anim.SetBool("Change Direction", false);
		//print (DeltaYAxis);
		if (controller.isGrounded) {
			moveDirection.y = 0;
			DeltaY = LastY - 0;
			anim.SetBool ("Falling", false);
		}

		if (moveDirection.y < -2.0f) 
		{
			falling = true;
			anim.SetBool ("Falling", true);
		}
		//
		//timers
		//
		if (startJumpBufferTimer) 
		{
			jumpBufferTimer+=Time.deltaTime;
		}
		if (startShorthopTimer) 
		{
			jumpTimer+=Time.deltaTime;
		}


//RESET ALL STUFF WHEN YOU HIT GROUND
		if (controller.isGrounded) 
		{
			anim.speed = 1;

			if (DeltaY < -2.0f)
			{
				anim.Play ("Idle 0",0);
				anim.SetBool("Landing",true);
			} 
			else
			{
					anim.SetBool("Landing",false);
			}
			moveDirection.y = 0;

			//anim.Play("Idle",0);
			
//reset shorthop timer
			if (jumpTimer > .2)
			{
				jumpTimer = 0;
				anim.speed = 1;
				startShorthopTimer = false;
			}
			HasAirDodged = false;
			IsOnLeftLedge = false;
			jumpSpeed = 22.0f;
			HasDoubleJumped = false;
			HasFallen = false;
			LedgeGrabCount = 0;
			this.gameObject.layer = 2;

//controller dash right
			if (ControllerYAxis > 7.2f)
			{
				anim.SetBool("Crouching",true);
				crouching = true;
			} else {
				crouching = false;
				anim.SetBool("Crouching", false);
			}
			if (ControllerXAxis < 1.3f && ControllerXAxis > -1.3f)
			{
				anim.SetBool("Running",false);
				running = false;
				walking = false;
				movementBufferTimer = false;
				movementBuffer = 0;
			}

			if (ControllerXAxis > 7.2f)
			{
				walking = false;
				running = true;
				anim.SetBool("Running",true);
				movementBufferTimer = true;
			
				if (moveDirection.z < 0)
				{
					moveDirection.z = moveDirection.z + .05f*acceleration;
				}
				else 
				{
					moveDirection.z = moveDirection.z + acceleration;
				}
				if (!FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
					   //Vector3 tmp = transform.eulerAngles;
						//tmp.y = 180.0f;
						//transform.eulerAngles = tmp;
						transform.Rotate(0,180,0,Space.World);
						FacingRight = true;
					}
				} 
			} 
			else if (ControllerXAxis < 7.2f)
			{
				if (ControllerXAxis > 3.0f) 
				{
					running = false;
					walking = true;
					anim.SetBool("Running",false);

					if (moveDirection.z < 0)
					{
						moveDirection.z = moveDirection.z + .05f*acceleration;
					}
					else 
					{
						moveDirection.z = moveDirection.z + acceleration;
					}
					if (!FacingRight)
					{
						if (jumpBufferTimer < 0.02)
						{
							//anim.SetBool("Change Direction", true);
							//Vector3 tmp = transform.eulerAngles;
							//tmp.y = 180.0f;
							//transform.eulerAngles = tmp;
							transform.Rotate(0,180,0,Space.World);
							FacingRight = true;
						}
					} 
				}
			}
//controler dash left
			if (ControllerXAxis < -7.2f)
			{
				running = true;
				walking = false;
				anim.SetBool("Running",true);
				movementBufferTimer = true;
				if (moveDirection.z > 0)
				{
					moveDirection.z -= .05f*acceleration;
				} 
				else
				{
					moveDirection.z -= acceleration;
				}
				if (FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						//anim.SetBool("Change Direction", true);
						//Vector3 tmp = transform.eulerAngles;
						//tmp.y = 180.0f;
						//transform.eulerAngles = tmp;
						transform.Rotate(0,180,0,Space.World);
						FacingRight = false;
					}
				} 
				anim.SetBool("Change Direction", false);
			}
			else if ((ControllerXAxis < -3.0f) && (ControllerXAxis > -7.2f))
			{
				running = false;
				walking = true;
				anim.SetBool("Running",false);
				if (moveDirection.z > 0)
				{
					moveDirection.z -= .05f*acceleration;
				} 
				else
				{
					moveDirection.z -= acceleration;
				}
				if (FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						//anim.SetBool("Change Direction", true);
						//Vector3 tmp = transform.eulerAngles;
						//tmp.y = 180.0f;
						//transform.eulerAngles = tmp;
						transform.Rotate(0,180,0,Space.World);
						FacingRight = false;
					}
					anim.SetBool("Change Direction", false);
				} 
			}
			//KEYBOARD IDLE
			/*
			if (!Input.GetButton("Right") || !Input.GetButton("Left"))
			{
				anim.SetBool("Running",false);
			}
			*/

			if (Input.GetButton ("Right"))
			{
				running = true;
				walking = false;
				anim.SetBool("Running",true);
				if (moveDirection.z < 0)
				{
					moveDirection.z = moveDirection.z + .05f*acceleration;
				}
				else 
				{
					moveDirection.z = moveDirection.z + acceleration;
				}
				if (!FacingRight)
				{
					if (jumpBufferTimer < 0.048)
					{
						//anim.SetBool("Change Direction", true);
						//Vector3 tmp = transform.eulerAngles;
						//tmp.y = 0.0f;
						//transform.eulerAngles = tmp;
						transform.Rotate(0,180,0,Space.World);
						FacingRight = true;
					}
					//anim.SetBool("Change Direction", false);
				} 
			}
//
//ground combat
//

			if (Input.GetButtonDown("A"))
			{
				if (movementBuffer < 0.20f)
				{
					anim.SetBool("Running",false);
					running = false;
					walking = false;
					print ("test2");
					//add more friction to slow movement faster
					if (moveDirection.z > .750f)
					{
						moveDirection.z += friction*1.3f * Time.deltaTime;
					}
					else if (moveDirection.z < -.750)
					{
						moveDirection.z -= friction*1.3f * Time.deltaTime;
					}
					else 
					{
						moveDirection.z = 0;
					}
					//inAttack = true;
					anim.SetBool("FSmash",true);
				}
			}


//
//cap speed
//
			if (moveDirection.z > RunSpeed && running)
			{
				moveDirection.z = RunSpeed;
			} 
			else if (moveDirection.z > WalkSpeed && walking)
			{
				moveDirection.z = WalkSpeed;
			}

			if (Input.GetButton ("Left"))
			{
				running = true;
				walking = false;
				anim.SetBool("Running",true);
	
				if (moveDirection.z > 0)
				{
					moveDirection.z -= .05f*acceleration;
				} 
				else
				{
					moveDirection.z -= acceleration;
				}
				if (FacingRight)
				{
					if (jumpBufferTimer < 0.048)
					{
						//Vector3 tmp = transform.eulerAngles;
						//tmp.y = 180.0f;
						//transform.eulerAngles = tmp;
						transform.Rotate(0,180,0,Space.World);
						FacingRight = false;
					}
				} 
			}
			if (moveDirection.z < -RunSpeed && running)
			{
				moveDirection.z = -RunSpeed;
			}
			else if (moveDirection.z < -WalkSpeed && walking)
			{
				moveDirection.z = -WalkSpeed;
			}
		} 
		else 
		{		
//air control calculations
			anim.SetBool("Running",false);
			if (Input.GetButtonDown("Right") || ControllerXAxis > 4.0f)
			{
				if (IsOnRightLedge == true)
				{
					IsOnRightLedge = false;
					HasDoubleJumped = false;
					jumpSpeed = 22.0f;
					LedgeGrabCount++;
					TimeSinceLastGrab = 0;
					moveDirection.z = 0;
				}
			}
			if (Input.GetButtonDown("Left") || ControllerXAxis < -4.0f)
			{
				if (IsOnLeftLedge == true)
				{
					IsOnLeftLedge = false;
					HasDoubleJumped = false;
					jumpSpeed = 22.0f;
					LedgeGrabCount++;
					TimeSinceLastGrab = 0;
					moveDirection.z = 0;
				}
			}
//
//controller air control
//

			if (ControllerXAxis > 7.2f)
			{
				moveDirection.z = moveDirection.z + (AirControl * Time.deltaTime);
			}

			if (ControllerXAxis < -7.2f)
			{
				moveDirection.z = moveDirection.z - (AirControl * Time.deltaTime);
			}
//
//keyboard air control
//
			if (Input.GetButton ("Right"))
			{

				moveDirection.z = moveDirection.z + (AirControl * Time.deltaTime);
			}
//speed cap
			if (moveDirection.z > Airspeed)
			{
				moveDirection.z = Airspeed;
			}
			if (Input.GetButton ("Left")) {
				moveDirection.z = moveDirection.z - (AirControl * Time.deltaTime);
			}
//negative speed cap
			if (moveDirection.z < -Airspeed)
			{
				moveDirection.z = -Airspeed;
			}
//air dodge calculations
			if ((Input.GetButton ("Dodge") || Input.GetAxis("Dodge")*10 > 2) && !HasAirDodged)
			{
				if(Input.GetButton ("Right") || ControllerXAxis > 5.0f)
				{
					moveDirection.z = 2*AirDodgeSpeed;
					moveDirection.y = -AirDodgeSpeed;
					HasAirDodged = true;
				} 
				else if (Input.GetButton ("Left") || ControllerXAxis < -5.0f)
				{
					moveDirection.z = -2*AirDodgeSpeed;
					moveDirection.y = -AirDodgeSpeed;
					HasAirDodged = true;
				}
				else
				{
					moveDirection.z = 0;
					moveDirection.y = 0;
					HasAirDodged = true;
				}
			}
		}

//jump check for grounded
//TODO():move all this to the other is grounded if else?
//cluter?
		if (controller.isGrounded) 
		{
			if (Input.GetKeyDown("s") || DeltaYAxis > 3.0f)
			{
				this.gameObject.layer = 1;
				GameObject plat = GameObject.Find("Platform");
				plat.layer = voidLayer;
				
				if (plat)
				{
					Physics.IgnoreLayerCollision(1,31,true);
				}
			}
			if (Input.GetButtonDown ("Jump")) 
			{
				anim.SetBool("Jump", true);
				jumpBufferTimer+=Time.deltaTime;
				jumpTimer += Time.deltaTime;
				startShorthopTimer = true;
				startJumpBufferTimer = true;
			}

			if (jumpBufferTimer > 0.050f)
			{
				moveDirection.y = jumpSpeed;
				jumpBufferTimer = 0;
				startJumpBufferTimer = false;
				anim.SetBool("Jump", false);
			}
//short hop released before you leave the ground 
			if (Input.GetButtonUp ("Jump"))
			{
				if (jumpTimer < 0.10)
				{
					//if (!HasAirDodged)
					//{
					moveDirection.y = jumpSpeed/2 + 4.5f;
					jumpTimer = 0;
					jumpBufferTimer = 0;
					startJumpBufferTimer = false;
					startShorthopTimer = false;
					//anim.Play ("Rising",0);
					anim.speed = 2;
					anim.SetBool("Jump", false);
					//}
				}
				
			}


//friction while grounded
			if (moveDirection.z > .750f)
			{
				moveDirection.z += friction * Time.deltaTime;
			}
			else if (moveDirection.z < -.750)
			{
				moveDirection.z -= friction * Time.deltaTime;
			}
			else 
			{
				moveDirection.z = 0;
			}
		} 
		else 
		{
			if(IsOnRightLedge == true)
			{
				HasDoubleJumped = false;
			}
			if (Input.GetButton("Jump"))
			{
				jumpTimer += Time.deltaTime;
			}
			if (Input.GetKeyUp("s"))
			{
				this.gameObject.layer = 2;
			}

//fast fall calculations
			if (Input.GetKeyDown("s") || DeltaYAxis > 3.0f)
			{
				if (IsOnRightLedge || IsOnLeftLedge)
				{
					IsOnLeftLedge = false;
					IsOnRightLedge = false;
					jumpSpeed = 22.0f;
					LedgeGrabCount++;
					moveDirection.z = 0;
					TimeSinceLastGrab = 0;
				}

				this.gameObject.layer = 1;
				GameObject plat = GameObject.Find("Platform");
				plat.layer = voidLayer;

				if (plat)
				{
					Physics.IgnoreLayerCollision(1,31,true);
				}

				if (HasFallen == false)
				{
					if (moveDirection.y < 0)
					{
						moveDirection.y = -.75f*jumpSpeed;
						HasFallen = true;
					}
				} 
			}
//
//double jump calculations
//
			if (Input.GetButtonUp ("Jump"))
			{
				if (jumpTimer < 0.115)
				{
					//if (!HasAirDodged)
					//{
					moveDirection.y = jumpSpeed/2 + 1;
					jumpTimer = 0;
					startShorthopTimer = false;
					//}
				}

			}
			if (Input.GetButtonDown("Jump"))
			{
				if (HasDoubleJumped == false)
				{
					if (!HasAirDodged){
						if (!IsOnLeftLedge || !IsOnRightLedge)
						{
							moveDirection.y = jumpSpeed;
							moveDirection.z = 0.25f*moveDirection.z;
							anim.speed = 1;
							anim.SetBool("DoubleJump",true);
							HasDoubleJumped = true;
							jumpTimer = 999;
						}
					}
				}
			}
		}
//
//max fall speed
//
		if (moveDirection.y < -17.0f && !HasFallen) 
		{
			moveDirection.y = -17.0f;
		} 
		else 
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}
//dodgetimer
		if (startDodgeBufferTimer) 
		{
			DodgeBufferTimer += Time.deltaTime;
		}
		//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, smooth * Time.deltaTime);

		if (movementBufferTimer) 
		{
			movementBuffer += Time.deltaTime;
		}

		if (!IsOnRightLedge && !controller.isGrounded) {
			TimeSinceLastGrab += Time.deltaTime;
		}
		else 
		{
			TimeSinceLastGrab = 0;
		}
		if (!IsOnLeftLedge && !controller.isGrounded) {
			TimeSinceLastGrab += Time.deltaTime;
		}

		if (IsOnRightLedge) {
			GameObject e = GameObject.Find ("Edge1");
			moveDirection.x = 0;
			moveDirection.y = 0;
			moveDirection.z = 0;
			controller.Move (moveDirection * Time.deltaTime);
			this.transform.position = e.transform.position;

		} 
		else if (IsOnLeftLedge) 
		{
			GameObject e2 = GameObject.Find ("Edge2");
			moveDirection.x = 0;
			moveDirection.y = 0;
			moveDirection.z = 0;
			controller.Move (moveDirection * Time.deltaTime);
			this.transform.position = e2.transform.position;
		}
		LastY = moveDirection.y;
		LastYAxis = ControllerYAxis;
		LastXAxis = ControllerXAxis;

		// move
		if (!inAttack) {
			controller.Move (moveDirection * Time.deltaTime);
		} else {
			if (moveDirection.z > .750f)
			{
				moveDirection.z += friction*1.3f * Time.deltaTime;
			}
			else if (moveDirection.z < -.750)
			{
				moveDirection.z -= friction*1.3f * Time.deltaTime;
			}
			else 
			{
				moveDirection.z = 0;
			}

			if (moveDirection.z > 2.0f)
			{
				moveDirection.z = 2.0f;
			} 
			else if (moveDirection.z < -2.0f)
			{
				moveDirection.z = -2.0f;
			}

			controller.Move (moveDirection * Time.deltaTime);
		}
		// move
	}
}
