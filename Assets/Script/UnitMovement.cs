using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode.Components;
#if UNITY_EDITOR
using UnityEditor;
// This bypases the default custom editor for NetworkTransform
// and lets you modify your custom NetworkTransform's properties
// within the inspector view
[CustomEditor(typeof(UnitMovement), true)]
public class UnitMovementEditor : Editor
{
}
#endif
 
/// <summary>
/// Base authority motion handler that defaults to
/// owner authoritative mode.
/// </summary>

public class UnitMovement : NetworkTransform
{
        public enum AuthorityModes
        {
            Owner,
            Server
        }
    private Camera cam;
    private NavMeshAgent agent;
    public LayerMask ground;
    public bool isCommandedToMove;

    [SerializeField] private Transform basePrefab;
    private Transform spawnedObjetTransform;
    
    public AuthorityModes AuthorityMode = AuthorityModes.Owner;
    
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _int = 56,
        _bool = true,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
    public struct MyCustomData: INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        basePrefab = Resources.Load<Transform>("Prefabs/basePrefab");
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message );
        };
    }
    
    protected override void Update()
    {
        if (!IsOwner)
        {
            return;
        }
    
        if (IsSpawned && IsAuthority())
        {
            AuthorityUpdate();
            return;
        }
        base.Update();
    }
    
    /// <summary>
    /// Is only invoked for the authority, and I went ahead and made it
    /// protected and virtual in the event you wanted to derive from this
    /// class and use it for both player and AI related motion.
    /// Just placed your player input script in here so you can quickly
    /// test the component.
    /// </summary>
    protected virtual void AuthorityUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test spawn");
            
            spawnedObjetTransform = Instantiate(basePrefab);
            var networkObject = spawnedObjetTransform.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.Spawn(true);
            }
            else
            {
                Debug.LogError("Spawned object does not have a NetworkObject component.");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(spawnedObjetTransform.gameObject);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

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
    protected override bool OnIsServerAuthoritative()
    {
        return AuthorityMode == AuthorityModes.Server;
    }
 
    private bool IsAuthority()
    {
        return AuthorityMode == AuthorityModes.Owner ? IsOwner : IsServer;
    }
    
    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc" + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId );
    }
    
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("TestClientRpc");
    }
    
    [Rpc(SendTo.ClientsAndHost)]
    private void TestClientAndHostRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log("Client & Host Rpc" );
    }
    
    
 
}

