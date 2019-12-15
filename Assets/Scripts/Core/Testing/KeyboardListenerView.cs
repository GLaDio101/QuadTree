using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.Testing
{
    public enum KeyboardEvent
    {
        KeyPressed
    }

    public class KeyboardListenerView : EventView
    {
        private void Update()
        {
            if (!Input.anyKeyDown) return;
            if(Input.inputString != String.Empty)
                dispatcher.Dispatch(KeyboardEvent.KeyPressed,Input.inputString);
        }
    }
}
