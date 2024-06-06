using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject buildingPrefab;
    private GameObject currentBuilding;
    private Renderer buildingRenderer;
    private bool isPlacing = false;
    public LayerMask placementLayer;
    private BuildingCollisionDetector collisionDetector;

    void Update()
    {
        if (isPlacing)
        {
            MoveBuildingToMouse();
            UpdateBuildingColor();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
    }

    public void StartPlacing()
    {
        Debug.Log("StartPlacing called");
        if (!isPlacing)
        {
            Debug.Log("Start placing the building");
            isPlacing = true;
            currentBuilding = Instantiate(buildingPrefab);
            buildingRenderer = currentBuilding.GetComponent<Renderer>();
            collisionDetector = currentBuilding.GetComponentInChildren<BuildingCollisionDetector>(); // Get the detector from the Cube
            UpdateBuildingColor(); // Initial update to set the color
        }
    }

    void PlaceBuilding()
    {
        if (IsValidPlacement())
        {
            Debug.Log("Placing the building");
            isPlacing = false;
            SetBuildingColor(Color.white); // Change color to default or fixed color
            currentBuilding = null; // Reset current building
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
            Debug.Log("Valid placement");
            SetBuildingColor(Color.green);
        }
        else
        {
            Debug.Log("Invalid placement");
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
        if (collisionDetector != null)
        {
            Debug.Log("Collision detector state: " + collisionDetector.isColliding);
            if (collisionDetector.isColliding)
            {
                return false;
            }
        }

        // Additional checks can be added here (e.g., terrain suitability)
        return true; // Placeholder: valid if not colliding
    }
}
