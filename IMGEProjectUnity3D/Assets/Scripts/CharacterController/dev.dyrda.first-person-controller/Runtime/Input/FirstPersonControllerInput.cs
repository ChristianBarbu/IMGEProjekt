using System;
using UniRx;
using UnityEngine;

    public abstract class FirstPersonControllerInput : MonoBehaviour
    {
        /// <summary>
        ///     Move axes in WASD / D-Pad style.
        ///     Interaction type: continuous axes.
        /// </summary>
        public abstract IObservable<Vector2> Move { get; }

        /// <summary>
        ///     Jump button.
        ///     Interaction type: Trigger.
        /// </summary>
        public abstract IObservable<Unit> Jump { get; }

        public abstract IObservable<Unit> Crouch { get; }

        /// <summary>
        ///     Run button.
        ///     Interaction type: Toggle.
        /// </summary>
        public abstract ReadOnlyReactiveProperty<bool> Run { get; }

        public abstract ReadOnlyReactiveProperty<bool> Jump2 { get; }

        /// <summary>
        ///     Look axes following the free look (mouse look) pattern.
        ///     Interaction type: continuous axes.
        /// </summary>
        public abstract IObservable<Vector2> Look { get; }


        public abstract ReadOnlyReactiveProperty<bool> Shoot { get; }

        public abstract ReadOnlyReactiveProperty<bool> UseAbility { get; }
    }


