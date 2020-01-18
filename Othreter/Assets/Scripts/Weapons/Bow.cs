using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bow : Weapon
{
    private Camera cam;
	private UIController UIController;
	private SkinnedMeshRenderer meshRenderer;

	private PlayerController playerController;
	private Animator anim;

	[Header("Arrow")]
	[SerializeField]
	private GameObject ArrowPrefab = default;
	[SerializeField]
	private Transform ArrowSpawn = default;

	[Header("Cooldowns")]
	[SerializeField]
	private float bowCoolDown = 0.1f;
	[SerializeField]
	private float baseArrowCoolDown = 0.1f;
	private float arrowCoolDown;
    private Vector3 RayOrigin;

	[Header("Ammo")]
	[SerializeField]
	private int maxAmmo = 25;
	public int currentAmmo;

	[Header("Raycast")]
	[SerializeField]
	private float aimDistance = 1000.0f;
	[SerializeField]
	private LayerMask LayersForArrow = default;

	private Rigidbody arrowRB; //Arrow's rigidbody
	private GameObject arrow; //Arrow

	[HideInInspector]
	public RaycastHit aimHit;

	[Header("Shoot Force")]
	[SerializeField]
	private float lerpSpeed = 25f;
	private float currentLerpTime = 0.0f;

	public float shootForce = 0.0f;
	[SerializeField]
	private float baseShootForce = 5.0f;
	public float maxShootForce = 75.0f;
	[HideInInspector]
	public bool maxShootForceAchieved = false;

	[Header("Bools")]
	private bool startedToPull = false;
	private bool arrowReleased = false;
	[HideInInspector]
	public bool arrowInstantiated = false;

	[Header("PlayerIk")]
	private Transform chest;
	private Transform head;
	[SerializeField]
	private Vector3 boneAimOffset = new Vector3(0,0,0);


	public override void Start()
    {
		base.Start();

		currentAmmo = maxAmmo;

		cam = ObjectsMenager.instance.cam;
		playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();

		meshRenderer = GetComponent<SkinnedMeshRenderer>();

		anim = ObjectsMenager.instance.player.GetComponent<Animator>();

		UIController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();

		chest = anim.GetBoneTransform(HumanBodyBones.Chest);
		head = anim.GetBoneTransform(HumanBodyBones.Head);
	}
	public override void OnEnable()
	{
		base.OnEnable();

		if (arrowInstantiated == false && currentAmmo > 0) //instantiate arrow when picking up a bow
		{
			InstantiateArrow(); // add as a trigger in animation
		}

		StartCoroutine(DataHolder.SetAnimLayer(anim, DataHolder.BowEquippedLayerId, 1, 10.0f));
	}

	public void OnDisable()
	{
		anim.SetLayerWeight(DataHolder.BowEquippedLayerId, 0);
	}

	void FixedUpdate()
	{
		if (Physics.Raycast(RayOrigin, cam.transform.forward, out aimHit, aimDistance, LayersForArrow)) { } //raycast when not hitting anything
	}

	public override void Update()
	{
		base.Update();

		if (bowCoolDown >= 0.0f)
		{
			bowCoolDown -= Time.deltaTime; //cooldown after shooting
		}

		if(DataHolder.playerState_Aiming == true && DataHolder.playerState_Controllable)
		{
			if (currentAmmo > 0)
			{
				if ((Input.GetMouseButtonDown(0) || Input.GetAxis("Fire1") == 1) && arrowReleased == false && arrowInstantiated == true)
				{
					anim.SetLayerWeight(DataHolder.BowArrowLayerId, Mathf.Lerp(anim.GetLayerWeight(DataHolder.BowArrowLayerId), 1, 10 * Time.deltaTime));
				}

				if ((Input.GetMouseButton(0) || Input.GetAxis("Fire1") == 1) && arrowInstantiated == true)
				{
					if (arrowCoolDown >= 0.0f)
					{
						arrowCoolDown -= Time.deltaTime; //not to spam arrows
					}

					if (shootForce <= maxShootForce - 1.5f && arrowCoolDown <= 0.0f) //calculate shoot force
					{
						if(startedToPull == false)
						{
							startedToPull = true;
							anim.SetTrigger("BowDraw");
						}



						//CHECK
						currentLerpTime += Time.deltaTime;
						if (currentLerpTime > lerpSpeed)
						{ 
							currentLerpTime = lerpSpeed;
						} 
						float perc = currentLerpTime / lerpSpeed;
						shootForce = Mathf.Lerp(shootForce, maxShootForce, perc);



						meshRenderer.SetBlendShapeWeight(0, Mathf.Clamp(shootForce, 0.0f, 100.0f)); //offset arrow while pulling
						arrow.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.Clamp(shootForce, 0.0f, 100.0f));
					}
					if(shootForce >= maxShootForce - 1.5f)
					{
						maxShootForceAchieved = true;
					}
				}

				RayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
				Debug.DrawRay(RayOrigin, cam.transform.forward * 1000, Color.yellow);//shows ray when in scene mode

				if ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown <= 0.0f && arrowCoolDown <= 0.0f && arrowInstantiated == true && startedToPull == true) //fire an arrow
				{
					anim.SetTrigger("BowShot");
					anim.ResetTrigger("BowDraw");



					//CHECK
					arrowRB.constraints = RigidbodyConstraints.None; //apply phisycs to arrow

					if (maxShootForceAchieved == true) //go in straight line when full shoot force
					{
						arrowRB.useGravity = false;
						arrow.GetComponent<ConstantForce>().enabled = false;
						shootForce = 100f;
					}
					else
					{
						arrowRB.useGravity = true;
					}

					arrow.transform.parent = null;
					arrow.transform.position = ArrowSpawn.transform.position;

					if(aimHit.collider != null) //arrow lookat where player was aiming
					{
						arrow.transform.rotation = Quaternion.LookRotation(aimHit.point);
						arrowRB.velocity = (aimHit.point - transform.position).normalized * (shootForce + playerController.speed);
					}
					else //arrow lookat in front of push force
					{
						arrowRB.velocity = cam.transform.forward * (shootForce + playerController.speed);
					}





					meshRenderer.SetBlendShapeWeight(0, 0); //reset pull on bow
					arrow.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
					arrowInstantiated = false;
					currentAmmo--;

					UIController.AmmoChange(currentAmmo, maxAmmo);

					bowCoolDown = 1.0f;

					arrowReleased = true;

					arrow.GetComponent<Arrow>().speed = shootForce;

					if (arrowInstantiated == false && currentAmmo > 0)
					{
						InstantiateArrow();
					}
				}
				else if ((((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown > 0.0f) || ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && arrowCoolDown > 0.0f)) && arrowInstantiated == true) //cancel shooting when bugs
				{
					CancelAim();
				}
			}
		}
		else if(DataHolder.playerState_Aiming == false && arrowReleased == false && arrowInstantiated == true && startedToPull) //cancel shooting when stopping aim
		{
			CancelAim();
		}
	}

	private void LateUpdate() //set player model position
	{
		if(DataHolder.playerState_Aiming == true)
		{
			chest.rotation = cam.transform.rotation;
			head.rotation = cam.transform.rotation;

			chest.rotation *= Quaternion.Euler(boneAimOffset); //temp
			head.rotation *= Quaternion.Euler(-boneAimOffset * 0.75f); //temp
		}
	}

	public void InstantiateArrow() 
	{
		arrow = Instantiate(ArrowPrefab, ArrowSpawn.position, Quaternion.LookRotation(transform.forward), transform);
		arrowInstantiated = true;
		arrowRB = arrow.GetComponent<Rigidbody>();
		arrowRB.constraints = RigidbodyConstraints.FreezePosition;
		arrowReleased = false;
		startedToPull = false;
		arrowCoolDown = baseArrowCoolDown;
		maxShootForceAchieved = false;
		shootForce = baseShootForce;
		currentLerpTime = 0f;
	}

	public void ResetValues()
	{
		arrowRB.constraints = RigidbodyConstraints.FreezePosition;
		arrowReleased = false;
		startedToPull = false;
		arrowCoolDown = baseArrowCoolDown;
		maxShootForceAchieved = false;
		shootForce = baseShootForce;
		currentLerpTime = 0f;
		anim.ResetTrigger("BowShot");
	}

	public void CancelAim()
	{
		ResetValues();

		StartCoroutine(DataHolder.SetAnimLayer(anim, DataHolder.BowArrowLayerId, 0, 10.0f));

		meshRenderer.SetBlendShapeWeight(0, Mathf.Lerp(arrow.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), 0.0f, 2*Time.deltaTime)); //smooth arrow back to 0 place
		arrow.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.Lerp(arrow.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0), 0.0f, 2*Time.deltaTime));
	}

	public void AddAmmo()
	{
		currentAmmo++;
		UIController.AmmoChange(currentAmmo, maxAmmo);
		anim.SetTrigger("PickUpArrow");
	}

	public override void Aim()
	{
		base.Aim();

		anim.SetLayerWeight(DataHolder.BowAimLayerId, 1);

		//playermodel rotation
		if(playerController.modelRotationEnabled)
		{
			playerController.transform.rotation = Quaternion.Lerp(playerController.transform.rotation, Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0), 7.5f * Time.deltaTime);
			playerController.modelRotation = playerController.transform.rotation;
		}	
	}

	public override void StopAim()
	{
		base.StopAim();

		StartCoroutine(DataHolder.SetAnimLayer(anim, DataHolder.BowAimLayerId, 0, 10.0f));
	}
}

