using System.Collections.Generic;
using System.Linq;
using Core.Manager.Bundle;
using Core.Manager.Scene;
using Core.Manager.Screen;
using Core.Promise;
using Project.Enums;
using strange.extensions.command.impl;

namespace Project.Controller.Base
{
    public class LoadPlayGameCommand : EventCommand
    {
        [Inject] public ISceneModel sceneModel { get; set; }

//        private readonly List<string> BundleList = new List<string>
//        {
//            "level",
////      "shaders",
////      "fonts"
//        };

        [Inject] public IBundleModel bundleModel { get; set; }

        public override void Execute()
        {
//            Retain();
//            Promise<BundleLoadData>
//                .All(BundleList.Select(bundleInfo =>
//                    bundleModel.LoadBundle(bundleInfo, true)))
//                .Done(links =>
//                {
            sceneModel.LoadScene(ContextBundleKey.Level, SceneLayer.Middle);
//                    Release();
//                });
        }
    }
}