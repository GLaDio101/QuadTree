using System;
using System.Collections.Generic;
using Project.QueadTree;
using Project.QueadTree.Shapes;
using Unity.Entities;
using UnityEngine;

namespace Project.GameEntity
{
    [Serializable]
    public struct BodyComponent : ISharedComponentData, IEquatable<BodyComponent>, ICollisionBody, IQuadTreeBody
    {
        public BoxShape Shape;
        public Vector3 Pos;
        public Dictionary<int, ICollisionBody> StayMap;

        #region ICollisionBody

        public int RefId { get; set; }

        public bool Sleeping
        {
            get { return false; }
        }

        public ICollisionShape CollisionShape
        {
            get { return Shape; }
        }

        public void OnCollision(CollisionResult result, ICollisionBody other)
        {
            if (result.Type == CollisionType.Enter)
            {
                if (!StayMap.ContainsKey(other.GetHashCode()))
                {
                    StayMap[other.GetHashCode()] = other;
                }
            }
            else if (result.Type == CollisionType.Exit)
            {
                if (StayMap.ContainsKey(other.GetHashCode()))
                {
                    StayMap.Remove(other.GetHashCode());
                }
            }

//            Debug.Log("OnCollision : " + result.Type);
        }

        #endregion

        #region IQuadTreeBody

        public Vector2 Position
        {
            get { return new Vector2(Pos.x, Pos.z); }
        }

        public bool QuadTreeIgnore
        {
            get { return false; }
        }

        #endregion

        public bool Equals(BodyComponent other)
        {
            return Shape == other.Shape &&
                   Pos == other.Pos;
        }

        /// <summary>
        /// A representative hash code.
        /// </summary>
        /// <returns>A number that is guaranteed to be the same when generated from two objects that are the same.</returns>
        public override int GetHashCode()
        {
            int hash = 0;
            if (!ReferenceEquals(Shape, null)) hash ^= Shape.GetHashCode();
            hash ^= Pos.GetHashCode();
            return hash;
        }
    }
}