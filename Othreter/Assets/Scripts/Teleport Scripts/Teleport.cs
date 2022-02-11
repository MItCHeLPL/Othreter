using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
	[SerializeField]
	private Vector3 defaultPlace = Vector3.zero;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") //if colides with player
        {
            col.transform.position = defaultPlace; //teleport player to spawn
        }
    }

    public void TeleportPlayer()
    {
        ObjectsMenager.instance.player.transform.position = defaultPlace;
    }
    public void TeleportPlayer(Vector3 newPosition)
	{
        ObjectsMenager.instance.player.transform.position = newPosition;
    }
    public void TeleportPlayer(Vector3 newPosition, Quaternion newRotation)
    {
        ObjectsMenager.instance.player.transform.position = newPosition;
        ObjectsMenager.instance.player.transform.rotation = newRotation;
    }
    public void TeleportPlayer(Transform newTransform)
    {
        ObjectsMenager.instance.player.transform.position = newTransform.position;
        ObjectsMenager.instance.player.transform.rotation = newTransform.rotation;
    }
}
