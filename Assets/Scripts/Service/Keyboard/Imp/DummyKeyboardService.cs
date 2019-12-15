using System;
using Random = UnityEngine.Random;

namespace Service.Keyboard.Imp
{
    public class DummyKeyboardService:IKeyboardService
    {
        public float Height { get; private set; }

        public bool Visible
        {
            get { return Math.Abs(Height) > .1f; }
        }
        public void Open()
        {
            Height = Random.Range(100,1200);
        }

        public void Close()
        {
            Height = 0;
        }

        public string Text { get; set; }
    }
}