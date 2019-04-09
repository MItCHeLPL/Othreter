using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region Variables

	[Header("Scripts")]
	private Camera camMain;
	private CapsuleCollider playerTriggerCollider;
	private Animator anim;
	private CharacterController controller; //adds character controller
	private CameraController cameraController; //allows to get variables form CameraMouse.cs
	private FallDamage fallDamage; //allows to use FallDamage script
	[HideInInspector]
	public GameObject playerModel;
	private WeaponSwitching weaponHolder;

	[HideInInspector]
	public Vector3 moveDirection; //player moves in this direction
	private Vector3 jumpVelocity = Vector3.zero;
	private Quaternion modelRotation;
	private Vector3 desiredRotation;
	private bool desiredRotated = false;

	private RaycastHit hitCrouch;

	[Header("Speed")]
	public float speed = 5.0f; //player speed
	[HideInInspector]
	public float oldSpeed; //holds default player speed
	public float sprintSpeed = 8.0f; //speed when sprinting
	public float airSpeed = 3.0f; // speed mid air
	public float crouchSpeed = 3.0f; // speed when crouching
	public float backSpeed = 4.0f; //speed when moving backwards
	private float jumpCooldown;

	[Header("Physics")]
	public float pushPower = 2.0f; //how many times multiply player force on rigidbody objects
	public float jumpHeight = 8.0f; //jump height
	public float gravity = 20.0f; //gravitation force (20f is optimal as for earth gravity)

	[Header("Camera FOV")]
	public float camFov = 60.0f; //normal camera field of view
	public float camSprintFov = 65.0f; //camera field of view while sprinting

	[Header("Player Controller Size")]
	public float playerHeight = 2.0f; //default player height
	public float crouchHeight = 1.5f; //player height when crouching
	public Vector3 playerCenter = new Vector3(0.0f, 1.0f, 0.0f); //default player model center
	public float playerRadius = 0.3f; //default player radius

	private readonly float slopeLimitCheckDistance = 5.25f; //how low should ray go to check if player is coming down the slope
	private float groundAngle; //angle of the ground player is standing on

	[Header("Climbing")]
	private float climbingCheckDistance = 0.5f; //how long climbing raycast is
	public float climbingSpeed = 2.0f; //how fast player is climbing
	private readonly float climbingJumpHeight = 7.0f; //how high player jumps from wall
	private bool climbingJumped = false; //if player canceled climbing

	[Header("Layers")]
	public LayerMask crouchWallLayer; //player colides with theese layers
	public LayerMask climbLayer; //player can climb when raycast hits this layer

	[HideInInspector]
	public bool climbingProcess = false; //determine if player is in climbing state form being off ground to standing on the ground again
	[HideInInspector]
	public bool groundAngleOverLimit = false; //detects if ground angle is over slope limit
	[HideInInspector]
	public bool crouch = false; //informs whether player crouches
	[HideInInspector]
	public bool jump = false; //informs wether player jumped
	[HideInInspector]
	public bool inFight = false; //informs wether player jumped

	[Header("Combat")]
	public float fightDistance = 200.0f;
	private GameObject closestEnemy = null;
	private float distanceToClosestEnemy;
	public bool swordAiming = false;

	#endregion

	#region Funtions

	private void Start()
	{
		controller = GetComponent<CharacterController>(); //sets character controller
		playerModel = ObjectsMenager.instance.playerModel;
		camMain = ObjectsMenager.instance.cam; //optimalization
		anim = playerModel.GetComponent<Animator>();
		cameraController = camMain.GetComponent<CameraController>();
		fallDamage = GetComponent<FallDamage>();
		playerTriggerCollider = GetComponent<CapsuleCollider>();
		weaponHolder = ObjectsMenager.instance.weaponHolder.GetComponent<WeaponSwitching>();

		playerTriggerCollider.isTrigger = true;
		playerTriggerCollider.radius = 0.4f;
		playerTriggerCollider.height = 2.4f;
		playerTriggerCollider.center = new Vector3(0.0f, 0.9f, 0.0f);

		playerCenter.y = playerCenter.y + +controller.skinWidth;
		controller.height = playerHeight;//this could be changed depending on player model
		controller.center = playerCenter; //this could be changed depending on player model
		controller.radius = playerRadius;//this could be changed depending on player model
		controller.stepOffset = 0.4f; //step offset
		controller.slopeLimit = 45.0f; //slope limit
		controller.skinWidth = 0.06f;//dont change
		controller.minMoveDistance = 0.0f;//dont change

		camMain.fieldOfView = camFov; //sets default main camera field of view

		oldSpeed = speed; //saves default speed

		climbingCheckDistance = playerRadius + 0.2f; //check for climbing distance just in front of player

		modelRotation = playerModel.transform.rotation;
	}

	private void Update()
	{ 
		//edit binds
		#region Sprint

		if (Input.GetKey(InputMenager.input.sprint) && Input.GetKey(KeyCode.W) && crouch == false && climbingProcess == false && jump == false && fallDamage.fallen == false && cameraController.aiming == false) //sprint works only if you push forward and sprint key, crouch key cant be pressed
		{
			camMain.fieldOfView = Mathf.Lerp(camMain.fieldOfView, camSprintFov, 10 * Time.deltaTime); //rises fov
			speed = sprintSpeed; //multiplies speed
			anim.SetBool("Sprint", true);
		}
		else if ((Input.GetKey(InputMenager.input.sprint) == false || Input.GetKey(KeyCode.W) == false || climbingProcess) && fallDamage.fallen == false || (cameraController.aiming == true && weaponHolder.ActiveWeaponTag() == "Bow"))
		{
			camMain.fieldOfView = Mathf.Lerp(camMain.fieldOfView, camFov, 10 * Time.deltaTime);
			speed = oldSpeed;//brings default speed back\
			anim.SetBool("Sprint", false);
		}

		#endregion

		//edit binds
		#region Crouch

		if (Input.GetKey(InputMenager.input.crouch) && climbingProcess == false)//crouch key
		{
			crouch = true;//crouch is on
			controller.height = crouchHeight; //reduces player height
			controller.center = new Vector3(0.0f, crouchHeight / 2 + controller.skinWidth, 0.0f);//moves player center point lover
			speed = crouchSpeed;//reduces player speed by sprint speed
			anim.SetBool("Crouch", true);
		}

		if (Input.GetKey(InputMenager.input.crouch) == false)
		{
			Debug.DrawRay(transform.position, Vector3.up, Color.red);//shows ray when in scene mode
			if (!Physics.Raycast(transform.position, Vector3.up, out hitCrouch, playerHeight + 0.1f, crouchWallLayer))
			{
				controller.height = playerHeight; //makes player hight default
				controller.center = playerCenter; //makes player center default
				crouch = false;//crouch is off
				anim.SetBool("Crouch", false);
				if (Input.GetKey(InputMenager.input.sprint) == false && fallDamage.fallen == false && cameraController.aiming == false) //set speed back to normal only if player is not sprinting
				{
					speed = oldSpeed;//sets player speed to default value
				}
			}
		}

		#endregion

		//edit, edit binds
		#region Movement, Jump and Rotation

		if (climbingProcess == false)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

			anim.SetFloat("InputVertical", Input.GetAxis("Vertical"));
			anim.SetFloat("InputHorizontal", Input.GetAxis("Horizontal"));
			anim.SetFloat("Velocity", controller.velocity.magnitude);

			#region Model Rotation

			if(Input.GetMouseButtonDown(1) && weaponHolder.ActiveWeaponTag() == "Sword")
			{
				FindEnemy(null, false);
				if(closestEnemy != null)
				{
					closestEnemy.GetComponent<EnemyUI>().lockIndicator.enabled = true;
				}
			}

			if (Input.GetMouseButtonUp(1) && weaponHolder.ActiveWeaponTag() == "Sword" && closestEnemy != null)
			{
				closestEnemy.GetComponent<EnemyUI>().lockIndicator.enabled = false;
			}

			transform.rotation = Quaternion.Euler(0.0f, cameraController.currentX, 0.0f);
			if (moveDirection != Vector3.zero && cameraController.aiming == false && jump == false) //player rotation
			{
				playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.LookRotation(moveDirection, Vector3.up), 7.5f * Time.deltaTime);
				modelRotation = playerModel.transform.rotation;
				desiredRotation = moveDirection;
				desiredRotated = false;
			}

			else if (jump == true && inFight == false && cameraController.aiming == false)
			{
				if (Quaternion.Angle(playerModel.transform.localRotation, Quaternion.LookRotation(desiredRotation, Vector3.up)) < 0.1f)
				{
					desiredRotated = true;
				}
				if (desiredRotated == false)
				{
					playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.LookRotation(desiredRotation, Vector3.up), 7.5f * Time.deltaTime);
					modelRotation = playerModel.transform.rotation;
				}
				else
				{
					playerModel.transform.rotation = modelRotation;
				}

				/*playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.identity, 7.5f * Time.deltaTime);
				modelRotation = playerModel.transform.rotation;*/
			}

			else if (moveDirection == Vector3.zero && jump == false && inFight == false && cameraController.aiming == false)
			{
				if(Quaternion.Angle(playerModel.transform.localRotation, Quaternion.LookRotation(desiredRotation, Vector3.up)) < 0.1f)
				{
					desiredRotated = true;
				}
				if(desiredRotated == false)
				{
					playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.LookRotation(desiredRotation, Vector3.up), 7.5f * Time.deltaTime);
					modelRotation = playerModel.transform.rotation;
				}
				else
				{
					playerModel.transform.rotation = modelRotation;
				}
			}

			else if (cameraController.aiming == true && weaponHolder.ActiveWeaponTag() == "Bow")
			{
				playerModel.transform.localRotation = Quaternion.Lerp(playerModel.transform.localRotation, Quaternion.identity, 7.5f * Time.deltaTime);
				modelRotation = playerModel.transform.rotation;
			}
			else if (cameraController.aiming == true && weaponHolder.ActiveWeaponTag() == "Sword" && inFight == true && closestEnemy != null)
			{
				swordAiming = true;
				playerModel.transform.rotation = Quaternion.Lerp(playerModel.transform.rotation, Quaternion.LookRotation(new Vector3(closestEnemy.transform.position.x, transform.position.y, closestEnemy.transform.position.z) - playerModel.transform.position), 7.5f * Time.deltaTime);
				modelRotation = playerModel.transform.rotation;

				if (Input.GetKeyDown(InputMenager.input.changeFocus))
				{
					closestEnemy.GetComponent<EnemyUI>().lockIndicator.enabled = false;
					FindEnemy(closestEnemy, true);
					closestEnemy.GetComponent<EnemyUI>().lockIndicator.enabled = true;
				}
			}
			else
			{
				swordAiming = false;
			}

			#endregion

			moveDirection = transform.TransformDirection(moveDirection);

			if (controller.isGrounded)//when player is on ground
			{
				moveDirection *= speed;

				if (Input.GetKeyDown(InputMenager.input.jump) && groundAngleOverLimit == false && jumpCooldown <= 0.0f)
				{
					jump = true; //enables jump

					anim.SetTrigger("Jump");

					jumpVelocity = moveDirection * 0.75f;
					jumpVelocity.y = jumpHeight;
				}
			}
			else if (controller.isGrounded == false && jump == true)
			{
				moveDirection *= airSpeed;
				jumpVelocity.y -= gravity * Time.deltaTime;
			}
			else if (controller.isGrounded == false && jump == false)
			{
				moveDirection *= speed;
				jumpVelocity.y -= gravity * Time.deltaTime;
			}
			else
			{
				jumpVelocity = Vector3.zero;
			}

			//test this
			if(weaponHolder.ActiveWeaponTag() == "Bow" || weaponHolder.ActiveWeaponTag() == "Sword")
			{
				speed -= 1.5f;
			}
		}
		else
		{
			jumpVelocity = Vector3.zero;
		}

		controller.Move((moveDirection + jumpVelocity) * Time.deltaTime);

		if (jumpCooldown >= 0.0f)
		{
			jumpCooldown -= Time.deltaTime;
		}

		#endregion

		//edit binds
		#region Slope

		SlopeCheck();

		if (groundAngle > controller.slopeLimit) //detects if player is on slope that he cant be on
		{
			groundAngleOverLimit = true;
		}

		else
		{
			groundAngleOverLimit = false;
		}

		/*if(colliderHitLayer == slideLayer) //if object has tag slide, player will slide off of it
		{
			moveDirection = new Vector3(colliderHitNormal.x, -colliderHitNormal.y, colliderHitNormal.z);
			Vector3.OrthoNormalize(ref colliderHitNormal, ref moveDirection);
			moveDirection *= slideSpeed; //add sliding speed
		}*/

		if (SlopeCheck() && jump == false && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)) //detects if player is moving on the slope and isnt jumping
		{
			controller.Move(Vector3.down * playerHeight / 2 * slopeLimitCheckDistance); //moves player to the slope
		}

		#endregion

		//edit, edit binds
		#region Climb
		/*
		RaycastHit hitClimb; //creates raycast
		RaycastHit hitLeft; //creates raycast2
		RaycastHit hitDown; //creates raycast3
		RaycastHit hitRight; //creates raycast3
		RaycastHit hitUp;

		Debug.DrawRay(transform.position + new Vector3(0.0f, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.forward), Color.blue);//shows ray when in scene mode

		if (Physics.Raycast(transform.position + new Vector3(0.0f, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.forward), out hitClimb, controller.radius + climbingCheckDistance, climbLayer) && climbingJumped == false && cameraController.aiming == false)
		{
			if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, 0.10f))//when starts climbing from ground
			{
				transform.Translate(Vector3.up * 0.11f);//moves you just above the raycast which checks if you are grounded while climbing
			}
			climbingProcess = true; //starts climbing
			moveDirection = new Vector3(0.0f, 0.0f, 0.0f); //sticks player to the wall

			anim.SetBool("Climbing", true);

			transform.rotation = Quaternion.LookRotation(-hitClimb.normal); //rotate player depending on normal of colider
			playerModel.transform.rotation = Quaternion.LookRotation(-hitClimb.normal);

			if (Input.GetKey(KeyCode.W) && !Physics.Raycast(transform.position + new Vector3(0.0f, controller.height, 0.0f), Vector3.up, out hitUp, 0.1f))
			{
				transform.Translate(Vector3.up * climbingSpeed * Time.deltaTime); //going upwards
			}

			if (Input.GetKey(KeyCode.S))
			{
				transform.Translate(Vector3.down * climbingSpeed * Time.deltaTime); //going downwards
			}

			if (Input.GetKey(KeyCode.A) && Physics.Raycast(transform.position + new Vector3(-controller.radius / 2, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.forward), out hitLeft, climbingCheckDistance + controller.radius, climbLayer) && !Physics.Raycast(transform.position + new Vector3(-controller.radius / 2, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.left), out hitRight, controller.radius + 0.2f))
			{
				transform.Translate(Vector3.left * climbingSpeed * Time.deltaTime); //going left
			}

			if (Input.GetKey(KeyCode.D) && Physics.Raycast(transform.position + new Vector3(controller.radius / 2, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.forward), out hitLeft, climbingCheckDistance + controller.radius, climbLayer) && !Physics.Raycast(transform.position + new Vector3(-controller.radius / 2, controller.height / 1.5f, 0.0f), transform.TransformDirection(Vector3.right), out hitRight, controller.radius + 0.2f))
			{
				transform.Translate(Vector3.right * climbingSpeed * Time.deltaTime); //going right
			}
		}

		if (Input.GetButtonDown("Jump") && climbingProcess)
		{
			climbingJumped = true; //player canceled climbing
			jumpVelocity.y = climbingJumpHeight;
			EndClimbing();

		}
		else if (Input.GetButtonDown("Jump") && climbingJumped)
		{
			climbingJumped = false; //allows to climb
		}

		if (hitClimb.collider == null && climbingProcess) //if player is on top
		{
			jumpVelocity.y = climbingJumpHeight;
			anim.SetTrigger("ClimbedOnTop");
			EndClimbing();
		}

		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, 0.10f) && climbingProcess) //when you are hitting floor and is climbing
		{
			EndClimbing();
			climbingJumped = true; //cant climb again until is grounded
		}

		if (controller.isGrounded) //allows to climb again
		{
			EndClimbing();
			climbingJumped = false; //resets 
		}*/

		#endregion
	}

	private void LateUpdate()
	{
		if (jump && controller.isGrounded) //allows player to jump again
		{
			jumpVelocity = Vector3.zero;
			jumpCooldown = 0.05f;
			jump = false; //disables jump after landing
		}
	}


	/*void OnAnimatorIK(int layerIndex) //enable when animations will be done
	{
		if (climbingProcess == false && moveDirection != Vector3.zero && cameraController.aiming == false)
		{
			anim.SetLookAtWeight(1f);
			anim.SetLookAtPosition(new Vector3(camMain.transform.eulerAngles.x, camMain.transform.eulerAngles.y, 0.0f));
		}
	}*/

	private void OnControllerColliderHit(ControllerColliderHit hit) //Apply player push force on rigidbody objects
	{
		#region Character Controller can move Rigidbody

		Rigidbody body = hit.collider.attachedRigidbody;

		if (body == null || body.isKinematic) // If object has no rigidbody
		{
			return;
		}

		if (hit.moveDirection.y < -0.3) // if object is below us
		{
			return;
		}

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);// Calculate push direction from move direction

		body.velocity = pushDir * pushPower;// Apply the push

		#endregion
	}

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

	private void EndClimbing()
	{
		anim.SetBool("Climbing", false);
		climbingProcess = false;
	}

	private IEnumerator Cooldown(float sec)
	{
		yield return new WaitForSeconds(sec);
	}

	#region Finding Enemy
	public void FindEnemy(GameObject enemyToSkip, bool changeEnemy)
	{
		StartCoroutine(FindClosestEnemy(enemyToSkip, changeEnemy));
		if (closestEnemy != null)
		{
			inFight = true;
			closestEnemy.GetComponent<EnemyUI>().lockIndicator.enabled = true;
		}
		else
		{
			inFight = false;
		}
	}

	private IEnumerator FindClosestEnemy(GameObject enemyToSkip, bool changeEnemy)
	{
		distanceToClosestEnemy = fightDistance;
		closestEnemy = null;
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject currentEnemy in allEnemies)
		{
			float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

			if (distanceToEnemy < distanceToClosestEnemy && currentEnemy != enemyToSkip)
			{
				distanceToClosestEnemy = distanceToEnemy;
				closestEnemy = currentEnemy;
			}
		}
		
		if(closestEnemy == null && changeEnemy == false)
		{
			inFight = false;
			if(weaponHolder.ActiveWeaponTag() == "Sword")
			{
				cameraController.CancelSwordAim();
			}
		}
		else if(closestEnemy == null && changeEnemy == true)
		{
			closestEnemy = enemyToSkip;
		}

		yield return closestEnemy;
	}
	#endregion

	#endregion
}