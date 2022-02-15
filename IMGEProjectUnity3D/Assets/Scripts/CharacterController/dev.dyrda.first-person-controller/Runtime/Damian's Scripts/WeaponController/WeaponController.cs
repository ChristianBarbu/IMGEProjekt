using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class WeaponController : MonoBehaviour
{
    public GameObject bullet;
    public GameObject bulletSpawn;

    public float fireRate;

    protected float timeBetweenShots = 0;
    protected bool shouldShoot = false;

    public void startShooting()
    {
        this.shouldShoot = true;
    }
    public void stopShooting()
    {
        this.shouldShoot = false;
    }

    public abstract void fireShots();

    protected virtual void FixedUpdate()
    {
        if(this.shouldShoot && Time.time > timeBetweenShots)
        {
            timeBetweenShots = Time.time + (1/fireRate);
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