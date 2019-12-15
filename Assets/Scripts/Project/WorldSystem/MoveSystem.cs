using Project.GameEntity;
using Project.Manager;
using Project.Manager.SimulationManager;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Project.WorldSystem
{
    [UpdateBefore(typeof(QuadTreeCollisionSystem))]
    public class MoveSystem : ComponentSystem
    {
        private Vector2 _woldSize;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            Entities.ForEach((Entity entity, ref SimulationData simulationConfig) =>
            {
                _woldSize = simulationConfig.WorldSize;
            });
        }

        protected override void OnUpdate()
        {
            Entities.ForEach((ref Translation translation, ref MoveComponent moveSpeedComponent) =>
            {
                translation.Value.z += moveSpeedComponent.MoveSpeedZ * Time.deltaTime;
                translation.Value.x += moveSpeedComponent.MoveSpeedX * Time.deltaTime;

                if (translation.Value.z > _woldSize.y)
                {
                    moveSpeedComponent.MoveSpeedZ = -math.abs(moveSpeedComponent.MoveSpeedZ);
                }

                if (translation.Value.z < 0f)
                {
                    moveSpeedComponent.MoveSpeedZ = +math.abs(moveSpeedComponent.MoveSpeedZ);
                }

                if (translation.Value.x > _woldSize.x)
                {
                    moveSpeedComponent.MoveSpeedX = -math.abs(moveSpeedComponent.MoveSpeedX);
                }

                if (translation.Value.x < 0f)
                {
                    moveSpeedComponent.MoveSpeedX = +math.abs(moveSpeedComponent.MoveSpeedX);
                }

                moveSpeedComponent.Pos.x = translation.Value.x;
                moveSpeedComponent.Pos.y = translation.Value.z;
            });
        }
    }
}