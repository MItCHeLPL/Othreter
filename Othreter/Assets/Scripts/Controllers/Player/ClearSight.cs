using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClearSight : MonoBehaviour
{
	[SerializeField]
	private float distanceToPlayer = 2.0f;
	[SerializeField]
	private Material transparentMaterial = default;
	[SerializeField]
	private int antiShootLayer = 0;
	[SerializeField]
	private LayerMask layerMask = default;

	private CinemachineFreeLook freeLook;
	private CinemachineBrain brain;
	private Camera cam;

	private Vector3 RayOrigin;

	private void Start()
	{
		cam = ObjectsMenager.instance.cam;
		freeLook = GetComponent<CinemachineFreeLook>();
		brain = cam.gameObject.GetComponent<CinemachineBrain>();
	}

	private void Update()
	{
		if((Object)brain.ActiveVirtualCamera == freeLook)
		{
			RayOrigin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
			RaycastHit hit;
			if (Physics.Raycast(RayOrigin, cam.transform.forward, out hit, distanceToPlayer, layerMask))
			{
				Renderer objectRenderer = hit.collider.GetComponent<Renderer>();

				if (objectRenderer != null)
				{
					AutoTransparent AT = objectRenderer.GetComponent<AutoTransparent>();
					if (AT == null) // if no script is attached, attach one
					{
						AT = objectRenderer.gameObject.AddComponent<AutoTransparent>();
						AT.TransparentMaterial = transparentMaterial;
						AT.newLayer = antiShootLayer;
					}
					AT.BeTransparent(); // get called every frame to reset the falloff
				}
			}
		}
	}
}



/*private void Update()
	{
		RaycastHit[] hits; // you can also use CapsuleCastAll() 
						   // TODO: setup your layermask it improve performance and filter your hits. 
		hits = Physics.RaycastAll(transform.position, transform.forward, DistanceToPlayer);
		foreach (RaycastHit hit in hits)
		{
			Renderer R = hit.collider.GetComponent<Renderer>();
			if (R == null)
			{
				continue;
			}
			// no renderer attached? go to next hit 
			// TODO: maybe implement here a check for GOs that should not be affected like the player
			AutoTransparent AT = R.GetComponent<AutoTransparent>();
			if (AT == null) // if no script is attached, attach one
			{
				AT = R.gameObject.AddComponent<AutoTransparent>();
				AT.TransparentMaterial = TransparentMaterial;
			}
			AT.BeTransparent(); // get called every frame to reset the falloff
		}*/
