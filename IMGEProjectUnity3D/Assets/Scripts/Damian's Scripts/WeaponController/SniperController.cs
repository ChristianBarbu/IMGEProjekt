using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

public class SniperController : WeaponController
{
    public override void fireShots()
    {
        Instantiate(this.bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        this.stopShooting();
    }
}
