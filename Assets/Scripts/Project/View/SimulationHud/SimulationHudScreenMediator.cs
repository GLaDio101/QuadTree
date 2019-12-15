using Core.Manager.Scene;
using Core.Manager.Screen;
using Project.Enums;
using Project.GameEntity;
using Project.WorldSystem;
using strange.extensions.mediation.impl;
using Unity.Collections;
using Unity.Entities;

namespace Project.View.SimulationHud
{
    public enum SimulationHudScreenEvent
    {
        Menu
    }

    public class SimulationHudScreenMediator : EventMediator
    {
        [Inject] public SimulationHudScreenView view { get; set; }

        [Inject] public ISceneModel sceneModel { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(SimulationHudScreenEvent.Menu, OnMenu);
        }

        private void OnMenu()
        {
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo()
            {
                Name = GameElement.MenuScreen
            });

            sceneModel.Clear(SceneLayer.Middle).Then(layer =>
            {
                World.Active.GetExistingSystem<QuadTreeCollisionSystem>().Enabled = false;
                World.Active.GetExistingSystem<MoveSystem>().Enabled = false;
                World.Active.GetExistingSystem<LifeSystem>().Enabled = false;
                World.Active.GetExistingSystem<ColorEffectSystem>().Enabled = false;
                foreach (var entity in World.Active.EntityManager.GetAllEntities(Allocator.Temp))
                {
                    World.Active.EntityManager.DestroyEntity(entity);
                }
            });
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(SimulationHudScreenEvent.Menu, OnMenu);
        }
    }
}