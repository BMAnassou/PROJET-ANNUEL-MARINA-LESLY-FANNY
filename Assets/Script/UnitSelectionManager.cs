using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }
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
                        if (unit.GetComponent<AttackController>())
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
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
            
        }

        return false;
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();
        unitsSelected.Add(unit);
        TriggerSelectionIndicator(unit, true);
        EnableUnitMovement(unit, true);
        
    }
    
   public void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
        }
        groundMarker.SetActive(false);
        unitsSelected.Clear();
    }
    
    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<UnitMovement>().enabled = shouldMove;
    }
    
    private void MultiSelect(GameObject gameObject)
    {
        if (unitsSelected.Contains(gameObject) == false)
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

    private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
    {
        unit.transform.GetChild(1).gameObject.SetActive(isVisible);
    }
    
    internal void DragSelect(GameObject unit)
    {
        if(unitsSelected.Contains(unit) == false){
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
    }
     
}
