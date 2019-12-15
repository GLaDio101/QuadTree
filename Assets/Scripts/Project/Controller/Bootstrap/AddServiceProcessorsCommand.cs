using System.Collections.Generic;
using Core.Enums;
using Core.Manager.Screen;
using Project.Enums;
using Project.WorldSystem;
using strange.extensions.command.impl;
using Unity.Entities;
using UnityEngine;

namespace Project.Controller.Bootstrap
{
    public class AddServiceProcessorsCommand : EventCommand
    {
        [Inject] public IScreenModel screenModel { get; set; }

        public override void Execute()
        {
            World.Active.GetExistingSystem<QuadTreeCollisionSystem>().Enabled = false;
            World.Active.GetExistingSystem<MoveSystem>().Enabled = false;
            World.Active.GetExistingSystem<LifeSystem>().Enabled = false;
            World.Active.GetExistingSystem<ColorEffectSystem>().Enabled = false;
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo()
            {
                Name = GameElement.LoadingScreen
            });

            screenModel.IgnoreHistory = new List<string>()
            {
                BaseGameElement.LoadingScreen,
                "ConfirmPanel",
                "AlertPanel",
                "ExitPanel"
            };

#if UNITY_EDITOR
            Application.runInBackground = true;
#endif
        }
    }
}