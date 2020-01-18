using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	private Rigidbody myBody;
	private bool hitSomething = false;

	private Collision _collider;

	private BoxCollider arrowCollider;

	private EnemyStats enemy;

	private Bow bow;

	private bool enemyHit = false;

	[SerializeField] private GameObject trail = default;
	[SerializeField] private GameObject arrowLight = default;

	private bool released = false;

	[HideInInspector]
	public float speed = 0.0f;

	void Start()
	{
		myBody = GetComponent<Rigidbody>();

		myBody.velocity = Vector3.zero;

		arrowCollider = GetComponent<BoxCollider>();

		bow = ObjectsMenager.instance.bow.GetComponent<Bow>();

		Physics.IgnoreCollision(GetComponent<BoxCollider>(), ObjectsMenager.instance.player.GetComponent<CapsuleCollider>());

		arrowCollider.enabled = false;
		trail.SetActive(false);
		arrowLight.SetActive(false);
	}

	void Update()
	{
		if (!hitSomething && myBody.velocity != Vector3.zero)
		{
			arrowCollider.enabled = true;
			//arrowCollider.center = new Vector3(arrowCollider.center.x, arrowCollider.center.y, (bow.shootForce + 0.35f) * -0.004f);
			transform.rotation = Quaternion.LookRotation(myBody.velocity);

			trail.SetActive(true);

			released = true;
		}

		/*if(bow.arrowsOnGround > 25)
        {
            StartCoroutine(Destroy());
			bow.currentAmmo++;
		}*/
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds(3);
		Destroy(gameObject);
	}

	private void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Player") && enemyHit == false)
		{
			bow.AddAmmo();
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision col)
	{
		if (released == true)
		{
			_collider = col;

			if (col.gameObject.CompareTag("Arrow") || col.gameObject.CompareTag("Bow"))
			{
				Physics.IgnoreCollision(arrowCollider, col.collider);
			}

			else if (col.gameObject.CompareTag("Enemy"))
			{
				enemyHit = true;
				gameObject.transform.parent = _collider.transform;
				afterCollision();
				enemy = col.gameObject.GetComponent<EnemyStats>();
				enemy.TakeDamage((int)speed / 3);
			}
			else if (col.gameObject.CompareTag("Headshot"))
			{
				enemyHit = true;
				gameObject.transform.parent = _collider.transform.parent;
				afterCollision();
				enemy = col.transform.parent.gameObject.GetComponent<EnemyStats>();
				if (bow.maxShootForce <= speed)
				{
					enemy.TakeTrueDamage(enemy.currentHealth.GetValue());
				}
				else
				{
					enemy.TakeTrueDamage((int)(speed / 3) * 2);
				}
			}

			if (col.gameObject.GetComponent<Rigidbody>() != null)
			{
				gameObject.transform.parent = _collider.transform;
				afterCollision();
			}

			else
			{
				afterCollision();
			}
		}
	}

	private void afterCollision()
	{
		trail.GetComponent<TrailRenderer>().time = 0.3f;
		arrowLight.SetActive(true);
		hitSomething = true;
		myBody.constraints = RigidbodyConstraints.FreezeAll;
		myBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		arrowCollider.isTrigger = true;
		myBody.isKinematic = true;
		arrowCollider.size = new Vector3(5.0f, 5.0f, 50.0f);
		arrowCollider.center = new Vector3(0f, 0f, 15.0f);
		trail.transform.SetParent(null);
	}
}
