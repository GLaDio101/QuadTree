using System;
using Unity.Entities;
using UnityEngine;

namespace Project.GameEntity
{
    public struct ColorEffectSharedComponent : ISharedComponentData, IEquatable<ColorEffectSharedComponent>
    {
        public Material Green;
        public Material Red;
        public Material Default;

        public bool Equals(ColorEffectSharedComponent other)
        {
            return Green == other.Green &&
                   Red == other.Red &&
                   Default == other.Default;
        }

        /// <summary>
        /// A representative hash code.
        /// </summary>
        /// <returns>A number that is guaranteed to be the same when generated from two objects that are the same.</returns>
        public override int GetHashCode()
        {
            int hash = 0;
            if (!ReferenceEquals(Green, null)) hash ^= Green.GetHashCode();
            if (!ReferenceEquals(Red, null)) hash ^= Red.GetHashCode();
            if (!ReferenceEquals(Default, null)) hash ^= Default.GetHashCode();
            return hash;
        }
    }
}