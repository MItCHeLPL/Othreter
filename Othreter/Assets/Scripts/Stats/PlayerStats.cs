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
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
		anim.SetTrigger("GotHurt");
        base.TakeDamage(damage);
		UIController.HPChange(currentHealth, maxHealth);
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		base.TakeTrueDamage(damage);
		UIController.HPChange(currentHealth, maxHealth);
	}

	public override void Die()
    {
        base.Die();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//when player dies, game reloads level
    }
}