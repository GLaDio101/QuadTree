using JetBrains.Annotations;
using Project.Config;
using strange.extensions.context.impl;

namespace Project.Bootstrap
{
    public class LevelBootstrap : ContextView
    {
        [UsedImplicitly]
        private void Awake()
        {
            //Instantiate the context, passing it this instance.
            context = new LevelContext(this);
        }
    }
}