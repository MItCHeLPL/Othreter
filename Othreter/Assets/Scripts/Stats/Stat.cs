using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class used for all stats where we want to be able to add/remove modifiers */

[System.Serializable]
public class Stat
{
    [SerializeField]
    private int baseValue = 0;
    

    public int GetValue()
    {
        return baseValue;
    }

	public void SetValue(int value)
	{
		baseValue = value;
	}
}