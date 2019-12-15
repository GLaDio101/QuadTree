using System;
using System.Runtime.Serialization;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;

namespace Service.Save
{
    public abstract class AbstractSaveService : ISaveService
    {
        [Inject(ContextKeys.CONTEXT_DISPATCHER)]
        public IEventDispatcher dispatcher { get; set; }

        protected byte[] _templateData;

        protected byte[] _tempData;

        public void SetTemplate(object data)
        {
            _templateData = ToByteArray(data);
        }

        public abstract void LoadData();

        public abstract void ShowSaveManager();

        public abstract void SaveGame(object savedData);

        public abstract void SaveGame(object savedData, Texture2D icon);

        public string Filename
        {
            get { return Application.identifier; }
        }

        protected byte[] ToByteArray(object data)
        {
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            using (var stream = new System.IO.MemoryStream())
            {
                formatter.Serialize(stream, data);
                return stream.ToArray();
            }
        }

        protected object FromByteArray(byte[] array)
        {
            using (var stream = new System.IO.MemoryStream(array))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                try
                {
                    return formatter.Deserialize(stream);
                }
                catch (Exception)
                {
                    throw new SerializationException("Keep backing fields!");
                }
            }
        }

        public abstract void Clear();

        public T GetData<T>()
        {
            if (_templateData == null)
            {
                throw new InvalidOperationException("You have to set template data first!");
            }

            T loadedData;
            if (!(_tempData == null || _tempData.Length == 0))
            {
                try
                {
                    loadedData = (T)FromByteArray(_tempData);
                    return loadedData;
                }
                catch (Exception e)
                {
                    Debug.LogError("AbstractBackEndService::GetData > Data Lost. " + e.Message);
                    loadedData = (T)FromByteArray(_templateData);
                    return loadedData;
                }
            }

            return (T)FromByteArray(_templateData);
        }
    }
}