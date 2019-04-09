using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Rigidbody rb;
	PlayerController playerController;
	EnemyUI enemyUI;

	private void Start()
	{
		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();
		enemyUI = GetComponent<EnemyUI>();

		enemyUI.HPChange(currentHealth, maxHealth);
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
        base.TakeDamage(damage);
		enemyUI.HPChange(currentHealth, maxHealth);
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		base.TakeTrueDamage(damage);
		enemyUI.HPChange(currentHealth, maxHealth);
	}

	public override void Die()
    {
        base.Die();

        foreach (Transform child in transform)
        {
            if(child.transform.CompareTag("Arrow"))
            {
				/*rb = child.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.constraints = ~RigidbodyConstraints.FreezePosition;
                child.GetComponent<BoxCollider>().isTrigger = false;
                child.parent = null;*/
				GameObject.Destroy(child.gameObject);
            }
        }
		//FadeOut
		if(playerController.swordAiming == true)
		{
			playerController.FindEnemy(gameObject, false);
		}
		Destroy(gameObject);
	}
}
