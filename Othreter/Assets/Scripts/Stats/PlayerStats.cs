using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : CharacterStats
{
	private UIController UIController;

	[Space(10)]

	[SerializeField] private float waitToReloadAfterDeath = 4.0f;

	public override void Start()
	{
		base.Start();

		UIController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
        base.TakeDamage(damage);
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		base.TakeTrueDamage(damage);
	}

	public override void Heal(int amount)
	{
		base.Heal(amount);
	}

	public override void HealArmor(int amount)
	{
		base.HealArmor(amount);
	}

	public override void Die()
    {
        base.Die();

		DataHolder.playerState_Controllable = false;
		DataHolder.playerState_Dead = true;

		StartCoroutine(WaitToReloadTheLevel(waitToReloadAfterDeath));
	}

	public override void RefreshHealthUI()
	{
		base.RefreshHealthUI();

		UIController.HPChange(currentArmor.GetValue(), maxArmor.GetValue(), currentHealth.GetValue(), maxHealth.GetValue());
	}

	private IEnumerator WaitToReloadTheLevel(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		DataHolder.playerState_Controllable = true;
		DataHolder.playerState_Dead = false; //temp, do this in deathscreen

		SceneManager.LoadScene(SceneManager.GetActiveScene().name); //when player dies, game reloads level //temp, later change to deathscreen
	}
}