using Core.Model;
using strange.extensions.mediation.impl;

namespace Core.View.Volume
{
    public class EffectsControlMediator : EventMediator
    {
        [Inject]
        public IVolumeControlView view { get; set; }

        [Inject]
        public IBasePlayerModel playerModel { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(VolumeControlEvent.Update, OnLevelUpdate);

            view.Level = playerModel.Settings.Effects;

            OnLevelUpdate();
        }

        public float GetMasterLevel()
        {
            float value;
            bool result = view.Mixer.GetFloat("SfxVol", out value);
            if (result)
            {
                return value;
            }
            else
            {
                return 0f;
            }
        }

        private void OnLevelUpdate()
        {
            view.Mixer.SetFloat("SfxVol", view.LinearToDecibel(view.Level));
            playerModel.Settings.Effects = view.Level;
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(VolumeControlEvent.Update, OnLevelUpdate);
        }
    }
}