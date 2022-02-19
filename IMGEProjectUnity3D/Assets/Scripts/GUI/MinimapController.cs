using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public Transform player;    

    void LateUpdate()
    {
        this.transform.position = new Vector3(player.position.x, player.position.y + 100, player.position.z);
    }
}
