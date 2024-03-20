using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCubeScript : MonoBehaviour

{
    [SerializeField] GameObject field;
    // Start is called before the first frame update
    void Start()
    {
        //Pour désactiver le cube
        //field.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * 0.1f * Time.fixedTime * Input.GetAxis("Vertical"));
        transform.Translate(Vector3.right * 0.1f * Time.fixedTime * Input.GetAxis("Horizontal"));
    }
}
