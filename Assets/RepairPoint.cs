using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairPoint : MonoBehaviour
{
    [SerializeField] GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(1);
        if (collision.gameObject.tag == "Player")
        {
             parent.GetComponent<AttackPoint>().repairing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            parent.GetComponent<AttackPoint>().repairing = false;
            parent.GetComponent<AttackPoint>().repairSource.enabled = false;
        }
    }
}
