using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class UnitAttckState : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private AttackController attackController;

    public float stopAttackingDistance = 4.8f;
    
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.setAttackMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
        AnimatorControllerPlayable controller)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();
            agent.SetDestination(attackController.targetToAttack.position);

            var damageToInflict = attackController.unitDamage;
            attackController.targetToAttack.GetComponent<Enemy>().ReceiveDamage(damageToInflict);
            
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            { 
                agent.SetDestination(animator.transform.position);
                animator.SetBool("isAttacking", false);
            }
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
        AnimatorControllerPlayable controller)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex, controller);
    }
}
