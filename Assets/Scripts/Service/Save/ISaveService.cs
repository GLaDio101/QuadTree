using UnityEngine;

namespace Service.Save
{
    public interface ISaveService
    {
        void LoadData();

        T GetData<T>();

        void ShowSaveManager();

        void SaveGame(object savedData);

        void SaveGame(object savedData, Texture2D icon);

        void SetTemplate(object data);

        string Filename { get; }

        void Clear();
    }
}