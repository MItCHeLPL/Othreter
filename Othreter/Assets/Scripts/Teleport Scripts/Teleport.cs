using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	[SerializeField]
	private Vector3 place = default;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") //if colides with player
        {
            col.transform.position = place; //teleport player to spawn
        }
    }
}