/*OLD VERSION*/

/*private Camera cam;
public GameObject ArrowPrefab;
public Transform ArrowSpawn;
private UIController UIController;

private SkinnedMeshRenderer meshRenderer;

public float bowCoolDown = 0.0f;
public float arrowCoolDown = 0.1f;
private Vector3 RayOrigin;
ConstantForce Force;
public LayerMask LayersForArrow;

public int maxAmmo = 25;
public int currentAmmo;

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


[Header("PlayerIk")]
private Transform chest;
private Transform head;
[SerializeField]
private Vector3 boneAimOffset;

public override void OnEnable()
{
	base.OnEnable();
}

public override void Start()
{
	base.Start();

	currentAmmo = maxAmmo;

	cam = ObjectsMenager.instance.cam;
	playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();

	meshRenderer = GetComponent<SkinnedMeshRenderer>();

	anim = ObjectsMenager.instance.player.GetComponent<Animator>();

	UIController = ObjectsMenager.instance.UIMenager.GetComponent<UIController>();

	chest = anim.GetBoneTransform(HumanBodyBones.Chest);
	head = anim.GetBoneTransform(HumanBodyBones.Head);
}

void FixedUpdate()
{
	if (Physics.Raycast(RayOrigin, cam.transform.forward, out aimHit, aimDistance, LayersForArrow)) { }
}

public override void Update()
{
	base.Update();

	if (bowCoolDown >= 0.0f)
	{
		bowCoolDown -= Time.deltaTime;
	}

	if (DataHolder.playerState_Aiming == true)
	{
		if (currentAmmo > 0)
		{
			if (Input.GetMouseButtonDown(0) || (Input.GetAxis("Fire1") == 1 && arrowInstantiated == false))
			{
				anim.ResetTrigger("HideArrow");
				anim.ResetTrigger("BowShot");
				anim.SetTrigger("BowDraw");
				anim.SetLayerWeight(DataHolder.BowArrowLayerId, 1);
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

					meshRenderer.SetBlendShapeWeight(0, Mathf.Clamp(shootForce, 0.0f, 100.0f));
					go.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, Mathf.Clamp(shootForce, 0.0f, 100.0f));

					go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, go.transform.localPosition.z - 0.001f);
				}
				if (shootForce >= maxShootForce - 1.5f)
				{
					maxShootForceAchieved = true;
				}
			}

			RayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			Debug.DrawRay(RayOrigin, cam.transform.forward * 1000, Color.yellow);//shows ray when in scene mode
			if ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown <= 0.0f && arrowCoolDown <= 0.0f && arrowInstantiated == true)
			{
				arrowReleased = true;
				anim.ResetTrigger("BowDraw");
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

				if (aimHit.collider != null)
				{
					go.transform.rotation = Quaternion.LookRotation(aimHit.point);
					rb.velocity = (aimHit.point - transform.position).normalized * (shootForce + playerController.speed);
				}
				else
				{
					rb.velocity = cam.transform.forward * (shootForce + playerController.speed);
				}

				meshRenderer.SetBlendShapeWeight(0, 0);
				go.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
				arrowInstantiated = false;
				currentAmmo--;

				UIController.AmmoChange(currentAmmo, maxAmmo);

				bowCoolDown = 1.0f;
			}
			else if ((((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && bowCoolDown > 0.0f) || ((Input.GetMouseButtonUp(0) || (Input.GetAxis("Fire1") != 1 && arrowReleased == false)) && arrowCoolDown > 0.0f)) && arrowInstantiated == true)
			{
				anim.SetTrigger("HideArrow");
				meshRenderer.SetBlendShapeWeight(0, 0);
				arrowInstantiated = false;
				Destroy(go);
			}
		}
	}

	else if (DataHolder.playerState_Aiming == false && arrowReleased == false && arrowInstantiated == true)
	{
		arrowInstantiated = false;
		meshRenderer.SetBlendShapeWeight(0, 0);
		anim.SetTrigger("HideArrow");
		Destroy(go);
	}
	/*else
	{
		//temp
		//transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
	}*/
	//Debug.Log(bowCoolDown + "   " + arrowCoolDown);
/*}

private void LateUpdate()
{
	if (DataHolder.playerState_Aiming == true)
	{
		chest.rotation = cam.transform.rotation;
		head.rotation = cam.transform.rotation;

		chest.rotation *= Quaternion.Euler(boneAimOffset); //temp
		head.rotation *= Quaternion.Euler(-boneAimOffset * 0.75f); //temp
	}
}

public void AddAmmo()
{
	currentAmmo++;
	UIController.AmmoChange(currentAmmo, maxAmmo);
	anim.SetTrigger("PickUpArrow");
}

public override void Aim()
{
	base.Aim();
}

public override void StopAim()
{
	base.StopAim();
}*/
