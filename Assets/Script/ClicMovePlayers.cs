using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicMovePlayers : MonoBehaviour
{
    Vector3 newPosition = Vector3.zero;
    public int speed = 5;
    public GameObject Point;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast (ray, out hit))
            {
                newPosition = hit.point;
                //Pour creer une rotation vers le point de destination
                transform.LookAt (hit.point);
                Point.transform.position=new Vector3(newPosition.x,Point.transform.position.y, newPosition.z);
                
            }
        }

        if(newPosition!=Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position,newPosition,speed * Time.deltaTime);
        }

        //Pour activer et desactiver le point cible
        //il y a un petit probleme la pour le moment
        //Il reste activer
        if(transform.position==newPosition || newPosition==Vector3.zero)
        {
            Point.GetComponent<MeshRenderer> ().enabled = false;
        }
        else
        {
            Point.GetComponent<MeshRenderer> ().enabled = true;
        }

    }

    void OnTriggerEnter()
    {
        newPosition = transform.position;
    }

}
