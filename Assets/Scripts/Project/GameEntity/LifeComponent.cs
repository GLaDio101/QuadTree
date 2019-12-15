using System;
using Unity.Entities;

namespace Project.GameEntity
{
    [Serializable]
    public struct LifeComponent : IComponentData
    {
        public int Life;
    }
}