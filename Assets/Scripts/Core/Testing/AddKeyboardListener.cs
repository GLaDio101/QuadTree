using Core.Localization;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Core.Testing
{
    public class AddKeyboardListener : EventCommand
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        public override void Execute()
        {
            GameObject listener = new GameObject("KeyboardListener");
            listener.AddComponent<KeyboardListenerView>();
            listener.transform.SetParent(ContextView.transform);

            commandBinder.Bind("l").To<ChangeLanguageCommand>();
        }
    }
}