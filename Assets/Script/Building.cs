using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Building : NetworkBehaviour
{
    public GameObject buildingPrefab;
    private GameObject currentBuilding;
    private Renderer buildingRenderer;
    private bool isPlacing = false;
    public LayerMask placementLayer;
    private BuildingCollisionDetector collisionDetector;

    void Update()
    {
        if (!IsOwner) return;
        if (isPlacing)
        {
            MoveBuildingToMouse();
            UpdateBuildingColor();

            if (Input.GetMouseButtonDown(0) && IsValidPlacement())
            {
                PlaceBuildingServerRpc(currentBuilding.transform.position);
            }
        }
    }

    public void StartPlacing()
    {
        if (!isPlacing && IsOwner)
        {
            isPlacing = true;
            currentBuilding = Instantiate(buildingPrefab);
            buildingRenderer = currentBuilding.GetComponent<Renderer>();
            collisionDetector = currentBuilding.GetComponentInChildren<BuildingCollisionDetector>();
            UpdateBuildingColor();
        }
    }

    [ServerRpc]
    void PlaceBuildingServerRpc(Vector3 position, ServerRpcParams rpcParams = default)
    {
        GameObject newBuilding = Instantiate(buildingPrefab, position, Quaternion.identity);
        newBuilding.GetComponent<NetworkObject>().SpawnWithOwnership(rpcParams.Receive.SenderClientId);

        // Notify clients to finalize the placement
        FinalizePlacementClientRpc(newBuilding.GetComponent<NetworkObject>().NetworkObjectId);
    }

    [ClientRpc]
    void FinalizePlacementClientRpc(ulong networkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkObjectId];
        GameObject newBuilding = networkObject.gameObject;

        // Disable placement-related components
        BuildingCollisionDetector detector = newBuilding.GetComponentInChildren<BuildingCollisionDetector>();
        if (detector != null)
        {
            Destroy(detector);
        }

        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Set final color
        }

        if (IsOwner)
        {
            currentBuilding = null; // Reset current building reference
            isPlacing = false;      // Stop placing
        }
    }

    void MoveBuildingToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
        {
            currentBuilding.transform.position = hit.point;
        }
    }

    void UpdateBuildingColor()
    {
        if (IsValidPlacement())
        {
            SetBuildingColor(Color.green);
        }
        else
        {
            SetBuildingColor(Color.red);
        }
    }

    void SetBuildingColor(Color color)
    {
        if (buildingRenderer != null)
        {
            buildingRenderer.material.color = color;
        }
    }

    bool IsValidPlacement()
    {
        // Check if the building is colliding with another building
        if (collisionDetector != null && collisionDetector.isColliding)
        {
            return false;
        }

        // Additional checks can be added here (e.g., terrain suitability)
        return true; // Placeholder: valid if not colliding
    }
}
