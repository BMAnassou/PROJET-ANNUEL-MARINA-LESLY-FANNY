using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class UnitFollowState : StateMachineBehaviour
{
    private AttackController attackController;
    private NavMeshAgent agent;

    public float attackingDistance = 4f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        attackController.setFollowMaterial();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null)
        {
          animator.SetBool("isFollowing", false);  
        }
        else
        {
            if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
            {
                agent.SetDestination(attackController.targetToAttack.position);
                animator.transform.LookAt(attackController.targetToAttack);

                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
                if (distanceFromTarget < attackingDistance)
                { 
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);
                }
            }
        }

        
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    //{
    //}
}
