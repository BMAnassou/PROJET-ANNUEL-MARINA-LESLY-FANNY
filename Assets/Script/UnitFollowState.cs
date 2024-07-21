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

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        attackController.setFollowMaterial();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);
            Debug.Log("No target to attack, stopping follow state.");
        }
        else
        {
            if (animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
            {
                agent.SetDestination(attackController.targetToAttack.position);
                Debug.Log("Following target: " + attackController.targetToAttack.name);
                
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
                if (distanceFromTarget < attackingDistance)
                {
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);
                    Debug.Log("Entering attack state.");
                }
            }
        }
    }
}