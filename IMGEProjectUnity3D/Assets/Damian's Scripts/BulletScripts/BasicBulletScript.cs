using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBulletScript : MonoBehaviour
{
    public GameObject Pew;

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(this.transform.position, Vector3.forward, out hit, Time.deltaTime * 50))
        { 
            this.transform.position = hit.point;
            OnTriggerEnter(hit.collider);
        }
        else
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 50);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //insert damage functionality
        Pew = Instantiate(Pew, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Pew.transform.forward = this.transform.forward * (-1);
        Destroy(this.gameObject);
    }
}
