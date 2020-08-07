using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
	[SerializeField] private GameObject cloudPrefab;

	[SerializeField] private int cloudLayersAmount = 15;

	[SerializeField] private float scaleCutoff = 2.0f; //scales range on witch scaling can change between layers
	private float scaleDifference; //scale difference between layers

	[SerializeField] private float alphaClippingCutoff = 0.5f; //scales range on witch alphaClipping can change between layers
	private float alphaClippingDifference; //alpha clipping difference between layers

	[Range(0.0f, 1.0f)]
	[SerializeField] private float colorDifferenceCutoff = 0.5f; //cutoff lowest value of color to which layers can fade
	private float colorDifferenceR; //red color difference between layers
	private float colorDifferenceG; //green color difference between layers
	private float colorDifferenceB; //blue color difference between layers


	private Renderer rend;

	private GameObject layer;

	private float wantedAlphaClipping;

	private Color wantedColor; //color from prefab
	private float wantedColorIntensity; //color intensity from prefab

	private float layerColorR;
	private float layerColorG;
	private float layerColorB;
	private float layerColorIntensity;

	private float lastLayerScale;
	private float lastLayerAlphaClipping;

	private void Start()
	{
		rend = cloudPrefab.GetComponent<Renderer>();

		//get base values from cloud prefab
		//Alpha Clipping
		wantedAlphaClipping = rend.sharedMaterial.GetFloat("_alphaClipping"); //greatest layer
		alphaClippingDifference = Mathf.Clamp01(wantedAlphaClipping / (cloudLayersAmount * alphaClippingCutoff));

		//Color
		wantedColor = rend.sharedMaterial.GetColor("_Color2");
		wantedColorIntensity = Mathf.Log((wantedColor.r + wantedColor.g + wantedColor.b) / 3, 2); //HDR Intensity //HDR intensity is 2^IntensityAmount
		wantedColor /= wantedColorIntensity; //remove intensity from wanted color

		colorDifferenceR = (wantedColor.r / cloudLayersAmount) * colorDifferenceCutoff;
		colorDifferenceG = (wantedColor.g / cloudLayersAmount) * colorDifferenceCutoff;
		colorDifferenceB = (wantedColor.b / cloudLayersAmount) * colorDifferenceCutoff;

		//Scale
		scaleDifference = Mathf.Clamp01(1.0f / (cloudLayersAmount * scaleCutoff));

		//copy transform from prefab to generator so clouds will be in wanted place
		transform.position = cloudPrefab.transform.position;
		transform.rotation = cloudPrefab.transform.rotation;
		transform.localScale = cloudPrefab.transform.localScale;		

		//add one "virtual layer" so first layer can have scale 111 etc.
		lastLayerScale = 1.0f + scaleDifference;
		lastLayerAlphaClipping = wantedAlphaClipping - alphaClippingDifference;

		//instantiate first layer outside of for loop so it can have shadows enabled
		LayerInstantioation(0);

		rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided; //enable shadows on the greatest layer

		for (int i = 1; i < cloudLayersAmount; i++)
		{
			LayerInstantioation(i);
		}
	}

	private void LayerInstantioation(int i)
	{
		layer = Instantiate(cloudPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), transform);

		rend = layer.GetComponent<Renderer>();

		//Layer Alpha Clipping
		rend.material.SetFloat("_alphaClipping", Mathf.Clamp01(lastLayerAlphaClipping + alphaClippingDifference));
		lastLayerAlphaClipping = Mathf.Clamp01(lastLayerAlphaClipping + alphaClippingDifference);

		//Layer Color
		layerColorR = wantedColor.r - colorDifferenceR * (cloudLayersAmount - i - 1);
		layerColorG = wantedColor.g - colorDifferenceG * (cloudLayersAmount - i - 1);
		layerColorB = wantedColor.b - colorDifferenceB * (cloudLayersAmount - i - 1);

		layerColorIntensity = wantedColorIntensity - (wantedColorIntensity / cloudLayersAmount * colorDifferenceCutoff) * (cloudLayersAmount - i - 1); //HDR Intensity

		rend.material.SetColor("_Color2", new Color(layerColorR * layerColorIntensity, layerColorG * layerColorIntensity, layerColorB * layerColorIntensity, wantedColor.a));

		//Layer Scale
		layer.transform.localScale = new Vector3(Mathf.Clamp01(lastLayerScale - scaleDifference), Mathf.Clamp01(lastLayerScale - scaleDifference), Mathf.Clamp01(lastLayerScale - scaleDifference));
		lastLayerScale = Mathf.Clamp01(lastLayerScale - scaleDifference);

		//Layer Position
		layer.transform.localPosition = Vector3.zero;

		//Layer Shadows
		layer.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
	}
}
