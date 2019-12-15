using UnityEngine;

namespace Core.Manager.Pool
{
    public interface IObjectPoolModel
    {
        void Pool(string key, GameObject template, int count, int sleepPadding = 1);

        GameObject Get(string key);

        void Return(string key, GameObject used, int sleepPadding = 1);

        void HideAll();

        bool Has(string key);
    }
}