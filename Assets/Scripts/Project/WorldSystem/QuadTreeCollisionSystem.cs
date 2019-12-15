using System.Collections.Generic;
using Project.GameEntity;
using Project.Manager;
using Project.Manager.SimulationManager;
using Project.QueadTree;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Project.WorldSystem
{
    public class QuadTreeCollisionSystem : ComponentSystem
    {
        private QuadTree _quadTree;
        private CollisionSystemQuadTree _csQuad;
        private SimulationData _simulationData;
        private Entity _simulationEntity;
        private EntityQuery _bodyQuery;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            Entities.ForEach((Entity entity, ref SimulationData simulationConfig) =>
            {
                _simulationEntity = entity;
                _simulationData = simulationConfig;
            });

            _quadTree = new QuadTree(new Rect(0, 0, _simulationData.WorldSize.x, _simulationData.WorldSize.y),
                1, 6);
            _quadTree.EntityManager = EntityManager;
            _csQuad = new CollisionSystemQuadTree(_quadTree);
            _csQuad.EntityManager = EntityManager;

            EntityManager.SetSharedComponentData(_simulationEntity, new SimulationConfig()
            {
                QuadTree = _quadTree,
                CollisionSystem = _csQuad
            });

            Entities.ForEach((Entity entity, ref Translation translation) => { _csQuad.AddBody(entity); });
        }

        protected override void OnUpdate()
        {
            _quadTree.Clear();
            _bodyQuery = GetEntityQuery(typeof(BodyComponent));
            _simulationData.LiveBoxCount = _bodyQuery.CalculateEntityCount();

            Entities.ForEach((Entity entity, ref Translation translation) =>
            {
                var bodyComponent = EntityManager.GetSharedComponentData<BodyComponent>(entity);
                bodyComponent.Shape.Center = translation.Value;
                bodyComponent.Pos = translation.Value;

                _quadTree.AddBody(entity);
            });
            _csQuad.Entities = Entities;
            _csQuad.Step();
        }
    }
}