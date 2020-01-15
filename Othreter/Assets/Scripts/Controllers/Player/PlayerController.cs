using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region Variables

	[Header("Scripts")]
	private Camera camMain;
	private Animator anim;
	[HideInInspector] public CharacterController controller;


	[HideInInspector] public Vector3 moveDirection;
	private Quaternion modelRotation;
	private Vector3 desiredRotation;
	private bool desiredRotated = false;
	private Vector3 colliderHitNormal;
	private Vector3 slideDirection;

	[Header("Jump")]

	[SerializeField] private float jumpHeight = 9.0f;
	private Vector3 jumpVelocity = Vector3.zero;
	private float jumpCooldown;

	[Header("Script Settings")]

	[SerializeField] private bool jumpEnabled = true;
	[SerializeField] private bool sprintEnabled = true;
	[SerializeField] private bool crouchEnabled = true;
	[SerializeField] private bool modelRotationEnabled = true;
	[SerializeField] private bool rigidbodyForceEnabled = true;
	[SerializeField] private bool slidingEnabled = true;
	[SerializeField] private bool WASDEnabled = true;

	private bool horizontalMovementOnlyEnabled = false;

	private bool sprintSpeedReached = false;
	private bool crouchSpeedReached = false;

	[Header("Speed")]
	public float speed = 6.0f;

	public float defaultSpeed = 6.0f;
	[SerializeField] private float sprintSpeed = 8.0f;
	[SerializeField] private float crouchSpeed = 4.0f;
	[SerializeField] private float maxSlideSpeed = 17.0f;
	private float slideSpeed = 0.0f;

	[Header("Physics")]
	[SerializeField] private float pushPower = 2.0f; //how many times multiply player force on rigidbody objects
	[SerializeField] private float gravity = 20.0f; //gravitation force (20f is optimal as for earth gravity)
	private Vector3 force;
	[SerializeField] private float angleLimitToSlide = 45.0f;

	[Header("Player Controller Size")]
	private float playerHeight; //default player height
	private Vector3 playerCenter;
	[SerializeField] private float crouchHeight = 1.5f;

	[Header("Layers")]
	[SerializeField] private LayerMask crouchWallLayer;

	[HideInInspector] public bool groundAngleOverLimit = false;
	private readonly float slopeLimitCheckDistance = 5.25f; //how low should ray go to check if player is coming down the slope
	private float groundAngle;

	#endregion

	#region Funtions

	private void Start()
	{
		controller = GetComponent<CharacterController>();
		camMain = ObjectsMenager.instance.cam;
		anim = GetComponent<Animator>();

		controller.skinWidth = 0.06f;//dont change
		playerHeight = controller.height;
		playerCenter = controller.center;

		speed = defaultSpeed - DataHolder.activeWeaponSpeedSub;

		modelRotation = transform.rotation;
	}

	private void Update()
	{
		#region Sprint

		if (sprintEnabled && DataHolder.playerState_Controllable)
		{
			if (((Input.GetKey(DataHolder.Sprint) && Input.GetKey(KeyCode.W)) || (Input.GetKey(DataHolder.SprintController) && Input.GetAxis("Horizontal") > 0) && sprintSpeedReached == false) && DataHolder.playerState_Crouch == false && DataHolder.playerState_Fallen == false && DataHolder.playerState_Aiming == false && DataHolder.playerState_Sliding == false)
			{
				DataHolder.playerState_Sprint = true;
				anim.SetBool("Sprint", true);
				speed = Mathf.Lerp(speed, sprintSpeed - DataHolder.activeWeaponSpeedSub, Time.deltaTime * 2.0f);
				if(Mathf.Abs((sprintSpeed - DataHolder.activeWeaponSpeedSub) - speed) < 0.25f)
				{
					speed = sprintSpeed - DataHolder.activeWeaponSpeedSub;
					sprintSpeedReached = true;
				}
			}
			else if (((Input.GetKey(DataHolder.Sprint) == false || Input.GetKey(KeyCode.W) == false) && (Input.GetKey(DataHolder.SprintController) == false || Input.GetAxis("Horizontal") <= 0) && sprintSpeedReached == true) && DataHolder.playerState_Fallen == false || (DataHolder.playerState_Aiming && DataHolder.playerState_Crouch == false) || DataHolder.playerState_Sliding)
			{
				DataHolder.playerState_Sprint = false;
				anim.SetBool("Sprint", false);

				speed = Mathf.Lerp(speed, defaultSpeed - DataHolder.activeWeaponSpeedSub, Time.deltaTime * 2.0f);
				if (Mathf.Abs(speed - (defaultSpeed - DataHolder.activeWeaponSpeedSub)) < 0.25f)
				{
					speed = defaultSpeed - DataHolder.activeWeaponSpeedSub;
					sprintSpeedReached = false;
				}
			}
			else
			{
				DataHolder.playerState_Sprint = false;
				anim.SetBool("Sprint", false);
			}
		}

		#endregion

		#region Crouch

		if (crouchEnabled && DataHolder.playerState_Controllable)
		{
			RaycastHit hitCrouch;

			if ((Input.GetKey(DataHolder.Crouch) || Input.GetKey(DataHolder.CrouchController) && crouchSpeedReached == false) && DataHolder.playerState_Grounded)
			{
				DataHolder.playerState_Crouch = true;
				anim.SetBool("Crouch", true);

				controller.height = crouchHeight;
				controller.center = new Vector3(0.0f, crouchHeight / 2 + controller.skinWidth, 0.0f);//moves player center point lover
				speed = crouchSpeed - DataHolder.activeWeaponSpeedSub;
				crouchSpeedReached = true;
			}
			else if ((Input.GetKey(DataHolder.Crouch) == false && Input.GetKey(DataHolder.CrouchController) == false && crouchSpeedReached == true) && DataHolder.playerState_Sprint == false && DataHolder.playerState_Fallen == false)
			{
				if (!Physics.Raycast(transform.position, Vector3.up, out hitCrouch, playerHeight + 0.1f, crouchWallLayer))
				{
					DataHolder.playerState_Crouch = false;
					anim.SetBool("Crouch", false);

					controller.height = playerHeight;
					controller.center = playerCenter;
					speed = Mathf.Lerp(speed, defaultSpeed - DataHolder.activeWeaponSpeedSub, Time.deltaTime * 2.0f);
					if (Mathf.Abs(speed - (defaultSpeed - DataHolder.activeWeaponSpeedSub)) < 0.25f)
					{
						speed = defaultSpeed - DataHolder.activeWeaponSpeedSub;
						crouchSpeedReached = false;
					}
				}
			}
			else
			{
				if (!Physics.Raycast(transform.position, Vector3.up, out hitCrouch, playerHeight + 0.1f, crouchWallLayer))
				{
					DataHolder.playerState_Crouch = false;
					anim.SetBool("Crouch", false);

					controller.height = playerHeight;
					controller.center = playerCenter;
				}
			}
		}

		#endregion

		#region Movement, Jump and Rotation

		if(WASDEnabled && DataHolder.playerState_Controllable)
		{
			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			Vector3 forward = camMain.transform.forward;
			Vector3 right = camMain.transform.right;

			forward.y = 0f;
			right.y = 0f;

			forward.Normalize();
			right.Normalize();

			if(horizontalMovementOnlyEnabled)
			{
				moveDirection = (forward * movementInput.y + right * movementInput.x) - (slideDirection / 2);
			}
			else
			{
				moveDirection = (forward * movementInput.y + right * movementInput.x);
			}
			

			float inputDeadzone = DataHolder.inputDeadzone;

			if (moveDirection.magnitude < inputDeadzone)
			{
				moveDirection = Vector2.zero;
			}
			else
			{
				moveDirection = moveDirection.normalized * ((moveDirection.magnitude - inputDeadzone) / (1 - inputDeadzone));
			}
		}

		//The two below has to be together
		moveDirection = Vector3.ClampMagnitude(moveDirection, 1);
		controller.Move(((Vector3.ClampMagnitude(new Vector3(moveDirection.x + jumpVelocity.x, 0, moveDirection.z + jumpVelocity.z), 1) * speed) + new Vector3(0, moveDirection.y + jumpVelocity.y, 0) + slideDirection) * Time.deltaTime);

		anim.SetFloat("InputVertical", Input.GetAxis("Vertical"));
		anim.SetFloat("InputHorizontal", Input.GetAxis("Horizontal"));
		anim.SetFloat("Velocity", controller.velocity.magnitude);
		anim.SetFloat("VelocityXZ", (controller.velocity.x * controller.velocity.x) + (controller.velocity.z * controller.velocity.z));
		if (controller.velocity != Vector3.zero)
		{
			DataHolder.playerState_Idle = false;
		}
		else
		{
			DataHolder.playerState_Idle = true;
		}

		if (controller.isGrounded)
		{
			DataHolder.playerState_Grounded = true;

			if ((Input.GetKeyDown(DataHolder.Jump) || Input.GetKeyDown(DataHolder.JumpController)) && groundAngleOverLimit == false && jumpCooldown <= 0.0f && DataHolder.playerState_Crouch == false && jumpEnabled && DataHolder.playerState_Controllable) //jump
			{
				jumpVelocity = moveDirection;
				jumpVelocity = Vector3.ClampMagnitude(jumpVelocity, 1);
				jumpVelocity.y = jumpHeight;

				DataHolder.playerState_Jump = true;
				anim.SetBool("Jump", true);
			}
		}
		else
		{
			DataHolder.playerState_Grounded = false;

			//in air after jump
			jumpVelocity.y -= gravity * Time.deltaTime;

			if (controller.velocity.y < 0) //falling
			{
				if (DataHolder.playerState_Jump)
				{
					anim.SetBool("JumpFalling", true);
					DataHolder.playerState_JumpFalling = true;
				}
			}
		}

		if (jumpCooldown >= 0.0f)
		{
			jumpCooldown -= Time.deltaTime;
		}

		#endregion

		#region Model Rotation

		if (modelRotationEnabled && DataHolder.playerState_Controllable)
		{
			if (controller.velocity.magnitude > 0.25f && DataHolder.playerState_Aiming == false && DataHolder.playerState_Jump == false) //rotation when moving
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(controller.velocity.x, 0.0f, controller.velocity.z)), 7.5f * Time.deltaTime);
				modelRotation = transform.rotation;
				desiredRotation = controller.velocity;
				desiredRotated = false;
			}

			else if ((controller.velocity == Vector3.zero || DataHolder.playerState_Jump) && DataHolder.playerState_Aiming == false && desiredRotation != Vector3.zero && modelRotation != Quaternion.identity) //rotation to desired point
			{
				if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(new Vector3(desiredRotation.x, 0.0f, desiredRotation.z))) < 0.1f)
				{
					desiredRotated = true;
				}
				if (desiredRotated == false)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(desiredRotation.x, 0.0f, desiredRotation.z)), 7.5f * Time.deltaTime);
					modelRotation = transform.rotation;
				}
				else
				{
					transform.rotation = modelRotation;
				}
			}
			else if (DataHolder.playerState_Aiming) //rotation when aiming, tmep, it will be in weapon script
			{
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0), 7.5f * Time.deltaTime);
				modelRotation = transform.rotation;
			}
		}

		#endregion

		#region Slope

		if(slidingEnabled && DataHolder.playerState_Controllable)
		{ 
			if (groundAngle > angleLimitToSlide)
			{
				groundAngleOverLimit = true;

				DataHolder.playerState_Sliding = true;

				jumpVelocity = Vector3.zero;

				Vector3 c = Vector3.Cross(colliderHitNormal, Vector3.up);
				slideDirection = -Vector3.Cross(c, colliderHitNormal);

				slideSpeed = Mathf.Lerp(slideSpeed, maxSlideSpeed, Mathf.Abs(groundAngle * 0.025f) * Time.deltaTime);
				if (Mathf.Abs(maxSlideSpeed - slideSpeed) <= 1.0f)
				{
					slideSpeed = maxSlideSpeed;
				}

				if(controller.velocity.y < 0)
				{
					horizontalMovementOnlyEnabled = true;
				}
				else
				{
					horizontalMovementOnlyEnabled = false;
				}

				slideDirection *= slideSpeed; //add sliding speed

				if (slideSpeed >= maxSlideSpeed * 0.15f)
				{
					anim.SetBool("Sliding", true);
				}
			}
			else
			{
				if (DataHolder.playerState_Sliding)
				{
					slideSpeed = 0;
					slideDirection = Vector3.zero;
					horizontalMovementOnlyEnabled = false;
					anim.SetBool("Sliding", false);
					DataHolder.playerState_Sliding = false;
				}

				groundAngleOverLimit = false;
			}
		}

		if (SlopeCheck() && DataHolder.playerState_Jump == false && DataHolder.playerState_Idle == false) //detects if player is moving on the slope and isnt jumping
		{
			moveDirection = new Vector3(moveDirection.x, moveDirection.y - (playerHeight * slopeLimitCheckDistance), moveDirection.z); //moves player to the slope
		}

		#endregion
	}

	private void LateUpdate()
	{
		if (((DataHolder.playerState_Jump && jumpVelocity.y < 0) || DataHolder.playerState_JumpFalling) && controller.isGrounded) //landing
		{
			jumpVelocity = Vector3.zero;
			jumpCooldown = 0.04f;

			anim.SetBool("JumpFalling", false);
			anim.SetBool("Jump", false);//temp

			DataHolder.playerState_Jump = false;

			DataHolder.playerState_JumpFalling = false;
		}
	}

	#region Character Controller can move Rigidbody

	private void OnControllerColliderHit(ControllerColliderHit hit) //Apply player push force on rigidbody objects
	{
		colliderHitNormal = hit.normal;
		if (rigidbodyForceEnabled)
		{
			Rigidbody body = hit.collider.attachedRigidbody;

			if (body == null || body.isKinematic) // If object has no rigidbody
			{
				return;
			}

			if (hit.moveDirection.y < -0.3) // if object is below us
			{
				body.AddForceAtPosition(new Vector3(0, -1.0f, 0) * pushPower, hit.point);
			}
			else
			{
				body.velocity = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z) * pushPower;// Calculate push direction from move direction
			}
		}
	}

	#endregion

	private bool SlopeCheck() //detects if player is on the slope
	{
		if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitSlope, controller.height / 2 * 1.5f))
		{
			groundAngle = Vector3.Angle(Vector3.up, hitSlope.normal); //checks ground angle
			if (hitSlope.normal != Vector3.up)
			{
				return true;
			}
		}
		return false;
	}

	public void RefreshSpeed()
	{
		speed = defaultSpeed - DataHolder.activeWeaponSpeedSub;
	}

	#endregion
}