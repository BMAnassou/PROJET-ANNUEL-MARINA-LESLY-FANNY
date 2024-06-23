using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollisionDetector : MonoBehaviour
{
    public bool isColliding = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            isColliding = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            isColliding = false;
        }
    }
}
