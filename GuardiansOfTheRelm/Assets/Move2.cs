using UnityEngine;
using System.Collections;

public class Move2 : MonoBehaviour {

	private Animator anim = null;
	CharacterController controller;

	public float jumpSpeed = 17.0F;
	public float gravity = 50.0F;
	public float RunSpeed = 8.0f;
	public float WalkSpeed = 7.5f;
	public float friction = -40.0f;
	public float acceleration = 10.0f;
	public float Airspeed = 10.5f;
	public float AirControl = 24.0f;
	public float AirDodgeSpeed = 8.0f;
	public float smooth = 5f;

	bool HasFallen = false;
	public bool HasDoubleJumped = false;
	bool FacingRight = true;



	private Vector3 moveDirection = Vector3.zero;
	private Vector3 userInput = Vector3.zero;
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
	
	bool inGroundAttack = false;
	
	public float jumpBufferTimer = 0;
	bool startJumpBufferTimer = false;
	
	public float DodgeBufferTimer = 0;
	bool startDodgeBufferTimer = false;
	
	public float movementBuffer = 0;
	bool movementBufferTimer = false;

	private float jumpTimer = 0;
	bool startShorthopTimer = false;

	public float combatTimer = 0;
	bool startCombatTimer = false;

	public float TimerTest = 0;
	bool TimerTestState = false;

	public float smashBuffer = 0;
	bool startSmashBuffer = false;

	void resetBools(string name)
	{

	}

	void setTimers ()
	{
		if (startSmashBuffer) 
		{
			smashBuffer += Time.deltaTime;
		} 
		else 
		{
			smashBuffer = 0;
		}

		if (startJumpBufferTimer) 
		{
			jumpBufferTimer+=Time.deltaTime;
		}
		if (startShorthopTimer) 
		{
			jumpTimer+=Time.deltaTime;
		}
		if (startCombatTimer) 
		{
			combatTimer += Time.deltaTime;
		} 
		else 
		{
			combatTimer = 0;
		}

		if (startDodgeBufferTimer) 
		{
			DodgeBufferTimer += Time.deltaTime;
		}

		if (movementBufferTimer) 
		{
			movementBuffer += Time.deltaTime;
		}

		if (!IsOnRightLedge && !controller.isGrounded) 
		{
			TimeSinceLastGrab += Time.deltaTime;
		}
		else 
		{
			TimeSinceLastGrab = 0;
		}

		if (!IsOnLeftLedge && !controller.isGrounded) 
		{
			TimeSinceLastGrab += Time.deltaTime;
		}
		else 
		{
			TimeSinceLastGrab = 0;
		}

	}

	// Use this for initialization
	void Start () {
		gameObject.SetActive (true);
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator> ();
		voidLayer = 31;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton ("Submit")) {
			anim.SetTrigger("movetest");
		}

		if (smashBuffer > 0.1) 
		{
			startSmashBuffer = false;
		}

		userInput = Vector3.zero;
		ControllerXAxis = Input.GetAxis ("Mouse X") * 100;
		ControllerYAxis = Input.GetAxis ("Mouse Y") * 100;

		if (controller.isGrounded) {
			moveDirection.y = 0;
			DeltaY = LastY - 0;
			anim.SetBool ("Falling", false);
		}

		if (inGroundAttack) 
		{
			if (anim.GetBool("FSmash"))
			{
				if (combatTimer > 0.60f)
				{
			
					anim.SetBool("FSmash",false);
					startCombatTimer = false;
					inGroundAttack = false;
				}
			} 
			else if (anim.GetBool("Dash Attack"))
			{
				if (combatTimer > 0.60f)
				{
				
					anim.SetBool("Dash Attack",false);
					startCombatTimer = false;
					inGroundAttack = false;
				}
			} 
			else if (anim.GetBool("DSmash"))
			{
				if (combatTimer > 0.77f)
				{
			
					anim.SetBool("DSmash",false);
					startCombatTimer = false;
					inGroundAttack = false;
				}
			} 
			else if (anim.GetBool("USmash"))
			{
				if (combatTimer > 0.80f)
				{
				
					anim.SetBool("USmash",false);
					startCombatTimer = false;
					inGroundAttack = false;
				}
			}
		}

		DeltaYAxis = ControllerYAxis - LastYAxis;
		DeltaXAxis = ControllerXAxis - LastXAxis;

		if (DeltaYAxis < -4.0f) {
		
			startSmashBuffer = true;
		}

