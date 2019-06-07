using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	#region Variables

	[Header("Scripts")]
	private PlayerController playerController; //allows to get variables form PlayerMovement.cs
	private Camera cam; //Main camera
	private GameObject playerObject; //Player model
	private WeaponSwitching weaponHolder;
	private Bow bow;
	private GameObject pauseMenu;
	private PerlinCameraShake camShake;

	[Header("Placement Settings")]
	[SerializeField]
	private float minYAngle = -89.0f; //camera Y angle limitations min
	[SerializeField]
	private float maxYAngle = 89.0f; //camera Y angle limitations amx
	[SerializeField]
	private float weaponDistance = 1.75f;
	private bool weaponPicked = false;
	[SerializeField]
	private bool changingDistanceEnabled = true;
	[SerializeField]
	private float maxDistance = 3.5f; //camera distance limitations max 
	[SerializeField]
	private float minDistance = 1.5f; //camera distance limitations min
	private float oldMinDistance;
	[SerializeField]
	private float distance = 2.0f; //camera placement distance
	[SerializeField]
	private float aimDistance = 1.0f; //camera placement distance
	private float oldDistance;
	private float oldDistanceBeforeWeapon;
	private Vector3 vectorDistance;
	[SerializeField]
	private float sensitivityX; //Camera sensitivity in x axis
	[SerializeField]
	private float sensitivityY; //Camera sensitivity in y axis
	[SerializeField]
	private Vector3 lookAtOffset = new Vector3(0.8f, 1.75f, 0.0f);
	private float lookAtXOffset;
	private float lookAtYOffset;
	private float lookAtZOffset;
	[SerializeField]
	private float WaitToShake = 3.0f;

	[Header("Input")]
	[HideInInspector]
	public float currentX; //camera placement in x axis
	[HideInInspector]
	public float currentY; //camera placement in y axis
	private Quaternion rotation;
	private bool inputDone = false;

	[Header("Layers")]
	[SerializeField]
	private LayerMask wallLayer; //camera colides with theese layers

	[Header("Indicators")]
	[HideInInspector]
	public bool aiming = false;
	private bool camOnPosition = true;
	private bool distanceOnPosition = true;
	private bool camShoulder = false; //switches camera beetween left and right shoulder right - false, left - true
	[HideInInspector]
	public bool weaponDistanceChangeDone = true;
	private float waitTimer;
	private bool colliding = false;

	[Header("GameObjects")]
	private GameObject right;  //right shoulder lookatpoint
	private GameObject left; //left shoulder lookatpoint
	private GameObject sright;  //right shoulder lookatpoint
	private GameObject sleft; //left shoulder lookatpoint
	private GameObject middle; //middle lookatpoint (in player models head)
	private GameObject middleAim;
	private GameObject leftAim; //left shoulder lookatpoint
	private GameObject rightAim; //left shoulder lookatpoint

	#endregion

	private void Start()
	{
		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>(); //allows to get variables from PlayerMovement.cs
		cam = ObjectsMenager.instance.cam;
		playerObject = ObjectsMenager.instance.player;
		weaponHolder = ObjectsMenager.instance.weaponHolder.GetComponent<WeaponSwitching>();
		bow = ObjectsMenager.instance.bow.GetComponent<Bow>();
		pauseMenu = ObjectsMenager.instance.pauseMenu;
		camShake = GetComponent<PerlinCameraShake>();

		cam.nearClipPlane = 0.04f; //optimal camera clipping when camera is coliding with wall etc.

		sensitivityX = InputMenager.input.mouseSensitivityX;
		sensitivityY = InputMenager.input.mouseSensitivityY;

		oldDistance = distance;
		oldDistanceBeforeWeapon = distance;

		#region LookAt Points Placements

		lookAtXOffset = lookAtOffset.x;
		lookAtYOffset = lookAtOffset.y;
		lookAtZOffset = lookAtOffset.z;

		//creates 3 gameobjects for camera to lookAt
		//to have camera look from right shoulder
		right = new GameObject("RightShoulder"); //creates GameObject
		right.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		right.transform.localPosition = new Vector3(lookAtXOffset, lookAtYOffset, lookAtZOffset); //moves point relative to player model

		//to have camera look from left shoulder
		left = new GameObject("LeftShoulder"); //creates GameObject
		left.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		left.transform.localPosition = new Vector3(-lookAtXOffset, lookAtYOffset, lookAtZOffset); //moves point relative to player model

		//to have camera look from right shoulder
		sright = new GameObject("MovingRightShoulder"); //creates GameObject
		sright.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		sright.transform.localPosition = new Vector3(lookAtXOffset, lookAtYOffset, lookAtZOffset); //moves point relative to player model

		//to have camera look from left shoulder
		sleft = new GameObject("MovingLeftShoulder"); //creates GameObject
		sleft.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		sleft.transform.localPosition = new Vector3(-lookAtXOffset, lookAtYOffset, lookAtZOffset); //moves point relative to player model


		//to have camera look at on freecam and fpp view, it is in players head
		middle = new GameObject("Middle"); //creates GameObject
		middle.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		middle.transform.localPosition = new Vector3(0.0f, 1.75f, 0.0f);//moves point relative to player model

		//to have camera look at on freecam and fpp view, it is in players head
		middleAim = new GameObject("MiddleAim"); //creates GameObject
		middleAim.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		middleAim.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);//moves point relative to player model

		//to have camera look from left shoulder
		leftAim = new GameObject("LeftAim"); //creates GameObject
		leftAim.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		leftAim.transform.localPosition = new Vector3(-0.6f, 1.75f, 0.0f); //moves point relative to player model

		//to have camera look from left shoulder
		rightAim = new GameObject("RightAim"); //creates GameObject
		rightAim.transform.parent = playerObject.transform;//sets gameobject as child of main player model
		rightAim.transform.localPosition = new Vector3(0.6f, 1.75f, 0.0f); //moves point relative to player model

		#endregion
	}

	private void Update()
	{
		//edit binds
		#region Input

		if(Input.anyKey)
		{
			inputDone = true;
		}

		if (camOnPosition == true && pauseMenu.activeInHierarchy == false)
		{
			currentX += Input.GetAxis("Mouse X") * sensitivityX * Time.timeScale; //multiplyes camera movement times sensitivityx
			currentY -= Input.GetAxis("Mouse Y") * sensitivityY * Time.timeScale; //multiplyes camera movement times sensitivityy  

			if(camShake._shakeJobRunning == false && aiming == false && distanceOnPosition == true && changingDistanceEnabled && weaponPicked == false)
			{
				if (Input.GetAxis("Mouse ScrollWheel") != 0)
				{
					distance += Input.GetAxis("Mouse ScrollWheel") * -1;//allows to change distance by scrolling
				}
				
				if (Input.GetKeyDown(InputMenager.input.zoomIn))
				{
					//distance -= 0.1f;
					StartCoroutine(SmoothDistance(-0.25f, 10.0f));
				}
				else if (Input.GetKeyDown(InputMenager.input.zoomOut))
				{
					//distance += 0.1f;
					StartCoroutine(SmoothDistance(0.25f, 10.0f));
				}
			}
		}

		if(pauseMenu.activeInHierarchy == true)
		{
			sensitivityX = InputMenager.input.mouseSensitivityX;
			sensitivityY = InputMenager.input.mouseSensitivityY;
		}

		#endregion

		#region Convert Inputs

		currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);//sets camera y angle between max and min limits
		distance = Mathf.Clamp(distance, minDistance, maxDistance);//sets camera distance between max and min limits
		if (distance == minDistance || distance == maxDistance)
		{
			distanceOnPosition = true;
		}
		vectorDistance = new Vector3(0.0f, 0.0f, -distance);//converts distance when using freecam to vector to add it to camera movement

		#endregion
	}

	private void LateUpdate()
	{
		//edit binds
		#region Crouch

		if (playerController.crouch)
		{
			middle.transform.position = new Vector3(middle.transform.position.x, playerObject.transform.position.y + 1.50f, middle.transform.position.z);//sets middle gameobjects position on eyesight hight while crouching
			rightAim.transform.position = new Vector3(rightAim.transform.position.x, playerObject.transform.position.y + 1.50f, rightAim.transform.position.z);//sets middle gameobjects position on eyesight hight while crouching
			leftAim.transform.position = new Vector3(leftAim.transform.position.x, playerObject.transform.position.y + 1.50f, leftAim.transform.position.z);//sets middle gameobjects position on eyesight hight while crouching
		}

		else
		{
			middle.transform.position = new Vector3(middle.transform.position.x, playerObject.transform.position.y + 1.75f, middle.transform.position.z); //sets middle gameobjects position on eyesight hight
			rightAim.transform.position = new Vector3(rightAim.transform.position.x, playerObject.transform.position.y + 1.75f, rightAim.transform.position.z);//sets middle gameobjects position on eyesight hight while crouching
			leftAim.transform.position = new Vector3(leftAim.transform.position.x, playerObject.transform.position.y + 1.75f, leftAim.transform.position.z);//sets middle gameobjects position on eyesight hight while crouching
		}

		#endregion

		//edit binds
		#region Shoulder
		if(camShake._shakeJobRunning == false && Input.GetKeyDown(InputMenager.input.switchShoulder) && camOnPosition == true && aiming == false && pauseMenu.activeInHierarchy == false)
		{
			if (playerController.moveDirection != Vector3.zero)//shoulder camera swap key
			{
				if (camShoulder == false)//right
				{
					StartCoroutine(Smooth(cam.gameObject, left.transform.position + rotation * vectorDistance, 15.0f));
					camShoulder = true;
				}
				else//left
				{
					StartCoroutine(Smooth(cam.gameObject, right.transform.position + rotation * vectorDistance, 15.0f));
					camShoulder = false;
				}
			}
			if (playerController.moveDirection == Vector3.zero)//shoulder camera swap key
			{
				if (camShoulder == false)//right
				{
					StartCoroutine(Smooth(cam.gameObject, sleft.transform.position + rotation * vectorDistance, 15.0f));
					camShoulder = true;
				}
				else//left
				{
					StartCoroutine(Smooth(cam.gameObject, sright.transform.position + rotation * vectorDistance, 15.0f));
					camShoulder = false;
				}
			}
		}
		

		//camera movement and lookat when using shoulder camera
		if (aiming == false) //free cam key
		{
			//do edycji
			if (playerController.moveDirection != Vector3.zero && playerController.jump == false)
			{
				right.transform.localPosition = new Vector3(lookAtXOffset, lookAtYOffset, lookAtZOffset);
				left.transform.localPosition = new Vector3(-lookAtXOffset, lookAtYOffset, lookAtZOffset);
				if (camShoulder == false)//right
				{
					CameraRotate(right);
				}
				else if (camShoulder == true)//left
				{
					CameraRotate(left);
				}
			}

			else if(playerController.moveDirection == Vector3.zero || playerController.jump == true)
			{
				Quaternion rotationR = Quaternion.Euler(0.0f, currentX, 0.0f);
				Quaternion rotationL = Quaternion.Euler(0.0f, currentX, 0.0f);

				sright.gameObject.transform.position = playerObject.transform.position + rotationR * new Vector3(lookAtXOffset, lookAtYOffset, lookAtZOffset);
				sleft.gameObject.transform.position = playerObject.transform.position + rotationL * new Vector3(-lookAtXOffset, lookAtYOffset, lookAtZOffset);

				if (camShoulder == false)//right
				{
					CameraRotate(sright);
				}
				else if (camShoulder == true)//left
				{
					CameraRotate(sleft);
				}
			}
		}

		#endregion

		if (weaponHolder.ActiveWeaponTag() != "Hands" && weaponDistanceChangeDone == false && aiming == false)
		{
			StartCoroutine(SmoothDistanceBeetweenValues(weaponDistance, 10.0f, true));
			weaponDistanceChangeDone = true;
			weaponPicked = true;
		}
		else if(weaponHolder.ActiveWeaponTag() == "Hands" && weaponDistanceChangeDone == false)
		{
			StartCoroutine(SmoothDistanceBeetweenValues(oldDistanceBeforeWeapon, 10.0f, false));
			weaponDistanceChangeDone = true;
			weaponPicked = false;
		}

		//edit binds
		#region Aim

			#region Aim Bow

			if (weaponHolder.ActiveWeaponTag() == "Bow" && camOnPosition == true && distanceOnPosition == true)
				{
					if (Input.GetKeyDown(InputMenager.input.switchShoulder) && aiming == true)//shoulder camera swap key
					{
						if (camShoulder == false)//right
						{
							StartCoroutine(Smooth(cam.gameObject, leftAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
							camShoulder = true;
						}
						else//left
						{
							StartCoroutine(Smooth(cam.gameObject, rightAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
							camShoulder = false;
						}
					}

					if (Input.GetMouseButtonDown(1) && playerController.climbingProcess == false)
					{
						//bow.FadeAmmoOut();

						oldMinDistance = minDistance;
						oldDistance = distance;

						minDistance = aimDistance;
						distance = aimDistance;

						if (camShoulder == false)//right
						{
							aiming = true;

							StartCoroutine(Smooth(cam.gameObject, rightAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
						}
						else//left
						{
							aiming = true;

							StartCoroutine(Smooth(cam.gameObject, leftAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
						}
					}

					if (Input.GetMouseButton(1) && playerController.climbingProcess == false && aiming == true)
					{
						if (camShoulder == false)//right
						{
							CameraRotate(rightAim);
						}
						else if (camShoulder == true)//left
						{
							CameraRotate(leftAim);
						}
					}

					if (playerController.climbingProcess == false && aiming == true && (Input.GetMouseButtonUp(1) || Input.GetMouseButton(1) == false))
					{
						if (camShoulder == false)//right
						{
							StartCoroutine(Smooth(cam.gameObject, right.transform.position + rotation * new Vector3(0.0f, 0.0f, -oldDistance), 15.0f));
						}
						else//left
						{
							StartCoroutine(Smooth(cam.gameObject, left.transform.position + rotation * new Vector3(0.0f, 0.0f, -oldDistance), 15.0f));
						}
						distance = oldDistance;
						minDistance = oldMinDistance;
						aiming = false;
					}
				}

				#endregion

			
			#region Aim Sword

				else if (weaponHolder.ActiveWeaponTag() == "Sword" && playerController.inFight == true && camOnPosition == true && distanceOnPosition == true)
				{
					/*if (Input.GetKeyDown(InputMenager.key.switchShoulder) && camOnPosition == true && aiming == true)//shoulder camera swap key
					{
						if (camShoulder == false)//right
						{
							StartCoroutine(Smooth(cam.gameObject, leftAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
							camShoulder = true;
						}
						else//left
						{
							StartCoroutine(Smooth(cam.gameObject, rightAim.transform.position + rotation * new Vector3(0.0f, 0.0f, -aimDistance), 15.0f));
							camShoulder = false;
						}
					}*/

					if (Input.GetMouseButtonDown(1) && playerController.climbingProcess == false)
					{
						/*oldMinDistance = minDistance;
						oldDistance = distance;

						minDistance = aimDistance;
						distance = aimDistance;*/

						aiming = true;

						StartCoroutine(Smooth(cam.gameObject, middleAim.transform.position + rotation * vectorDistance, 15.0f));
					}		

					if (Input.GetMouseButton(1) && playerController.climbingProcess == false && aiming == true)
					{
						CameraRotate(middleAim);
					}

					if (playerController.climbingProcess == false && aiming == true && (Input.GetMouseButtonUp(1) || Input.GetMouseButton(1) == false))
					{
						CancelSwordAim();
					}
				}

				#endregion

		#endregion

		#region Camera Colision

		RaycastHit hit; //creates raycast
		Vector3 start = middle.transform.position; //raycast from player position
		Vector3 direction = cam.transform.position - middle.transform.position;//sets raycast direction between player and camera
		if (Physics.Raycast(middle.transform.position, direction, out hit, distance, wallLayer) && camOnPosition == true)
		{
			Debug.DrawRay(middle.transform.position, direction, Color.green);//shows ray when in scene mode
			Vector3 sphereCastCenter = middle.transform.position + (direction.normalized * hit.distance * 0.75f); //dont change hit.distance multiplier, this line makes camera collision seamless when camera is looking at left, right or middle gameobjects 
			cam.transform.position = sphereCastCenter; //moves camera to point where raycast hits the wall
			colliding = true;
		}
		else
		{
			colliding = false;
		}

		#endregion
	}

	private void CameraRotate(GameObject lookatpoint) //calculates camera position by adding distance, rotation and player position
	{
		rotation = Quaternion.Euler(currentY, currentX, cam.gameObject.transform.rotation.z); //calculates rotation

		if (camOnPosition == true && (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Mouse ScrollWheel") != 0 || inputDone == false || colliding == true || playerController.controller.velocity != Vector3.zero))
		{
			cam.gameObject.transform.position = lookatpoint.transform.position + rotation * vectorDistance; //camera position
			cam.gameObject.transform.LookAt(lookatpoint.transform.position);//camera lookAt set at lookat point
			waitTimer = WaitToShake;
			if(camShake._trauma > 0)
			{
				camShake._trauma = 0;
			}
		}
		/*else if (camOnPosition == true) //old setting for backup while testing nev wersion
		{
			cam.gameObject.transform.LookAt(lookatpoint.transform.position);//camera lookAt set at lookat point
			if(waitTimer > 0)
			{
				waitTimer -= Time.deltaTime;
			}
			else if(waitTimer <= 0)
			{
				camShake._trauma = Mathf.Clamp01(camShake._trauma + (Time.deltaTime / 2));
			}
		}*/
		else if (camOnPosition == true)
		{
			if (waitTimer > 0)
			{
				waitTimer -= Time.deltaTime;
				cam.gameObject.transform.LookAt(lookatpoint.transform.position);//camera lookAt set at lookat point
				cam.gameObject.transform.position = lookatpoint.transform.position + rotation * vectorDistance; //camera position
			}
			else if (waitTimer <= 0)
			{
				camShake._trauma = Mathf.Clamp01(camShake._trauma + (Time.deltaTime / 2));
				cam.gameObject.transform.LookAt(lookatpoint.transform.position);//camera lookAt set at lookat point
			}
		}
	}

	public void CancelSwordAim()
	{
		if (camShoulder == false)//right
		{
			StartCoroutine(Smooth(cam.gameObject, right.transform.position + rotation * vectorDistance, 15.0f));
		}
		else//left
		{
			StartCoroutine(Smooth(cam.gameObject, left.transform.position + rotation * vectorDistance, 15.0f));
		}
		/*distance = oldDistance;
		minDistance = oldMinDistance;*/
		aiming = false;
	}

	private IEnumerator Smooth(GameObject gameObject, Vector3 endPoint, float speed)
	{
		camOnPosition = false;

		while (camOnPosition == false)
		{
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, endPoint, speed * Time.deltaTime);
			if (Vector3.Distance(gameObject.transform.position, endPoint) < 0.1f)
			{
				camOnPosition = true;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator SmoothDistance(float value, float speed)
	{
		distanceOnPosition = false;
		float endPoint = distance + value;

		while (distanceOnPosition == false)
		{
			distance = Mathf.Lerp(distance, endPoint, speed * Time.deltaTime);
			if (Mathf.Abs(distance - endPoint) < 0.01f)
			{
				distance = endPoint;
				distanceOnPosition = true;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator SmoothDistanceBeetweenValues(float value, float speed, bool saveOldDistance)
	{
		distanceOnPosition = false;
		if(saveOldDistance)
		{
			oldDistanceBeforeWeapon = distance;
		}
		while (distanceOnPosition == false)
		{
			distance = Mathf.Lerp(distance, value, speed * Time.deltaTime);
			if (Mathf.Abs(distance - value) < 0.01f)
			{
				distance = value;
				distanceOnPosition = true;
			}
			yield return new WaitForEndOfFrame();
		}
	}
}