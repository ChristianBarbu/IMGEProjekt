using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public float tick;

    private bool playerInside = false;
    private float waited = 0;
    private void Update()
    {
        if (playerInside)
        {
            waited += Time.deltaTime;
            if(waited >= tick)
            {
                GameData.Instance.DecreaseHealth(1);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            waited = 0;
            playerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerInside = false;
            waited = 0;
        }
    }
}
