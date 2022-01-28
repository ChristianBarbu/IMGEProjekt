using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableTypes{Rockets,Shield,Speedbooster,Scorebooster,ZA_WARUDO,Jetpack}
public class ConsumableObject : MonoBehaviour
{
    [SerializeField] private ConsumableTypes type;
    [SerializeField] private int secondsForTimer;
    
    //for the other codes that need an effect
    public bool timerBoolean = false;

    //every consumable that has a timer will always use a boolean that will be turned on and off
    //by a coroutine
    
    public void UseConsumable()
    {
        switch (type)
        {
            case ConsumableTypes.Rockets:
                //code where rocket gets shot;
                break;
            case ConsumableTypes.Shield:
                secondsForTimer = 0;
                //start coroutine timer and set a shield with that certain boolean as parameter
                // if boolean is false after set time, then the shield will not be there any more
                // shield my only be visual UI and block of any damage
                //StartCoroutine(Timer(secondsForTimer,ref timerBoolean));
                break;
            case ConsumableTypes.Scorebooster:
                //timer same gimmick with shield.
                //will boost the score by 3 for set time
                break;
            case ConsumableTypes.Speedbooster:
                //again same type with scorebooster
                //will only increase speed to 1.75 times normal speed.
                break;
            case ConsumableTypes.ZA_WARUDO:
                //name won't change
                //with a coroutine timer idea like above
                //I'll use a variable called STANDO_PAUWA that divides every objects speed besides 
                //the player and their bullets speed and 'reaction' whenever the consumable is being used
                break;
            case ConsumableTypes.Jetpack:
                //no idea how...
                //probably gonna add force to Rigidbody that will gradually increase until it reaches max
                //should have a timer for its usage that will work as long as the jump button is being pressed
                break;
        }
    }

    /*
    private IEnumerable Timer(int seconds, ref bool timerBoolean)
    {
        timerBoolean = true;
        yield return new WaitForSeconds(seconds);
        timerBoolean = false;
    }
    */

    /*
    public void OnTriggerEnter(Collider player)
    {
        player.Consumable = this;
    }
    */
}

/*
 firstPersonControllerInput.UseConsumable.Subscribe(input =>
    {
        if(input)
        {
            UseConsumable();
        }
    }
 */
