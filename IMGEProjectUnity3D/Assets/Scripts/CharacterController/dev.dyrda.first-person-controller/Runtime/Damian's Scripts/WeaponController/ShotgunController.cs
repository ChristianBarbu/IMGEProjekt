using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : WeaponController
{
    public override void fireShots()
    {
        for(int numberOfShots = 0; numberOfShots < 15; numberOfShots++)
        {
            GameObject bullet = Instantiate(this.bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
            bullet.transform.Rotate(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0));
        }

        this.stopShooting();
    }
}
