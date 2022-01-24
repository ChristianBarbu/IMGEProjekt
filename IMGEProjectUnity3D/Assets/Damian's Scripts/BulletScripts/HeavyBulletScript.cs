using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBulletScript : MonoBehaviour
{
    private int EntityCount = 5;
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        //insert Damage Function here
        if (other.tag == "Enemy" && EntityCount != 0)
        {
            EntityCount--;
        }
        else
        {
            Destroy(this);
        }
    }
}
