using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovementSinglePlayer : MonoBehaviour
{
    private Camera _cam;
    private NavMeshAgent agent;
    public LayerMask ground;
    public bool isCommandedToMove;
    
   
    private void Start()
    {
        _cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        
    }
    
    private void Update()
    {
        
        
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                isCommandedToMove = true;
                agent.SetDestination(hit.point);
            }
        }

        if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
    }

    
}