		if (controller.isGrounded) 
		{
			anim.speed = 1;
			anim.SetBool ("Falling", false);
			moveDirection.y = 0;
			if (DeltaY < -2.0f)
			{
				anim.Play ("Idle 0",0);
				anim.SetBool("Landing",true);
				jumpTimer = 0;
			} 
			else
			{
				anim.SetBool("Landing",false);
			}

			//resets
			HasAirDodged = false;
			IsOnLeftLedge = false;
			IsOnRightLedge = false;
			jumpSpeed = 22.0f;
			HasDoubleJumped = false;
			HasFallen = false;
			LedgeGrabCount = 0;
			this.gameObject.layer = 2;
			if (jumpTimer > .2)
			{
				jumpTimer = 0;
				anim.speed = 1;
				startShorthopTimer = false;
			}
			//

			//crouching
			if (ControllerYAxis > 8.0f)
			{
				anim.SetBool("Crouching",true);
				crouching = true;
				if (Input.GetButton("A"))
				{
					if (!inGroundAttack)
					{
						anim.SetBool("DSmash",true);
				   		startCombatTimer = true;
						inGroundAttack = true;
					}
				}
			} 
			else 
			{
				crouching = false;
				anim.SetBool("Crouching", false);
			}

			if (crouching)
			{
				friction = -80.0f;
			} 
			else
			{
				friction = -40.0f;
			}
			//

			if (DeltaYAxis > 4.0f)
			{
				startSmashBuffer = true;
			}

			//up smash.tilt
			if (ControllerYAxis < -8.0f)
			{
				if (Input.GetButton("A"))
				{
					if (smashBuffer < 0.08)
					{
						if (!inGroundAttack)
						{
							anim.SetBool("USmash",true);
							inGroundAttack = true;
							startCombatTimer = true;
						}
					}
				}
			}
			//

			//deadzone idle resets
			if (ControllerXAxis < 1.8f && ControllerXAxis > -1.8f)
			{
				anim.SetBool("Running",false);
				running = false;
				walking = false;
				movementBufferTimer = false;
				movementBuffer = 0;
			}
			//

			//right movement
			if (ControllerXAxis > 8.2f)
			{
				walking = false;
				running = true;
				anim.SetBool("Running",true);
				movementBufferTimer = true;

				if (moveDirection.z < 0)
				{
					userInput.z = .05f*acceleration;
				}
				else
				{
					userInput.z = acceleration;
				}
				if (!FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						if (!inGroundAttack)
						{
							transform.Rotate(0,180,0,Space.World);
							FacingRight = true;
						}
					}
				}

				//fsmash
				if (Input.GetButton("A"))
				{
					if (movementBuffer < 0.1)
					{
						if (!inGroundAttack)
						{
							anim.SetBool("FSmash",true);
							startCombatTimer = true;
							inGroundAttack = true;
							movementBuffer = 0;
						}
					}
					else
					{
						if (!inGroundAttack)
						{
							anim.SetBool("Dash Attack",true);
							startCombatTimer = true;
							inGroundAttack = true;
						}
					}
				}
				//
			} 
			else if (ControllerXAxis < 7.2f && ControllerXAxis > 3.0f)
			{
				running = false;
				walking = true;
				anim.SetBool("Running",false);
				
				if (moveDirection.z < 0)
				{
					userInput.z = .05f*acceleration;
				}
				else 
				{
					userInput.z = 0.5f*acceleration;
				}
				if (!FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						if (!inGroundAttack)
						{
							transform.Rotate(0,180,0,Space.World);
							FacingRight = true;
						}
					}
				}


			}
			//

			//movement left
			if (ControllerXAxis < -8.2f)
			{
				walking = false;
				running = true;
				anim.SetBool("Running",true);
				movementBufferTimer = true;
				
				if (moveDirection.z > 0)
				{
					userInput.z = (-.05f)*acceleration;
				}
				else
				{
					userInput.z = -acceleration;
				}
				if (FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						if (!inGroundAttack)
						{
							transform.Rotate(0,180,0,Space.World);
							FacingRight = false;
						}
					}
				}

				//fsmash
				if (Input.GetButton("A"))
				{
					if (movementBuffer < 0.1)
					{
						if (!inGroundAttack)
						{
							anim.SetBool("FSmash",true);
							startCombatTimer = true;
							inGroundAttack = true;
							movementBuffer = 0;
						}
					}
					else
					{
						if (!inGroundAttack)
						{
							anim.SetBool("Dash Attack",true);
							startCombatTimer = true;
							inGroundAttack = true;
						}
					}
				}
				//
			} 

			else if (ControllerXAxis > -7.2f && ControllerXAxis < -3.0f)
			{
				running = false;
				walking = true;
				anim.SetBool("Running",false);
				
				if (moveDirection.z < 0)
				{
					userInput.z = -.05f*acceleration;
				}
				else 
				{
					userInput.z = -0.5f*acceleration;
				}
				if (FacingRight)
				{
					if (jumpBufferTimer < 0.02)
					{
						if (!inGroundAttack)
						{
							transform.Rotate(0,180,0,Space.World);
							FacingRight = false;
						}
					}
				} 
			}
			//

			//speed capping
			if (moveDirection.z > RunSpeed && running)
			{
				moveDirection.z = RunSpeed;
			} 
			else if (moveDirection.z > WalkSpeed && walking)
			{
				moveDirection.z = WalkSpeed;
			} 
			else if (moveDirection.z < -RunSpeed && running)
			{
				moveDirection.z = -RunSpeed;
			}
			else if (moveDirection.z < -WalkSpeed && walking)
			{
				moveDirection.z = -WalkSpeed;
			} 
			//

			//jump initialization
			if (Input.GetButtonDown ("Jump")) 
			{
				if (!inGroundAttack)
				{
					print ("wtf");
					anim.SetBool("Jump", true);
					jumpBufferTimer+=Time.deltaTime;
					jumpTimer += Time.deltaTime;
					startShorthopTimer = true;
					startJumpBufferTimer = true;
				}
			}
			if (jumpBufferTimer > 0.050f)
			{
				moveDirection.y = jumpSpeed;
				jumpBufferTimer = 0;
				startJumpBufferTimer = false;
				anim.SetBool("Jump", false);
			}

			if (Input.GetButtonUp ("Jump"))
			{
				if (!inGroundAttack)
				{
					anim.SetBool ("Jump",false);
					startJumpBufferTimer = false;
					if (jumpTimer < 0.10 && jumpTimer > 0)
					{
						moveDirection.y = jumpSpeed/2 + 4.5f;
						jumpTimer = 0;
						jumpBufferTimer = 0;
						startJumpBufferTimer = false;
						startShorthopTimer = false;
						anim.speed = 2;

					}
				}
			}

			//FRICTION
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
			//

		//end if grounded
		} 
		else 
		{
			//ledge detection
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
					//anim.SetBool("DoubleJump",false);
					jumpSpeed = 22.0f;
					LedgeGrabCount++;
					TimeSinceLastGrab = 0;
					moveDirection.z = 0;
				}
			}
			//

			//air control
			if (ControllerXAxis > 7.2f)
			{
				userInput.z = (AirControl * Time.deltaTime);
			}

			if (ControllerXAxis < -7.2f)
			{
				userInput.z = -(AirControl * Time.deltaTime);
			}
			if (moveDirection.z > Airspeed)
			{
				moveDirection.z = Airspeed;
			}
			if (moveDirection.z < -Airspeed)
			{
				moveDirection.z = -Airspeed;
			}
			//

			//air dodge code
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
			//

			//ledge housekeeping
			if(IsOnRightLedge == true || IsOnLeftLedge == true)
			{
				HasDoubleJumped = false;
			}

			if (Input.GetKeyUp("s"))
			{
				this.gameObject.layer = 2;
			}
			//

			//fast fall calc
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

			if (Input.GetButtonUp ("Jump"))
			{
				anim.SetBool ("Jump",false);
				startJumpBufferTimer = false;
				startShorthopTimer = false;
				if (jumpTimer < 0.115 && jumpTimer > 0)
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
				if (!HasDoubleJumped)
				{
					if (!HasAirDodged){
						if (!IsOnLeftLedge || !IsOnRightLedge)
						{
							moveDirection.y = 1.25f*jumpSpeed;
							moveDirection.z = 0.25f*moveDirection.z;
							anim.speed = 1;
							if (ControllerXAxis < -3.0f)
							{
								if (FacingRight)
								{
									anim.SetBool("DoubleJumpBack",true);
								}
								else
								{
									anim.SetBool("DoubleJumpForeward",true);
								}
							}
							else if (ControllerXAxis > 3.0f)
							{
								if (FacingRight)
								{
									anim.SetBool("DoubleJumpForeward",true);
								}
								else
								{
									anim.SetBool("DoubleJumpBack",true);
								}
							}
							else 
							{
								if (FacingRight)
								{
									anim.SetBool("DoubleJumpForeward",true);
								}
								else
								{
									anim.SetBool("DoubleJumpBack",true);
								}
							}

							HasDoubleJumped = true;
							jumpTimer = 0;
						}
					}
				}
			}
			//

			if (moveDirection.y < -0.5)
			{
				anim.SetBool("Falling",true);
				anim.SetBool("DoubleJumpForeward",false);
				anim.SetBool("DoubleJumpBack",false);
			}

		//end is grounded if/else
		}

		//fall speed 
		if (moveDirection.y < -17.0f && !HasFallen) 
		{
			moveDirection.y = -17.0f;
		} 
		else 
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}
		//

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

		if (inGroundAttack) 
		{
			userInput = Vector3.zero;
		}
		moveDirection += userInput;
		controller.Move (moveDirection * Time.deltaTime);

		setTimers ();
		LastY = moveDirection.y;
		LastYAxis = ControllerYAxis;
		LastXAxis = ControllerXAxis;

		if (jumpTimer != 0) 
		{
			print (jumpTimer);
		}

	}
}
