using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class EnemyAttackController: MonoBehaviour
    {

            public Transform targetToAttack;
        
            public int unitDamage;
            public bool isEnemy;
            private void OnTriggerEnter(Collider other)
            {
                if (isEnemy && other.CompareTag("Player") && targetToAttack == null)
                {
                    targetToAttack = other.transform;
                }
        
                
            }

            private void OnTriggerExit(Collider other)
            {
                if (isEnemy && other.CompareTag("Player") && targetToAttack != null)
                {
                    targetToAttack = null;
                }
            }
    }

    
