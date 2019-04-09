using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatistics : MonoBehaviour
{
    private int jumps = 0;
    private int crouches = 0;
    private int climbes = 0;

    private PlayerController playerController;

    private void Start()
    {
        playerController = ObjectsMenager.instance.player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if(playerController.jump)
        {
            jumps++;
        }

        if (playerController.crouch)
        {
            crouches++;
        }

        if (playerController.climbingProcess)
        {
            climbes++;
        }
    }
}
