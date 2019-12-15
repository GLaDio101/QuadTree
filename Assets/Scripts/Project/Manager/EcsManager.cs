using System.Collections.Generic;
using Project.GameEntity;
using Project.Manager.SimulationManager;
using Project.QueadTree;
using Project.QueadTree.Shapes;
using strange.extensions.mediation.impl;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Project.Manager
{
    public enum ColorEffectEnum
    {
        Default,
        Spawn,
        Damage
    }

    public class EcsManager : EventView
    {
        public Mesh Mesh;
        public Material DefaultMat;
        public Material RedMat;
        public Material GreenMat;
        public SimulationManagerView SimulationManager;
        private EntityManager _entityManager;
        private EntityArchetype _entityArchetype;

        protected override void Start()
        {
            base.Start();

            _entityManager = World.Active.EntityManager;

            _entityArchetype = _entityManager.CreateArchetype(
                typeof(Translation),
                typeof(RenderMesh),
                typeof(LocalToWorld),
                typeof(MoveComponent),
                typeof(BodyComponent),
                typeof(LifeComponent),
                typeof(ColorEffectSharedComponent),
                typeof(ColorEffectData)
            );
        }

        private void CreateEntity(int count)
        {
            var entities = new NativeArray<Entity>(count, Allocator.Temp);
            _entityManager.CreateEntity(_entityArchetype, entities);

            foreach (var entity in entities)
            {
                _entityManager.SetComponentData(entity, new MoveComponent()
                {
                    MoveSpeedX = Random.Range(1f, 2f),
                    MoveSpeedZ = Random.Range(1f, 2f)
                });

                _entityManager.SetComponentData(entity, new LifeComponent()
                {
                    Life = SimulationManager.ConfigVo.BoxStartLife
                });

                _entityManager.SetComponentData(entity, new Translation()
                {
                    Value = new float3(Random.Range(0f, SimulationManager.ConfigVo.WorldSize.x), 0,
                        Random.Range(0, SimulationManager.ConfigVo.WorldSize.y))
                });

                _entityManager.SetSharedComponentData(entity, new BodyComponent()
                {
                    Pos = _entityManager.GetComponentData<Translation>(entity).Value,
                    Shape = new BoxShape(new Bounds(Vector3.zero, Vector3.one), false),
                    StayMap = new Dictionary<int, ICollisionBody>()
                });

                _entityManager.SetSharedComponentData(entity, new RenderMesh()
                {
                    mesh = Mesh,
                    material = DefaultMat
                });

                _entityManager.SetSharedComponentData(entity, new ColorEffectSharedComponent()
                {
                    Green = GreenMat,
                    Red = RedMat,
                    Default = DefaultMat,
                });

                _entityManager.SetComponentData(entity, new ColorEffectData()
                {
                    EffectType = ColorEffectEnum.Spawn.GetHashCode(),
                    Duration = .7f
                });

                if (SimulationManager.SimulationEntity == Entity.Null) continue;
                if (_entityManager.GetSharedComponentData<SimulationConfig>(SimulationManager.SimulationEntity)
                        .CollisionSystem != null)
                {
                    _entityManager.GetSharedComponentData<SimulationConfig>(SimulationManager.SimulationEntity)
                        .CollisionSystem.AddBody(entity);
                }
            }

            entities.Dispose();
        }

        private void Update()
        {
            if (SimulationManager.SimulationEntity == Entity.Null)
                return;
            var simulationData = _entityManager.GetComponentData<SimulationData>(SimulationManager.SimulationEntity);
            if (simulationData.LiveBoxCount < SimulationManager.ConfigVo.MaxBoxCount)
            {
                CreateEntity(SimulationManager.ConfigVo.MaxBoxCount - simulationData.LiveBoxCount);
            }
        }
    }
}