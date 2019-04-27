using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private CharacterController controller; //controller

    private float lastPositionY = 0.0f; //last postion on the ground
    private float fallDistance = 0.0f; //distance between new position and last position
    private float velocity = 0.0f; //falling veloity

    private PlayerStats player; //allows to use PlayerStats Script
    private PlayerController playerMovement; //allows to use PlayerMovement Script
	private Animator anim;

    public float minFallDistance = 4.0f; //minimal distance that player have to fall to get damage
    public float minVelocity = 15.0f; //minimal velocity, that player has to have to get damage
    public float damageDivider = 2.0f; //how many times you want to divide the damage from falling

	public GameObject trail;

    public float speedAfterFalling = 3.0f; //valu that speed is reduced to when falling and getting damage

    [HideInInspector]
    public bool fallen; //if player got damage from falling

    private void Awake()
    {
        controller = GetComponent<CharacterController>(); //conroller

		trail.SetActive(false);

        player = GetComponent<PlayerStats>(); //allows to use PlayerStats Script
        playerMovement = GetComponent<PlayerController>(); //allows to get variables from PlayerMovement.cs
		anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

        if (minVelocity > 0)
        {
            minVelocity *= -1; //minimum velocity is converted to calculate falling velocity
        }
    }

    public void resetValues() //resets values after falling
    {
		trail.SetActive(false);
		lastPositionY = 0.0f;
        fallDistance = 0.0f;
        velocity = 0.0f;
    }

    public void resetFallen() //resets boolean 
    {
        if (fallen == true)
        {
            fallen = false;
        }
    }
    void Update()
    {
        if (lastPositionY > transform.position.y) //calculating fall distance
        {
            fallDistance += lastPositionY - transform.position.y; 
        }

        lastPositionY = transform.position.y; //updating last position

        if (controller.isGrounded == false)
        {
            velocity = controller.velocity.y; //calculating velocity

			if(fallDistance >= minFallDistance && velocity <= minVelocity)
			{
				trail.SetActive(true);
				anim.SetBool("Falling", true);
			}
			else
			{
				anim.SetBool("Falling", false);
			}
        }

        if (fallDistance >= minFallDistance && velocity <= minVelocity && controller.isGrounded) //applying damag to player
        {
			anim.SetTrigger("Fallen");
            player.TakeTrueDamage((int)((velocity * -1) / damageDivider)); //givint true damage

            fallen = true; //player fell
            playerMovement.speed = speedAfterFalling; //reduce speed

            resetValues(); //reset values
            Invoke("resetFallen", 2.0f); //resets boolean after time so player regain his normal speed
        }

        if(playerMovement.speed >= speedAfterFalling && fallen == true)
        {
            playerMovement.speed = Mathf.Lerp(playerMovement.speed, playerMovement.oldSpeed, 2.0f * Time.deltaTime); //regain speed after time
        }

        if (fallDistance < minFallDistance && velocity > minVelocity && controller.isGrounded)
        {
            resetValues(); //when player fell, but didnt get damage
        }
    } 
}
