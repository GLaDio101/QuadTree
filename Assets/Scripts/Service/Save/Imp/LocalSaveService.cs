using System;
using System.Linq;
using UnityEngine;

namespace Service.Save.Imp
{
    public class LocalSaveService : AbstractSaveService
    {
        public sealed override void LoadData()
        {
            if (_templateData == null)
            {
                throw new InvalidOperationException("You have to set template data first!");
            }

            if (PlayerPrefs.HasKey(Application.identifier))
            {
                var str = PlayerPrefs.GetString(Application.identifier);
                _tempData = Convert.FromBase64String(str);
            }
            else
            {
                _tempData = _templateData.ToArray();
                PlayerPrefs.SetString(Application.identifier, Convert.ToBase64String(_tempData));
            }

            dispatcher.Dispatch(SaveEvent.DataReady);
        }

        public sealed override void ShowSaveManager()
        {

        }

        public sealed override void SaveGame(object savedData)
        {
            _tempData = ToByteArray(savedData);
            PlayerPrefs.SetString(Application.identifier, Convert.ToBase64String(_tempData));
            dispatcher.Dispatch(SaveEvent.Saved);
        }

        public sealed override void SaveGame(object savedData, Texture2D icon)
        {
            SaveGame(savedData);
        }

        public sealed override void Clear()
        {
            PlayerPrefs.DeleteKey(Application.identifier);
            dispatcher.Dispatch(SaveEvent.Cleared);
        }
    }
}