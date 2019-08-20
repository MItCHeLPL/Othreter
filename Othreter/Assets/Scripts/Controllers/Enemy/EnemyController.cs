using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Transform target;   // Reference to the player
    UnityEngine.AI.NavMeshAgent agent; // Reference to the NavMeshAgent
    
    private PlayerController playerController; //player controller script
    private float distance; //distance between player and enemy
    private Vector3 targetDir; //direction for enemy to face
    private float angleToPlayer; // angle to player
    private Vector3 lastPlayerPosition; //last player position whet he was in sight
    private bool wasInSight = false; //if player was once i nsight

	public float lookDistance = 20.0f;
    public float minWaitTime = 0.0f;
    public float maxWaitTime = 6.0f;
    private float waitTime;

    public Transform[] moveSpots;
    private int randomSpot;
    private int prevRandomSpot;
    private int prevPrevRandomSpot;

	public float hitspeed = 2.0f;
	public float normalspeed = 4.0f;
	private bool wasHit = false;

	private float coolDown = 3.0f;

	public bool inFight = false;

	PlayerStats playerStats;

	private Animator anim;

	void Start()
    {
        target = ObjectsMenager.instance.player.transform; //player gameobject
        playerController = target.GetComponent<PlayerController>(); //player controller script
		playerStats = target.GetComponent<PlayerStats>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //nav mesh

        lastPlayerPosition = transform.position; //last player position

        agent.stoppingDistance = agent.radius + 1.0f; //do not move the player
        agent.speed = playerController.speed - 1.0f;

        waitTime = Random.Range(minWaitTime, maxWaitTime);
        randomSpot = Random.Range(0, moveSpots.Length);

		anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Calculations
        distance = Vector3.Distance(target.position, transform.position);
        targetDir = target.position - transform.position;
        angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

		anim.SetFloat("Velocity", agent.velocity.magnitude);

		UnityEngine.AI.NavMeshHit hit;
		// If inside the lookRadius, in fov of enemy, is steping loudlyand if is in direct line of sight
		if (distance <= lookDistance && angleToPlayer >= -80 && angleToPlayer <= 80 && !agent.Raycast(target.position, out hit) || distance <= 5.0f && playerController.crouch == false && !agent.Raycast(target.position, out hit))
        {
            lastPlayerPosition = target.position; //last known player position
            wasInSight = true; //wa in sight once
            waitTime = Random.Range(minWaitTime, maxWaitTime);
			//Debug.Log("In sight");

			


			agent.SetDestination(lastPlayerPosition);
			if (Vector3.Distance(transform.position, lastPlayerPosition) < 1.4f)
			{
				inFight = true;
				if (coolDown < 0)
				{
					anim.SetTrigger("Attack");
					playerStats.TakeDamage(10);
					coolDown = 3.0f;
				}
				else if (coolDown >= 0)
				{
					coolDown -= Time.deltaTime;
				}
			}
			else
			{
				inFight = false;
			}

				// If within attacking distance
			if (distance <= agent.stoppingDistance)
            {
                FaceTarget();   // Make sure to face towards the target
            }
        }

        else if (wasInSight) //search for player in his last position
        {
            if (Vector3.Distance(transform.position, lastPlayerPosition) < 1.4f)
            {
                if (waitTime <= 0)
                {
                    //Debug.Log("Coming back");
                    wasInSight = false;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                    //Debug.Log("Waiting");
                }
            }
            else
            {
                agent.SetDestination(lastPlayerPosition);
                //Debug.Log("searching");
            }
        }

        else
        {
            if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 1.4f)
            { 
                if (waitTime <= 0)
                {
					// Debug.Log("Next point");
					prevPrevRandomSpot = prevRandomSpot;
                    prevRandomSpot = randomSpot;
                    while (prevRandomSpot == randomSpot || randomSpot == prevPrevRandomSpot)
                    {
                        randomSpot = Random.Range(0, moveSpots.Length);
                        if (randomSpot == prevRandomSpot || randomSpot == prevPrevRandomSpot)
                        {
                            randomSpot = Random.Range(0, moveSpots.Length);
                        }
                        
                        else
                        {
                            waitTime = Random.Range(minWaitTime, maxWaitTime);
                        }
                    }
                }

                else
                {
                    waitTime -= Time.deltaTime;
                    //Debug.Log("Waiting");
                }
            }

            else
            {
                agent.SetDestination(moveSpots[randomSpot].position);
            }
        }

		if(wasHit == true)
		{
			agent.speed = Mathf.Lerp(agent.speed, normalspeed, 2.0f * Time.deltaTime);
			wasInSight = true;
			if (agent.speed == (normalspeed - 0.1f) && Vector3.Distance(transform.position, lastPlayerPosition) < 1.4f)
			{
				agent.SetDestination(moveSpots[randomSpot].position);
				wasInSight = false;
				wasHit = false;
			}
		}
    }



	private void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.CompareTag("Arrow"))
		{
			agent.speed = hitspeed;
			lastPlayerPosition = target.position;
			wasHit = true;
		}
	}

	// Rotate to face the target
	void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0.0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.0f);
    }
}
