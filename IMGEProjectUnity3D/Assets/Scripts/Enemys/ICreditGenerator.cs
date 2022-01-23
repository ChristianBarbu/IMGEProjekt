using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface ICreditGenerator
{
    ReadOnlyReactiveProperty<double> Credits { get; }
}
