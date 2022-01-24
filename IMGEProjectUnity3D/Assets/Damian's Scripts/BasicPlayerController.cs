using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
    public GameObject weapon;    

    public Input input;

    private void Awake()
    {
       weapon.GetComponent<WeaponController>().startShooting();
    }
}
