using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bow : MonoBehaviour
{
    private Camera cam;
    public GameObject ArrowPrefab;
    public Transform ArrowSpawn;
	private UIController UIController;

	public float bowCoolDown = 0.0f;
	public float arrowCoolDown = 0.1f;
    private Vector3 RayOrigin;
    ConstantForce Force;
	public LayerMask LayersForArrow;

	public int maxAmmo = 25;
	public int currentAmmo;

    private CameraController cameraController;
    private PlayerController playerController;
	private Animator anim;

	public float aimDistance = 1000.0f;

	private Rigidbody rb;
	private GameObject go;

	[HideInInspector]
	public RaycastHit aimHit;
	[HideInInspector]
	public bool arrowReleased = false;
	[HideInInspector]
	public bool arrowInstantiated = false;

	[Header("ShootForce")]
	public float lerpSpeed = 25f;
	private float currentLerpTime;

	public float shootForce = 5.0f;
	public float maxShootForce = 75.0f;
	[HideInInspector]
	public bool maxShootForceAchieved = false;

	private void Start()
    {
		currentAmmo = maxAmmo;

		cam = ObjectsMenager.instance.cam;
		cameraController = cam.GetComponent<CameraController>();
        playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();

		anim = ObjectsMenager.instance.playerModel.GetComponent<Animator>();

		UIController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();
	}

	void Update()
	{
		if(bowCoolDown >= 0.0f)
		{
			bowCoolDown -= Time.deltaTime;
		}

		if(Physics.Raycast(RayOrigin, cam.transform.forward, out aimHit, aimDistance, LayersForArrow)) { }

		if(cameraController.aiming == true)
		{
			//temp
			transform.localRotation = Quaternion.Euler(cam.transform.eulerAngles.x, 0.0f, 90.0f);

			if (currentAmmo > 0)
			{
				if (Input.GetMouseButtonDown(0) || (Input.GetAxis("Fire1") == 1 && arrowInstantiated == false))
				{
					anim.SetTrigger("BowDraw");
					maxShootForceAchieved = false;
					shootForce = 5.0f;
					arrowReleased = false;
					go = Instantiate(ArrowPrefab, ArrowSpawn.position, Quaternion.LookRotation(transform.forward), transform);
					arrowInstantiated = true;
					rb = go.GetComponent<Rigidbody>();
					rb.constraints = RigidbodyConstraints.FreezePosition;
					arrowCoolDown = 0.1f;

					currentLerpTime = 0f;
				}

				if (Input.GetMouseButton(0) || Input.GetAxis("Fire1") == 1)
				{
					if (arrowCoolDown >= 0.0f)
					{
						arrowCoolDown -= Time.deltaTime;
					}

					if (shootForce <= maxShootForce - 1.5f && arrowCoolDown <= 0.0f && arrowInstantiated == true)
					{
						currentLerpTime += Time.deltaTime;
						if (currentLerpTime > lerpSpeed)
						{ 
							currentLerpTime = lerpSpeed;
						} 
						float perc = currentLerpTime / lerpSpeed;
						shootForce = Mathf.Lerp(shootForce, maxShootForce, perc);

						go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, go.transform.localPosition.z - 0.001f);
						anim.SetBool("BowPulling", true);
					}
					else
					{
						anim.SetBool("BowPulling", false);
						anim.SetBool("BowPulledHolding", true);
					}
					if(shootForce >= maxShootForce - 1.5f)
					{
						maxShootForceAchieved = true;
					}
				}

				RayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
				Debug.DrawRay(RayOrigin, cam.transform.forward * 1000, Color.yellow);//shows ray when in scene mode
				if ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown <= 0.0f && arrowCoolDown <= 0.0f && arrowInstantiated == true)
				{
					arrowReleased = true;
					anim.SetBool("BowPulledHolding", false);
					anim.SetTrigger("BowShot");

					rb.constraints = RigidbodyConstraints.None;

					if (maxShootForceAchieved == true)
					{
						rb.useGravity = false;
						go.GetComponent<ConstantForce>().enabled = false;
						shootForce = 100f;
					}
					else
					{
						rb.useGravity = true;
					}

					go.transform.parent = null;
					go.transform.position = ArrowSpawn.transform.position;

					if(aimHit.collider != null)
					{
						go.transform.rotation = Quaternion.LookRotation(aimHit.point);
						rb.velocity = (aimHit.point - transform.position).normalized * (shootForce + playerController.speed);
					}
					else
					{
						rb.velocity = cam.transform.forward * (shootForce + playerController.speed);
					}

					arrowInstantiated = false;
					currentAmmo--;

					UIController.AmmoChange(currentAmmo, maxAmmo);

					bowCoolDown = 1.0f;
				}
				else if ((((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown > 0.0f) || ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && arrowCoolDown > 0.0f)) && arrowInstantiated == true)
				{
					Debug.Log("elo2");
					anim.SetTrigger("HideArrow");
					arrowInstantiated = false;
					Destroy(go);
				}
			}
		}

		else if(cameraController.aiming == false && arrowReleased == false && arrowInstantiated == true)
		{
			arrowInstantiated = false;
			anim.SetTrigger("HideArrow");
			Destroy(go);
		}
		else
		{
			//temp
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
		}
		//Debug.Log(bowCoolDown + "   " + arrowCoolDown);
	}
	
	public void AddAmmo()
	{
		currentAmmo++;
		UIController.AmmoChange(currentAmmo, maxAmmo);
	}		
}
