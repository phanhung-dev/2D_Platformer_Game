using UnityEngine;

public class SetComboBehaviour : StateMachineBehaviour
{
    //[SerializeField] private string boolName;
    private float openComboTime = 0.5f;
    private float closeComboTime = 1f;
    private float currentTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationStrings.canCombo, false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = stateInfo.normalizedTime % 1f;

        if (currentTime >= openComboTime && currentTime <= closeComboTime)
        {
            animator.SetBool(AnimationStrings.canCombo, true);
        }
        else
        {
            animator.SetBool(AnimationStrings.canCombo, false);
        }

        if (currentTime < openComboTime)
        {
            animator.ResetTrigger(AnimationStrings.attackTrigger);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(AnimationStrings.canCombo, false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
