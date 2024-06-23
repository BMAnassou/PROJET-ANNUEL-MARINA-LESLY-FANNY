using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManagerSinglePlayer : MonoBehaviour
{
    public static UnitSelectionManagerSinglePlayer Instance { get; set; }
    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();
    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    private Camera cam;
    public LayerMask attackable;
    public bool attackCursorVisible;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;
                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }

        if (unitsSelected.Count > 0 && AtLeastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered with mouse");
                attackCursorVisible = true;
                if (Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    foreach (GameObject unit in unitsSelected)
                    {
                        if (unit != null && unit.GetComponent<AttackController>() != null)
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
                else
                {
                    attackCursorVisible = false;
                }
            }
        }
    }

    private bool AtLeastOneOffensiveUnit(List<GameObject> gameObjects)
    {
        foreach (GameObject unit in gameObjects)
        {
            if (unit != null && unit.GetComponent<AttackController>() != null)
            {
                return true;
            }
        }
        return false;
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        if (unit != null)
        {
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit != null)
            {
                EnableUnitMovement(unit, false);
                TriggerSelectionIndicator(unit, false);
            }
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        if (unit != null && unit.GetComponent<UnitMovementSinglePlayer>() != null)
        {
            unit.GetComponent<UnitMovementSinglePlayer>().enabled = shouldMove;
        }
    }

    private void MultiSelect(GameObject gameObject)
    {
        if (gameObject != null)
        {
            if (!unitsSelected.Contains(gameObject))
            {
                unitsSelected.Add(gameObject);
                TriggerSelectionIndicator(gameObject, true);
                EnableUnitMovement(gameObject, true);
            }
            else
            {
                EnableUnitMovement(gameObject, false);
                TriggerSelectionIndicator(gameObject, false);
                unitsSelected.Remove(gameObject);
            }
        }
    }

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        if (unit != null && unit.transform.childCount > 1)
        {
            unit.transform.GetChild(1).gameObject.SetActive(isVisible);
        }
    }

    internal void DragSelect(GameObject unit)
    {
        if (unit != null && !unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
    }
}
