using Project.GameEntity;
using Project.Manager;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Project.WorldSystem
{
    public class ColorEffectSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref ColorEffectData colorEffectData) =>
            {
                colorEffectData.Duration -= 1 * Time.deltaTime;

                var colorEffectSharedComponent =
                    EntityManager.GetSharedComponentData<ColorEffectSharedComponent>(entity);

                var renderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
                if (colorEffectData.Duration < 0) //Default
                {
                    EntityManager.SetSharedComponentData(entity, new RenderMesh()
                    {
                        mesh = renderMesh.mesh,
                        material = colorEffectSharedComponent.Default
                    });
                }
                else
                {
                    if (colorEffectData.EffectType == ColorEffectEnum.Spawn.GetHashCode())
                    {
                        EntityManager.SetSharedComponentData(entity, new RenderMesh()
                        {
                            mesh = renderMesh.mesh,
                            material = colorEffectSharedComponent.Green
                        });
                    }
                    else if (colorEffectData.EffectType == ColorEffectEnum.Damage.GetHashCode())
                    {
                        EntityManager.SetSharedComponentData(entity, new RenderMesh()
                        {
                            mesh = renderMesh.mesh,
                            material = colorEffectSharedComponent.Red
                        });
                    }
                }
            });
        }
    }
}