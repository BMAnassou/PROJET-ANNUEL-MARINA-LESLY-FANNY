using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovement : NetworkBehaviour
{
    private Camera cam;
    private NavMeshAgent agent;
    public LayerMask ground;
    public bool isCommandedToMove;

    [SerializeField] private Transform basePrefab;
    private Transform spawnedObjetTransform;
    
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
        
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message );
        };
    }
    
    private void Update()
    {
        
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test spawn");
            
            spawnedObjetTransform =  Instantiate(basePrefab);
            spawnedObjetTransform.GetComponent<NetworkObject>().Spawn(true);
            // TestClientRpc(new ClientRpcParams{ Send = new ClientRpcSendParams{ TargetClientIds = new List<ulong>{ 1 } } });
            /*
              randomNumber.Value = new MyCustomData
             {
                 _int = 10,
                 _bool = false,
                 message = "All your bases belongs to us lol!"
             };
             */
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Destroy( spawnedObjetTransform.gameObject);
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

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc" + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId );
    }
    
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams )
    {
        Debug.Log("TestClientRpc" );
    }
}
