using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedPlasmaAttack : EnemyAttack
{
    public EnemyPlasmaProjectile projectile;
    public float initForce = 100;
    public Transform spawnPosition;
    public override void Attack(Vector3 forward)
    {
        var p = Instantiate(projectile, spawnPosition.position, Quaternion.Euler(new Vector3(0,0,0)));
        p.AddForce(initForce);
    }


}
