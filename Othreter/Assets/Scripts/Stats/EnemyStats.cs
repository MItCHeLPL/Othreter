using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Rigidbody rb;
	PlayerController playerController;
	EnemyUI enemyUI;
	[SerializeField]
	private GameObject model;
	[SerializeField]
	private Material dieMaterial;
	[SerializeField]
	private GameObject uIGameObject;

	private Renderer modelRenderer;

	private Animator anim;

	private void Start()
	{
		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();
		enemyUI = GetComponent<EnemyUI>();
		anim = GetComponent<Animator>();

		StartCoroutine(CheckArmour());

		RefreshHealthUI();

		modelRenderer = model.gameObject.GetComponent<Renderer>();
	}

	public override void TakeDamage(int damage) //u can add more events while taking damage only for player
    {
        base.TakeDamage(damage);
		anim.SetTrigger("GotHurt");
		RefreshHealthUI();
	}

	public override void TakeTrueDamage(int damage) //u can add more events while taking damage only for player
	{
		base.TakeTrueDamage(damage);
		anim.SetTrigger("GotHurt");
		RefreshHealthUI();
	}

	public override void Die()
    {
        base.Die();

		anim.ResetTrigger("GotHurt");
		anim.SetTrigger("Death");
		enemyUI.enabled = false;
		uIGameObject.SetActive(false);
		GetComponent<EnemyController>().enabled = false;
		GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

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

		StartCoroutine(Dissolve());

		/*if (playerController.swordAiming == true)
		{
			playerController.FindEnemy(gameObject, false);
		}*/
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

	public IEnumerator Dissolve()
	{
		modelRenderer.material = dieMaterial;

		int progressId = Shader.PropertyToID("Vector1_CFDFAB28");

		float progress = modelRenderer.material.GetFloat(progressId);

		yield return new WaitForSeconds(3.0f);

		while (modelRenderer.material.GetFloat(progressId) < 1)
		{
			progress = Mathf.Lerp(progress, 1.0f, Time.deltaTime * 0.75f);
			modelRenderer.material.SetFloat(progressId, progress);
			if (progress > 0.75f)
			{
				Destroy(gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
