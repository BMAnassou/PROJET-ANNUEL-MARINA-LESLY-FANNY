using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{

    public static BuildingPlacer instance;
    public LayerMask groundLayerMask;
    private GameObject _buildingPrefab;

    private GameObject _toBuild;
    private Camera _mainCamera;
    private Ray _ray;
    private RaycastHit _hit;
    private void Awake()
    {
        instance = this;
        _mainCamera = Camera.main;
        _buildingPrefab = null;
    }

    public void SetBuildingPrefab(GameObject prefab)
    {
        _buildingPrefab = prefab;
        _PrepareBuilding();
    }

    private void Update()
    {
        if (_buildingPrefab != null)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (_toBuild.activeSelf)
                {
                    _toBuild.SetActive(false);
                }
            }else if (!_toBuild.activeSelf)
            {
                _toBuild.SetActive(true);
            }
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if ( Physics.Raycast(_ray, out _hit, 1000f, groundLayerMask))
            {
                if (!_toBuild.activeSelf)
                {
                    _toBuild.SetActive(true);                    
                }
                _toBuild.transform.position = _hit.point;
                if (Input.GetMouseButtonDown(0))
                {
                    BuildingManager m = _toBuild.GetComponent<BuildingManager>();
                    if (m.hasValidPlacement)
                    {
                        m.SetPlacementMode(PlacementMode.Fixed);
                        _buildingPrefab = null;
                        _toBuild = null;
                    }
                }
            }
            else if (_toBuild.activeSelf)
            {
                _toBuild.SetActive(false);
            }
            
        }
    }

    private void _PrepareBuilding()
    {
        if (_toBuild)
        {
            Destroy(_toBuild);
        }
        _toBuild = Instantiate(_buildingPrefab);
        _toBuild.SetActive(false);

        BuildingManager m = _toBuild.GetComponent<BuildingManager>();
        m.isFixed = false;
        m.SetPlacementMode(PlacementMode.Valid);
    }
}
