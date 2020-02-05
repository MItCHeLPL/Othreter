using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTransparent : MonoBehaviour
{
	private Material[] oldMaterials = null;

	public Material TransparentMaterial { get; set; }
	public int newLayer { get; set; }

	private bool shouldBeTransparent = true;

	private int oldLayer;

	public void BeTransparent()
	{
		shouldBeTransparent = true;
	}

	private void Start()
	{
		if (oldMaterials == null)
		{
			// Save the current materials
			oldMaterials = GetComponent<Renderer>().materials;

			oldLayer = gameObject.layer;
			gameObject.layer = newLayer;

			Material[] materialsList = new Material[oldMaterials.Length];

			for (int i = 0; i < materialsList.Length; i++)
			{
				// repalce material with transparent
				materialsList[i] = Object.Instantiate(TransparentMaterial);

				materialsList[i].SetColor("Color_C0F60F0A", oldMaterials[i].GetColor("_BaseColor"));
				materialsList[i].SetTexture("Texture2D_BF917AA2", oldMaterials[i].GetTexture("_BaseColorMap"));
				materialsList[i].SetTexture("Texture2D_B9AD839C", oldMaterials[i].GetTexture("_NormalMap"));
				materialsList[i].SetFloat("Vector1_DAB16CC", oldMaterials[i].GetFloat("_Smoothness"));
				materialsList[i].SetFloat("Vector1_84DC683", oldMaterials[i].GetFloat("_Metallic"));
			}

			// make transparent
			GetComponent<Renderer>().materials = materialsList;
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if (!shouldBeTransparent)
		{
			Destroy(this);
		}

		//The object will start to become visible again if BeTransparent() is not called
		shouldBeTransparent = false;
	}

	private void OnDestroy()
	{
		// restore old materials
		GetComponent<Renderer>().materials = oldMaterials;
		gameObject.layer = oldLayer;
	}
}
