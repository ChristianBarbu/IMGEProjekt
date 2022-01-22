using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : WeaponController
{
    public override void fireShots()
    {
        
        this.stopShooting();
    }
}
