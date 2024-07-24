using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Components;
using UnityEngine.AI;


public class UnitNetworkController : NetworkBehaviour
{
    private NavMeshAgent agent;
    private NetworkVariable<Vector3> targetPosition = new NetworkVariable<Vector3>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (IsServer)
        {
            targetPosition.OnValueChanged += OnTargetPositionChanged;
        }
    }

    void Update()
    {
        if (IsOwner && agent != null)
        {
            
            agent.SetDestination(targetPosition.Value);
        }
    }

    public void SetTargetPosition(Vector3 position)
    {
        if (IsServer)
        {
            targetPosition.Value = position;
        }
        else
        {
            SubmitTargetPositionRequestServerRpc(position);
        }
    }

    private void OnTargetPositionChanged(Vector3 oldValue, Vector3 newValue)
    {
        agent.SetDestination(newValue);
    }

    [ServerRpc]
    private void SubmitTargetPositionRequestServerRpc(Vector3 position)
    {
        targetPosition.Value = position;
    }
}
