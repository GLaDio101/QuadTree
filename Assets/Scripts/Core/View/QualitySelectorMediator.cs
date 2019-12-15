using Core.Model;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.View
{
    public enum QualitySelectorEvent
    {
        Update
    }

    public class QualitySelectorMediator : EventMediator
    {
        [Inject]
        public QualitySelectorView view { get; set; }

        [Inject]
        public IBasePlayerModel playerModel { get; set; }

        public override void OnRegister()
        {
            view.dispatcher.AddListener(QualitySelectorEvent.Update, OnQuality);
            view.Level = playerModel.Settings.Quality;
        }

        private void OnQuality(IEvent payload)
        {
            var level = playerModel.Settings.Quality;
            if ((int)payload.data == 0)//previous
            {
                level--;
                if (level < 0)
                    level = (short)(view.LevelCount - 1);
            }
            else if ((int)payload.data == 1)//next
            {
                level++;
                if (level == view.LevelCount)
                    level = 0;
            }
            playerModel.Settings.Quality = level;
            view.Level = playerModel.Settings.Quality;
            QualitySettings.SetQualityLevel(level);
        }

        public override void OnRemove()
        {
            view.dispatcher.RemoveListener(LanguageSelectorEvent.Update, OnQuality);
        }
    }
}