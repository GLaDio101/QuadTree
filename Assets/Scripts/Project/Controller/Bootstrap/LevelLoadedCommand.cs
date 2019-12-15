using Core.Manager.Screen;
using Project.Enums;
using Project.WorldSystem;
using strange.extensions.command.impl;
using Unity.Entities;

namespace Project.Controller.Bootstrap
{
    public class LevelLoadedCommand : EventCommand
    {
        public override void Execute()
        {
            World.Active.GetExistingSystem<QuadTreeCollisionSystem>().Enabled = true;
            World.Active.GetExistingSystem<MoveSystem>().Enabled = true;
            World.Active.GetExistingSystem<LifeSystem>().Enabled = true;
            World.Active.GetExistingSystem<ColorEffectSystem>().Enabled = true;
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo()
            {
                Name = GameElement.SimulationHudScreen
            });
        }
    }
}