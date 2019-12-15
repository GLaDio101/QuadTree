using strange.extensions.mediation.impl;
using UnityEngine;

namespace Project.View.Base
{
    public class LoadingScreenMediator : EventMediator
    {
        [Inject]
        public LoadingScreenView view { get; set; }

        public override void OnRegister()
        {
            Resources.UnloadUnusedAssets();
        }

        public override void OnRemove()
        {
        }
    }
}