using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8.0f; //player speed
    private float oldspeed; //holds default player speed
	public float sprintspeed = 10f; //speed when sprinting
    public float airspeed = 5.0f; // speed mid air
    public float crouchspeed = 5.0f; // speed when crouching
    public float backspeed = 5.0f; //speed when moving backwards

	public float pushPower = 2.0f; //how many times multiply player force on rigidbody objects

    public float jumpHeight = 8.0f; //jump height

    public float gravity = 20.0f; //gravitation force (20f is optimal as for earth gravity)

    public float camfov = 60.0f; //normal camera field of view
    public float camsprintfov = 65.0f; //camera field of view while sprinting

    public float playerHeight = 2.0f; //default player height
    public Vector3 playerCenter = new Vector3(0.0f, 1.0f, 0.0f); //default player model center
    public float playerRadius = 0.5f; //default player radius

    public float crouchheight = 1.5f; //player height when crouching

    public float slopelimitcheckdistance = 5.25f; //how low should ray go to check if player is coming down the slope
    public float slideSpeed = 2.0f; // ajusting how fast player is sliding when slope is > than slopeLimit

    public LayerMask wallLayer; //player colides with theese layers

    private Vector3 moveDirection; //player moves in this direction

    private CharacterController controller; //adds character controller

    private CameraMouse cameraMouse; //allows to get variables form CameraMouse.cs

    private float groundAngle; //angle of the ground player is standing on

    private Vector3 hitNormal; //hit normal to use in update
	
    [HideInInspector] //variables under this wont be visible in unity inspector, but they have to be public, becouse other scripts use them
    public bool crouch = false; //informs whether player crouches

    [HideInInspector] //variables under this wont be visible in unity inspector, but they have to be public, becouse other scripts use them
    public bool jump = false; //informs wether player jumped

    void Start()
    {
        //allows to get variables from CamerMouse.cs
        cameraMouse = GameObject.Find("MainCamera").GetComponent<CameraMouse>();

        controller = GetComponent<CharacterController>(); //sets character controller

        //optimal character controller settings for this project
		controller.height = playerHeight;//this could be changed depending on player model
        controller.center = playerCenter; //this could be changed depending on player model
		controller.radius = playerRadius;//this could be changed depending on player model
		controller.stepOffset = 0.4f;
		controller.slopeLimit = 35.0f;
        controller.skinWidth = 0.06f;//dont change
        controller.minMoveDistance = 0.0f;//dont change

        Camera.main.fieldOfView = camfov; //sets default main camera field of view

		oldspeed = speed; //saves default speed
    }

    void Update()
    {
		//Sprint
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift)==false) //sprint works only if you push forward and sprint key, crouch key cant be pressed
		{
            Camera.main.fieldOfView = camsprintfov; //rises fov
			speed = sprintspeed; //multiplies speed
		}
        else if(Input.GetKey(KeyCode.LeftControl)==false || Input.GetKey(KeyCode.W) == false)
        {
            Camera.main.fieldOfView = camfov;//brings default fov back
            speed = oldspeed;//brings default speed back
        }

		//Crouch
		if(Input.GetKey(KeyCode.LeftShift))//crouch key
		{
            crouch=true;//crouch is on
            controller.height = crouchheight; //reduces player height
            controller.center = new Vector3(0.0f, crouchheight / 2, 0.0f);//moves player center point lover
            speed = crouchspeed;//reduces player speed by sprint speed
		}
        if (Input.GetKey(KeyCode.LeftShift) == false)
        {
            RaycastHit hit;//creates raycast
            Debug.DrawRay(transform.position, Vector3.up, Color.red);//shows ray when in scene mode
            if (!Physics.Raycast(transform.position, Vector3.up, out hit, playerHeight + 0.1f, wallLayer))
            {
                controller.height = playerHeight; //makes player hight default
                controller.center = playerCenter; //makes player center default
                crouch = false;//crouch is off
                if(Input.GetKey(KeyCode.LeftControl) == false) //set speed back to normal only if player is not sprinting
                {
                    speed = oldspeed;//sets player speed to default value
                }
            }
        }

        if (controller.isGrounded)//when player is on ground
        {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));//movement on ground
            moveDirection = transform.TransformDirection(moveDirection) * speed; //apply speed

            if (Input.GetButtonDown("Jump"))//Jump
            {
                jump = true; //enables jump
                moveDirection.y = jumpHeight;//apply jump to movement
            }
       	}   

	   	else if (controller.isGrounded == false) //player movement mid-air
     	{
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.z = Input.GetAxis("Vertical");

			moveDirection.x *= airspeed; //apply mid-air speed
            if (moveDirection.z > 0.0f)
            {
                moveDirection.z *= (speed); //when player is moving forwards he can sprint and move normally
            }
            else
            {
                moveDirection.z *= (airspeed);//when player is moving backwards he uses mid-air speed
            }

			moveDirection = transform.TransformDirection(moveDirection);
    	}

        if(groundAngle>controller.slopeLimit && OnSlope()) //sliding on slope when slope angle is > slopeLimit
        {
            moveDirection.y += (-controller.slopeLimit); //force to hold player to slope
            moveDirection.x += (1f - hitNormal.y) * hitNormal.x * (controller.slopeLimit * slideSpeed); //moves the player to slide
            moveDirection.z += (1f - hitNormal.y) * hitNormal.z * (controller.slopeLimit * slideSpeed); //moves the player to slide
        }

        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime); //add gravity to movement

        controller.Move(moveDirection * Time.deltaTime); //move player


        if(OnSlope() && jump == false && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") !=0.0f)) //detects if player is moving on the slope and isnt jumping
        {
            controller.Move(Vector3.down * playerHeight / 2 * slopelimitcheckdistance * Time.deltaTime); //moves player to the slope
        }
		
		if (Input.GetKey(KeyCode.LeftAlt) == false || cameraMouse.fpp == true) //freecam key
 	  	{
			transform.rotation = Quaternion.Euler(0.0f, Camera.main.transform.eulerAngles.y, 0.0f);//rotate player where camera is facing
  	  	}
    }

    void LateUpdate() 
    {
        if(jump && controller.isGrounded)
        {
            jump = false; //disables jump after landing
        }        
    }

    private bool OnSlope() //detects if player is on the slope
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * 1.5f))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }

    //Apply player push force on rigidbody objects
	void OnControllerColliderHit(ControllerColliderHit hit)
    {
        groundAngle = Vector3.Angle(Vector3.up, hit.normal); //checks ground angle

        hitNormal = hit.normal; //we can use hit.normal outside the function

        Rigidbody body = hit.collider.attachedRigidbody;

        // If object has no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // if object is below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);

        // Apply the push
        body.velocity = pushDir * pushPower;
	}
}