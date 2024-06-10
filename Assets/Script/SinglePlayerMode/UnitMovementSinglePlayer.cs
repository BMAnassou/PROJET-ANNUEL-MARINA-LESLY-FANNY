using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovementSinglePlayer : MonoBehaviour
{
    private Camera _cam;
    private NavMeshAgent _agent;
    public LayerMask ground;
    public bool isCommandedToMove;
    
   
    private void Start()
    {
        _cam = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        
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
                _agent.SetDestination(hit.point);
            }
        }

        if (_agent.hasPath == false || _agent.remainingDistance <= _agent.stoppingDistance)
        {
            isCommandedToMove = false;
        }
    }

    
}
