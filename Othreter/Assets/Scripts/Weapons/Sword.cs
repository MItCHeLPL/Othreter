using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
	EnemyStats enemyStats;
	CameraController cameraController;
	Animator anim;
	GameObject enemy;
	float angleToEnemy;

	public float coolDown = 1.0f;
	public int damage = 25;
	public float attackDistance = 5.0f;

	private bool wasHit = false;

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public override void Start()
	{
		cameraController = ObjectsMenager.instance.cam.GetComponent<CameraController>();
		anim = GetComponent<Animator>();

		//Physics.IgnoreCollision(GetComponent<BoxCollider>(), ObjectsMenager.instance.player.GetComponent<CapsuleCollider>()); when using ontriggerenter
	}

	public override void Update()
	{
		if (Input.GetMouseButtonDown(0) || (Input.GetAxis("Fire1") == 1 && wasHit == false))
		{
			//play anim

			//temp
			anim.SetTrigger("attack");
		}
	}

	private IEnumerator Cooldown(float sec)
	{
		yield return new WaitForSeconds(sec);
		wasHit = false;
	}

	/*private void OnTriggerEnter(Collider col) //sword has to have rigidbody and box collider with trigger for this to work
	{
		if (col.gameObject.CompareTag("Enemy") && wasHit == false)
		{
			enemyStats = col.gameObject.GetComponent<EnemyStats>();
			enemyStats.TakeDamage(damage);
			wasHit = true;
			StartCoroutine(Cooldown(coolDown));
		}
	}*/

	private void Damage() //is triggered by animation event
	{
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject currentEnemy in allEnemies)
		{
			float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;

			if (distanceToEnemy <= attackDistance)
			{
				enemyStats = currentEnemy.GetComponent<EnemyStats>();

				angleToEnemy = Vector3.Angle(currentEnemy.transform.position - ObjectsMenager.instance.player.transform.position, ObjectsMenager.instance.player.transform.forward);

				if (angleToEnemy >= -80 && angleToEnemy <= 80 && currentEnemy != null)
				{
					enemyStats.TakeDamage(damage);
					wasHit = true;
				}
			}
		}
		StartCoroutine(Cooldown(coolDown));
	}

	public override void Aim()
	{
		base.Aim();
	}

	public override void StopAim()
	{
		base.StopAim();
	}
}