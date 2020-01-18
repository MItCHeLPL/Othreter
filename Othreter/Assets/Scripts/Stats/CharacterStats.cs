using UnityEngine;
using System.Collections;

/* Base class that player and enemies can derive from to include stats. */

public class CharacterStats : MonoBehaviour
{
	//public int currentHealth { get; private set; }

	private Animator anim;

	[HideInInspector]
	public Stat damage;

	[Header("Settings")]
	public Stat currentHealth;
	public Stat maxHealth;

	[Space(10)]

	[SerializeField] private bool armorEnabled = true;

	public Stat currentArmor;
	public Stat maxArmor;

	public int waitBeforeArmorRegen = 5;
	public int addedArmorPerRegen = 5;
	public int regenRatePerSecond = 1;

	private IEnumerator armorRegen = null;

	[HideInInspector]
	public bool isAlive = true;
	[HideInInspector]
	public bool armorRegenerating = false;

	public virtual void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Awake()
    {
        currentHealth.SetValue(maxHealth.GetValue());
		if(currentArmor.GetValue() < maxArmor.GetValue() && armorEnabled)
		{
			armorRegen = ArmorRegen();
			StartCoroutine(armorRegen);
		}
		else if(!armorEnabled)
		{
			currentArmor.SetValue(0);
		}
    }

	// Damage the character
	public virtual void TakeDamage(int damage)
    {
		if(isAlive)
		{
			if (armorEnabled)
			{
				if (armorRegenerating)
				{
					StopCoroutine(armorRegen);
				}

				if (currentArmor.GetValue() > 0)
				{
					int temp = currentArmor.GetValue();
					currentArmor.SetValue(Mathf.Clamp(currentArmor.GetValue() - damage, 0, maxArmor.GetValue()));
					damage -= temp;
				}

				damage = Mathf.Clamp(damage, 0, int.MaxValue);

				anim.SetTrigger("GotHurt");

				// Damage the character
				currentHealth.SetValue(Mathf.Clamp(currentHealth.GetValue() - damage, 0, maxHealth.GetValue()));

				// If health reaches zero
				if (currentHealth.GetValue() <= 0)
				{
					Die();
				}

				armorRegen = ArmorRegen();
				StartCoroutine(armorRegen);

				RefreshHealthUI();
			}
			else
			{
				TakeTrueDamage(damage);
			}
		}
	}

    public virtual void TakeTrueDamage(int damage) //damage that ignores armor
    {
		if (isAlive)
		{
			// Damage the character
			if (armorEnabled)
			{
				currentArmor.SetValue(Mathf.Clamp(currentArmor.GetValue() - damage, 0, maxArmor.GetValue()));
			}
			currentHealth.SetValue(Mathf.Clamp(currentHealth.GetValue() - damage, 0, maxHealth.GetValue()));

			anim.SetTrigger("GotHurt");

			// If health reaches zero
			if (currentHealth.GetValue() <= 0)
			{
				Die();
			}

			if (armorEnabled)
			{
				armorRegen = ArmorRegen();
				StartCoroutine(armorRegen);
			}

			RefreshHealthUI();
		}
	}

	public virtual void Heal(int amount) //damage that ignores armor
	{
		if (isAlive)
		{
			currentHealth.SetValue(currentHealth.GetValue() + amount);

			RefreshHealthUI();
		}
	}

	public virtual void HealArmor(int amount) //damage that ignores armor
	{
		if (isAlive)
		{
			currentArmor.SetValue(Mathf.Clamp(currentArmor.GetValue() + amount, 0, maxArmor.GetValue()));

			RefreshHealthUI();
		}
	}

	public virtual void Die()
    {
		isAlive = false;
		anim.ResetTrigger("GotHurt");
		anim.SetTrigger("Death");

		// This method is meant to be overwritten
	}

	public virtual void RefreshHealthUI()
	{
		//To be overwritten
	}

	private IEnumerator ArmorRegen()
	{
		armorRegenerating = true;
		yield return new WaitForSeconds(waitBeforeArmorRegen);

		while (currentArmor.GetValue() < maxArmor.GetValue())
		{
			currentArmor.SetValue(Mathf.Clamp(currentArmor.GetValue() + addedArmorPerRegen, 0, maxArmor.GetValue()));
			RefreshHealthUI();
			yield return new WaitForSeconds(regenRatePerSecond);
		}
		armorRegenerating = false;
	}
}