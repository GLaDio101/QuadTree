using UnityEngine;

namespace Project.QueadTree
{
    public struct CollisionResult
    {
        public bool Collides;
        public Vector3 Normal;
        public float Penetration;
        public CollisionType Type;
        public bool First;
    }
}

