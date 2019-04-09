using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class used for all stats where we want to be able to add/remove modifiers */

[System.Serializable]
public class Stat
{

    [SerializeField]
    private int baseValue = 0;  // Starting value
    

    public int GetValue() //returns value
    {
        return baseValue; //temp while theres no perks nor items
    }
}