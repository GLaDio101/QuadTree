using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Project.GameEntity
{
    [Serializable]
    public struct MoveComponent : IComponentData
    {
        public float MoveSpeedX;
        public float MoveSpeedZ;
        public float2 Pos;
    }
}