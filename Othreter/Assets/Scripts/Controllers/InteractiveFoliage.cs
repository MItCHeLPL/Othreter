using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveFoliage : MonoBehaviour
{
    [SerializeField]private Material[] materials;

    void Start()
    {
        StartCoroutine(WriteToMaterial());
    }

    private IEnumerator WriteToMaterial()
	{
        while(true)
		{
            //for(int i=0; i<materials.Length; i++)
            foreach(Material mat in materials)
			{
                mat.SetVector("_position", transform.position);
			}

            yield return null;
		}
	}
}
