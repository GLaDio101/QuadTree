using System;
using Project.Model.SimulationConfig;
using Project.QueadTree;
using strange.extensions.mediation.impl;
using Unity.Entities;
using UnityEngine;

namespace Project.Manager.SimulationManager
{
    public class SimulationManagerView : EventView
    {
        private EntityManager _entityManager;
        public Entity SimulationEntity;
        public SimulationConfigVo ConfigVo;

        protected override void Start()
        {
            base.Start();
            var simulationData = new SimulationData
            {
                WorldSize = ConfigVo.WorldSize,
                MaxBoxCount = ConfigVo.MaxBoxCount
            };
            _entityManager = World.Active.EntityManager;

            var entityArchetype = _entityManager.CreateArchetype(
                typeof(SimulationConfig), typeof(SimulationData)
            );

            SimulationEntity = _entityManager.CreateEntity(entityArchetype);
            _entityManager.SetComponentData(SimulationEntity, simulationData);
        }


        private void OnDrawGizmos()
        {
            if (_entityManager == null)
                return;

            if (!ConfigVo.DrawGizmos)
                return;

            var sharedComponentData = _entityManager.GetSharedComponentData<SimulationConfig>(SimulationEntity);
            sharedComponentData.QuadTree?.DrawGizmos();
            sharedComponentData.CollisionSystem?.DrawGizmos();
        }
    }

    public struct SimulationConfig : ISharedComponentData, IEquatable<SimulationConfig>
    {
        public QuadTree QuadTree;
        public CollisionSystemQuadTree CollisionSystem;

        public bool Equals(SimulationConfig other)
        {
            return QuadTree == other.QuadTree && CollisionSystem == other.CollisionSystem;
        }

        /// <summary>
        /// A representative hash code.
        /// </summary>
        /// <returns>A number that is guaranteed to be the same when generated from two objects that are the same.</returns>
        public override int GetHashCode()
        {
            int hash = 0;
            if (!ReferenceEquals(QuadTree, null)) hash ^= QuadTree.GetHashCode();
            if (!ReferenceEquals(CollisionSystem, null)) hash ^= CollisionSystem.GetHashCode();

            return hash;
        }
    }

    public struct SimulationData : IComponentData
    {
        public Vector2 WorldSize;
        public int LiveBoxCount;
        public int MaxBoxCount;
    }
}