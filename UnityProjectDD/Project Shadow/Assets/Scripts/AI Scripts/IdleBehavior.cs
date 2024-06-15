using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{
    [SerializeField] private float timeUntilRandom;
    [SerializeField] private int numberOfIdleAnimations;

    private bool isRandom;
    private float idleTime;
    private int idleAnimation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsAttacking")) return;

        if (isRandom == false)
        {
            idleTime += Time.deltaTime;

            if (idleTime > timeUntilRandom && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isRandom = true;
                idleAnimation = Random.Range(1, numberOfIdleAnimations + 1);
                idleAnimation = idleAnimation * 2 - 1;

                animator.SetFloat("IdleAnimation", idleAnimation - 1);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("IdleAnimation", idleAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if(isRandom)
        {
            idleAnimation--;
        }

        isRandom = false;
        idleTime = 0;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
