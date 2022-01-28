using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * Copied from 'Ohi Ohi Dungeon' project.
 */
public class SensorEventArgs : EventArgs
{
    public readonly Collision associatedCollision;

    public SensorEventArgs(Collision collision) : base()
    {
        associatedCollision = collision;
    }
}
