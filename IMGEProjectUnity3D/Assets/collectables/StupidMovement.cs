using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using Vector3 = UnityEngine.Vector3;

/**
 * This is a simple movement script, only for testing purposes.
 */
public class StupidMovement : MonoBehaviour
{
    public float speed;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Time.deltaTime * speed * Vector3.forward, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate( speed * Time.deltaTime * -Vector3.forward, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(speed * Time.deltaTime * Vector3.right, Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(speed * Time.deltaTime * Vector3.left, Space.World);
        }
    }
}
