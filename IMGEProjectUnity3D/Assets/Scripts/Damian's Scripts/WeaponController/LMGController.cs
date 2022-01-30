using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using weapon;

public class LMGController : WeaponController
{
    public override void fireShots()
    {
        Instantiate(this.bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }
}
