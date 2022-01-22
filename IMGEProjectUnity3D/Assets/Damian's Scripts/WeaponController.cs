using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    public GameObject bullet;

    private bool shouldShoot = false;

    public void startShooting()
    {
        this.shouldShoot = true;
    }
    public void stopShooting()
    {
        this.shouldShoot = false;
    }

    public abstract void fireShots();

    private void Update()
    {
        if(this.shouldShoot)
        {
            fireShots();
        }
    }

}

/* Shootinputstream wird geraised ruft shoot methode auf welche dann

firstPersonControllerInput.Shoot.Subscribe(input->
        {
    if (Input)
    {
        this.weapon.startShooting();
    }
    else
    {
        this.weapon.stopShooting();
    }


})*/