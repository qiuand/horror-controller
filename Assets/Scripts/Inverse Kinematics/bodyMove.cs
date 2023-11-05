using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script moves the character body forward
public class bodyMove : MonoBehaviour
{
    //Initialize variables
    public GameObject target;
    public float height;
    public LayerMask terrainLayer;
    public float speed = 5;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //Set variables
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Draw ray to get ground position relative to body transform
        Ray ray = new Ray(transform.position, Vector3.down*300);
        Debug.DrawRay(transform.position, Vector3.down*300);
        //If ray hits ground
        if (Physics.Raycast(ray, out RaycastHit info, 50, terrainLayer.value))
        {
            //Set position to a uniform position off the ground
            transform.position = info.point + new Vector3(0, height, 0);
        }
        /*        transform.LookAt(target.transform);*/

        //Move object forward
        transform.Translate(transform.forward * speed*Time.deltaTime);

/*        rb.velocity = new Vector3(0, 0, 0);*/
    }
}
