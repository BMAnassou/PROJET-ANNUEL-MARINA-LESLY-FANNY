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
    public float attackRate = 2f;
    private float attackTimer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.setAttackMaterial();
        attackTimer = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            LookAtTarget();
            agent.SetDestination(attackController.targetToAttack.position);

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                agent.SetDestination(animator.transform.position);
                animator.SetBool("isAttacking", false);
                Debug.Log("Stopping attack state, target out of range or null.");
            }
        }
    }

    private void Attack()
    {
        var damageToInflict = attackController.unitDamage;
        if (attackController.targetToAttack != null)
        {
            attackController.targetToAttack.GetComponent<Unit>().TakeDamage(damageToInflict);
            Debug.Log("Attacking target: " + attackController.targetToAttack.name);
        }
    }

    private void LookAtTarget()
    {
        if (attackController.targetToAttack != null)
        {
            Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
