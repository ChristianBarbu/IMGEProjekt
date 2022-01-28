using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PistolController : WeaponController
{
    public override void fireShots()
    {
        Instantiate(this.bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
    }
}
