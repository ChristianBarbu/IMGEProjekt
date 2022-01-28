using System;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public IObservable<EventArgs> SensorTriggered;
}
