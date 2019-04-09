using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Keeps track of the player */

public class CameraMenager : MonoBehaviour
{
    #region Singleton

    public static CameraMenager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public Camera cam;
}