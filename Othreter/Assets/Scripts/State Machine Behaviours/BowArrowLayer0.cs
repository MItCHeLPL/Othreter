using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BowArrowLayer0 : StateMachineBehaviour
{
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		animator.SetLayerWeight(DataHolder.BowArrowLayerId, 0);
	}
}
