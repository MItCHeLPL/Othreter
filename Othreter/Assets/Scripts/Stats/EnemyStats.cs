using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
	[Space(10)]

	Rigidbody rb;

	EnemyUI enemyUI;
	[SerializeField]
	private GameObject model = default;
	[SerializeField]
	private Material dieMaterial = default;
	[SerializeField]
	private GameObject uIGameObject = default;

	private Renderer modelRenderer;

	public override void Start()
	{
		base.Start();

		enemyUI = GetComponent<EnemyUI>();

		RefreshHealthUI();

		modelRenderer = model.gameObject.GetComponent<Renderer>();
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

		StartCoroutine(Dissolve()); //switch to death effect
	}

	public override void RefreshHealthUI()
	{
		base.RefreshHealthUI();

		enemyUI.HPChange(currentArmor.GetValue(), maxArmor.GetValue(), currentHealth.GetValue(), maxHealth.GetValue());
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
