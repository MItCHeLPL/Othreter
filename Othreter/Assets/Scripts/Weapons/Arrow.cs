using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody myBody;
    private bool hitSomething = false;

    Collision _collider;

    BoxCollider arrowCollider;

    EnemyStats enemy;

    Bow bow;
	private bool enemyHit = false;

	void Start()
    {
        myBody = GetComponent<Rigidbody>();

		myBody.velocity = Vector3.zero;

		arrowCollider = GetComponent<BoxCollider>();

        bow = ObjectsMenager.instance.bow.GetComponent<Bow>();

        Physics.IgnoreCollision(GetComponent<BoxCollider>(), ObjectsMenager.instance.player.GetComponent<CapsuleCollider>());
	}

    void Update()
    {
        if (!hitSomething && myBody.velocity != Vector3.zero)
        {
			//arrowCollider.center = new Vector3(arrowCollider.center.x, arrowCollider.center.y, (bow.shootForce + 0.35f) * -0.004f);
			transform.rotation = Quaternion.LookRotation(myBody.velocity);
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
		if(bow.arrowReleased == true)
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
				enemy.TakeDamage((int)bow.shootForce / 3);
			}
			else if (col.gameObject.CompareTag("Headshot"))
			{
				enemyHit = true;
				gameObject.transform.parent = _collider.transform.parent;
				afterCollision();
				enemy = col.transform.parent.gameObject.GetComponent<EnemyStats>();
				if(bow.maxShootForceAchieved == true)
				{
					enemy.TakeTrueDamage(enemy.currentHealth);
				}
				else
				{
					enemy.TakeTrueDamage((int)(bow.shootForce / 3) * 2);
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
		hitSomething = true;
		myBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
		arrowCollider.isTrigger = true;
		myBody.isKinematic = true;
        myBody.constraints = RigidbodyConstraints.FreezeAll;
        arrowCollider.size = new Vector3(1.5f, 1.75f, 1.5f);
        arrowCollider.center = new Vector3(0f, 0f, 0f);
    }
}
