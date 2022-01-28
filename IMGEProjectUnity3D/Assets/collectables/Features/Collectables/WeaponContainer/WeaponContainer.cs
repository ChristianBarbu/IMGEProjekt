using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = System.Random;

public class WeaponContainer : MonoBehaviour
{
    public Sensor sensor;
    public int weaponCount;

    private void Start()
    {
        sensor.SensorTriggered.Subscribe(CollectSignalDetected).AddTo(this);
    }

    private void Collect()
    {
        if (weaponCount <= 1) return;
        var random = new Random();
        var pickedWeaponId = random.Next(weaponCount);

        Debug.Log("You have picked the weapon with the id: " + pickedWeaponId);
    }

    void CollectSignalDetected(EventArgs args)
    {
        Collect();
        gameObject.SetActive(false);
    }

}
