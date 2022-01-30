using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableTypes{Rockets,Shield,Speedbooster,Scorebooster,TimeLapse,Jetpack}
public class ConsumableObject : MonoBehaviour
{
    public ConsumableTypes type;
    [SerializeField] private int secondsForTimer;
    

    public void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<FirstPersonController>() != null)
        {
            player.gameObject.GetComponent<FirstPersonController>().consumable = this;
        }
    }
    
}
