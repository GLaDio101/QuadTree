using System;
using Unity.Entities;

namespace Project.GameEntity
{
    [Serializable]
    public struct ColorEffectData : IComponentData
    {
        public int EffectType; //0: default 1: spawn 2:damage
        public float Duration;
    }
}