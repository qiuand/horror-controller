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
    private void OnTriggerEnter(Collider other)
    {
        print(1);
        if (other.gameObject.tag == "Player")
        {
            parent.GetComponent<AttackPoint>().repairing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            parent.GetComponent<AttackPoint>().repairing = false;
/*            parent.GetComponent<AttackPoint>().repairSource.enabled = false;
*/        }
    }
}
