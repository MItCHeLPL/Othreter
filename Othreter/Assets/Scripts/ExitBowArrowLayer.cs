using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBowArrowLayer : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
		animator.SetLayerWeight(DataHolder.BowArrowLayerId, 0);
    }
}
