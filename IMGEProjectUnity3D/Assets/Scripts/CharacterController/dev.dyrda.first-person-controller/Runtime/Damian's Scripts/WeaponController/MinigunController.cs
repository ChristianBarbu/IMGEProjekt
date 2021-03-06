using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunController : WeaponController
{
    public override void fireShots()
    {
        Instantiate(this.bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        if(this.fireRate < 10)
        {
            this.fireRate += .5f;
        }
    }

    protected override void FixedUpdate()
    {
        if (Time.time > timeBetweenShots)
        {
            if(this.shouldShoot)
            { 
                timeBetweenShots = Time.time + (1 / fireRate);
                fireShots();
            }
            else
            {
                if (this.fireRate > 1)
                {
                    timeBetweenShots = Time.time + (1 / fireRate);
                    this.fireRate -= .5f;
                }
            }
        }
        
    }
}
