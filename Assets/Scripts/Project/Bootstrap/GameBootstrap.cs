using JetBrains.Annotations;
using Project.Config;
using strange.extensions.context.impl;
using UnityEngine;

namespace Project.Bootstrap
{
    public class GameBootstrap : ContextView
    {
        [UsedImplicitly]
        private void Awake()
        {
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
  #endif
            //Instantiate the context, passing it this instance.
            context = new GameContext(this);
        }
    }
}