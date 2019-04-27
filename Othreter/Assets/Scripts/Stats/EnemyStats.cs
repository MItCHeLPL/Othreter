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

		StartCoroutine(CheckArmour());

		RefreshHealthUI();
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
        base.TakeDamage(damage);
		RefreshHealthUI();
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		base.TakeTrueDamage(damage);
		RefreshHealthUI();
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

	private void RefreshHealthUI()
	{
		enemyUI.HPChange(armor.GetValue(), maxArmor, currentHealth, maxHealth);
	}

	private IEnumerator CheckArmour()
	{
		while (true)
		{
			if (armorRegenerating)
			{
				RefreshHealthUI();
			}
			yield return new WaitForSeconds(regenRatePerSecond - 0.1f);
		}
	}
}
