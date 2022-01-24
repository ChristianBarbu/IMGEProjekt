using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBulletScript : MonoBehaviour
{
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
    }

    private void OnTriggerEnter(Collider other)
    {
        //insert Damage Function here
        Destroy(this);
    }
}
