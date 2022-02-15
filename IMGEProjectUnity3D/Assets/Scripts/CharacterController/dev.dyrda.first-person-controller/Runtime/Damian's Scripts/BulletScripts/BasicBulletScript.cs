using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBulletScript : MonoBehaviour
{   
    public GameObject Pew;

    private void Start()
    {
        Invoke(nameof(Death), 7.5f);
    }

    

    void Update()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(this.transform.position, Vector3.forward, out hit, Time.deltaTime * 50, ~10, QueryTriggerInteraction.Ignore))
        { 
            this.transform.position = hit.point;
            OnHitEnter(hit.collider);
        }
        else
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 50);
        }
    }

    private void OnHitEnter(Collider other)
    {
      
        //insert damage functionality
        Pew = Instantiate(Pew, this.gameObject.transform.position, this.gameObject.transform.rotation);
        Pew.transform.forward = this.transform.forward * (-1);
        Debug.Log("the mysterios object is: " + other.name);
        Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

}
