using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : CharacterStats
{
	private Animator anim;
	private UIController UIController;

	private void Start()
	{
		anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

		UIController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();


		StartCoroutine(CheckArmour());
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
		anim.SetTrigger("GotHurt");
        base.TakeDamage(damage);
		RefreshHealthUI();
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		anim.SetTrigger("GotHurt");
		base.TakeTrueDamage(damage);
		RefreshHealthUI();
	}

	public override void Die()
    {
        base.Die();

		anim.ResetTrigger("GotHurt");
		anim.SetTrigger("Death");

		//state death, cant control itp.

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);//when player dies, game reloads level //after fadee itp.
	}

	private void RefreshHealthUI()
	{
		UIController.HPChange(armor.GetValue(), maxArmor, currentHealth, maxHealth);
	}

	private IEnumerator CheckArmour()
	{
		while (true)
		{
			if(armorRegenerating)
			{
				RefreshHealthUI();
			}
			yield return new WaitForSeconds(regenRatePerSecond - 0.1f);
		}
	}
}