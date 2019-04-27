using UnityEngine;
using System.Collections;

/* Base class that player and enemies can derive from to include stats. */

public class CharacterStats : MonoBehaviour
{
    // Health
    public int maxHealth = 100;
	public int maxArmor = 100;
	public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

	public int waitBeforeArmorRegen = 5;
	public int addedArmorPerRegen = 5;
	public int regenRatePerSecond = 1;

	[HideInInspector]
	public bool armorRegenerating = false;

	// Set current health to max health
	// when starting the game.

	private IEnumerator armorRegen = null;

    void Awake()
    {
        currentHealth = maxHealth;
    }

	// Damage the character
	public virtual void TakeDamage(int damage)
    {
		if(armorRegenerating)
		{
			StopCoroutine(armorRegen);
		}
		
		if(armor.GetValue() > 0)
		{
			int temp = armor.GetValue();
			armor.SetValue(Mathf.Clamp(armor.GetValue() - damage, 0, maxArmor));
			damage -= temp;
		}

        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        // Damage the character
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        // If health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
		armorRegen = ArmorRegen();
		StartCoroutine(armorRegen);
	}

    public virtual void TakeTrueDamage(int damage) //damage that ignores armor
    {
        // Damage the character
        currentHealth -= damage;

        // If health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten
    }

	private IEnumerator ArmorRegen()
	{
		armorRegenerating = true;
		yield return new WaitForSecondsRealtime(waitBeforeArmorRegen);

		while (armor.GetValue() < maxArmor)
		{
			armor.SetValue(Mathf.Clamp(armor.GetValue() + addedArmorPerRegen, 0, maxArmor));
			yield return new WaitForSecondsRealtime(regenRatePerSecond);
		}
		armorRegenerating = false;
	}

}