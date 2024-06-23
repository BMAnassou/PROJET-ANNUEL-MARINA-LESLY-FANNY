using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class BuildingSinglePlayer : MonoBehaviour
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

            if (Input.GetMouseButtonDown(0) && IsValidPlacement())
            {
                PlaceBuilding();
            }
        }
    }

    public void StartPlacing()
    {
        if (!isPlacing)
        {
            isPlacing = true;
            currentBuilding = Instantiate(buildingPrefab);
            buildingRenderer = currentBuilding.GetComponent<Renderer>();
            collisionDetector = currentBuilding.GetComponentInChildren<BuildingCollisionDetector>();
            UpdateBuildingColor();
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
        if (collisionDetector != null && collisionDetector.isColliding)
        {
            return false;
        }
        return true;
    }

    void PlaceBuilding()
    {
        isPlacing = false;
        SetBuildingColor(Color.white);
        currentBuilding.layer = LayerMask.NameToLayer("Building");
        currentBuilding = null;
        buildingRenderer = null;
        collisionDetector = null;
    }


}
