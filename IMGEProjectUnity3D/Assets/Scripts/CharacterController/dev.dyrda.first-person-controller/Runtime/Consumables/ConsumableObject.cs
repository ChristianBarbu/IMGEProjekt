using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableTypes{Rockets,Shield,Speedbooster,Scorebooster,TimeLapse,Jetpack, None}
public class ConsumableObject : MonoBehaviour
{
    public ConsumableTypes type;
    
    public void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<FirstPersonController>() != null)
        {
            player.gameObject.GetComponent<FirstPersonController>().consumable = this.type;
            Destroy(this.gameObject);
        }
    }
    
}
