using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    private float lastPositionY = 0.0f; //last postion on the ground
    private float fallDistance = 0.0f; //distance between new position and last position

    private PlayerStats player; //allows to use PlayerStats Script
    private PlayerController playerMovement; //allows to use PlayerMovement Script
	private Animator anim;

    [SerializeField] private float minFallDistance = 4.0f; //minimal distance that player have to fall to get damage
	[SerializeField] private float damageMultiplier = 2.5f; //how many times you want to divide the damage from falling

	[SerializeField] private GameObject trail = default;

	[SerializeField] private float speedAfterFalling = 3.0f; //valu that speed is reduced to when falling and getting damage

    private void Awake()
    {

		trail.SetActive(false);

        player = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerController>();
		anim = GetComponent<Animator>();
    }

    void Update()
    {
		if (DataHolder.playerState_Controllable)
		{
			if (lastPositionY > transform.position.y) //calculating fall distance
			{
				fallDistance += lastPositionY - transform.position.y;
			}

			lastPositionY = transform.position.y; //updating last position

			if (DataHolder.playerState_Grounded == false && DataHolder.playerState_Sliding == false)
			{
				if (fallDistance >= minFallDistance)
				{
					trail.SetActive(true);
					anim.SetBool("Falling", true);
					DataHolder.playerState_Falling = true;
				}
				else
				{
					anim.SetBool("Falling", false);
					DataHolder.playerState_Falling = false;
				}
			}

			if (fallDistance >= minFallDistance && DataHolder.playerState_Grounded && DataHolder.playerState_Sliding == false) //applying damage to player
			{
				anim.SetTrigger("Fallen");
				player.TakeTrueDamage((int)(fallDistance * damageMultiplier)); //giving true damage

				playerMovement.speed = speedAfterFalling; //reduce speed

				DataHolder.playerState_Fallen = true;
				resetValues();
			}

			if (Mathf.Abs(playerMovement.speed - speedAfterFalling) > 0.1f && DataHolder.playerState_Fallen == true) //regain normal speed
			{
				playerMovement.speed = Mathf.Lerp(playerMovement.speed, playerMovement.defaultSpeed - DataHolder.activeWeaponSpeedSub, Time.deltaTime);
			}
			else if (Mathf.Abs(playerMovement.speed - speedAfterFalling) <= 0.1f && DataHolder.playerState_Fallen == true)
			{
				if (DataHolder.playerState_Fallen == true)
				{
					DataHolder.playerState_Fallen = false;
				}
			}

			if (fallDistance < minFallDistance && DataHolder.playerState_Grounded)
			{
				resetValues(); //when player fell, but didnt get damage
			}
		}
    }
	public void resetValues()
	{
		trail.SetActive(false);
		lastPositionY = 0.0f;
		fallDistance = 0.0f;
		anim.SetBool("Falling", false);
		DataHolder.playerState_Falling = false;
	}
}
