using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class WeaponContainer : MonoBehaviour
{
    public Sensor sensor;
    public int weaponCount;
    private FirstPersonController firstPersonController;
    
    private void Start()
    {
        
        firstPersonController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        sensor.SensorTriggered.Subscribe(CollectSignalDetected).AddTo(this);
    }

    private void Collect()
    {
        if (weaponCount <= 1) return;
        var random = new Random();
        var pickedWeaponId = random.Next(weaponCount);
        
        firstPersonController.ChangeWeapon(pickedWeaponId);   
        
        Debug.Log("You have picked the weapon with the id: " + pickedWeaponId);
    }

    void CollectSignalDetected(EventArgs args)
    {
        Collect();
        Destroy(gameObject);
        // gameObject.SetActive(false);
    }

}
