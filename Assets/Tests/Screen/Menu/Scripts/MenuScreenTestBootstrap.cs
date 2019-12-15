#if UNITY_EDITOR 
using JetBrains.Annotations;
using strange.extensions.context.impl;
using Tests.Screen.Menu.Scripts;
using UnityEngine;

namespace Assets.Tests.Screen.Menu.Scripts
{
    public class MenuScreenTestBootstrap : ContextView
    {
        [UsedImplicitly]
        private void Awake()
        {
			GameObject obj = GameObject.Find("ScreenContainer");
            if (obj != null)
            {
                var count = obj.transform.childCount;
                for (var i = 0; i < count; i++)
                {
                    DestroyImmediate(obj.transform.GetChild(0).gameObject);
                }
            }

            //Instantiate the context, passing it this instance.
            context = new MenuScreenTestContext(this);
        }
    }
}
#endif