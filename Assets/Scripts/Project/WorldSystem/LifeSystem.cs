using Project.GameEntity;
using Project.Manager;
using Project.Manager.SimulationManager;
using Unity.Collections;
using Unity.Entities;

namespace Project.WorldSystem
{
    [UpdateAfter(typeof(QuadTreeCollisionSystem))]
    public class LifeSystem : ComponentSystem
    {
        private Entity _simulationEntity;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            Entities.ForEach((Entity entity, ref SimulationData simulationConfig) => { _simulationEntity = entity; });
        }

        protected override void OnUpdate()
        {
            var _bodyQuery = GetEntityQuery(typeof(BodyComponent));
            var entityArray = _bodyQuery.ToEntityArray(Allocator.Temp);
            var simulationData = EntityManager.GetComponentData<SimulationData>(_simulationEntity);
            simulationData.LiveBoxCount = entityArray.Length;
            EntityManager.SetComponentData(_simulationEntity, simulationData);
            entityArray.Dispose();


            Entities.ForEach((Entity entity, ref LifeComponent lifeComponent) =>
            {
                if (lifeComponent.Life < 1)
                    EntityManager.DestroyEntity(entity);
            });
        }
    }
}