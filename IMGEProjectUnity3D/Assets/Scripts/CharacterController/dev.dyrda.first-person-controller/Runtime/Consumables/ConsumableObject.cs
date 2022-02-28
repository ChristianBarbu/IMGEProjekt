using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableTypes{/*Rockets,Shield,Scorebooster,*/ Speedbooster, TimeLapse, Jetpack, None}
public class ConsumableObject : MonoBehaviour
{
    public ConsumableTypes type;

    
    
    public void Awake()
    {
        int rng = UnityEngine.Random.Range(1,4);
        switch (rng)
        {
            case 1:
                this.type = ConsumableTypes.Speedbooster;
                break;
            case 2:
                this.type = ConsumableTypes.TimeLapse;
                break;
            case 3:
                this.type = ConsumableTypes.Jetpack;
                break;

        }
    }


    public void OnTriggerEnter(Collider player)
    {
        if (player.GetComponent<FirstPersonController>() != null)
        {
            player.gameObject.GetComponent<FirstPersonController>().consumable = this.type;
            player.gameObject.GetComponent<FirstPersonController>().ChangeIcon();
            Destroy(this.gameObject);
        }
    }
    
}
