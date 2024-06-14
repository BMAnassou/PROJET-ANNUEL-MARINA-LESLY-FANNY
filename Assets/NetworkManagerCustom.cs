using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerCustom : MonoBehaviour
{
    private Dictionary<ulong, bool> connectedClients = new Dictionary<ulong, bool>();

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!connectedClients.ContainsKey(clientId))
        {
            connectedClients.Add(clientId, true);
            Debug.Log("Client connected: " + clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (connectedClients.ContainsKey(clientId))
        {
            connectedClients.Remove(clientId);
            Debug.Log("Client disconnected: " + clientId);
        }
    }

    public bool IsClientConnected(ulong clientId)
    {
        return connectedClients.ContainsKey(clientId) && connectedClients[clientId];
    }
}

