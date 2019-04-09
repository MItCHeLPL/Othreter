using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollider : MonoBehaviour
{
    private PlayerStats player; //allows to get PlayerStats script

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") //if colides with player
        {
            player = col.GetComponent<PlayerStats>(); //allows to get PlayerStats script
            player.Die(); //kils player (temp)
        }
    }
}
