using UnityEngine;

namespace Service.Tracking
{
    public interface ITrackingService
    {
        void Earn(string source, string currency, float price);

        void Purchase(string item, string currency, float price);

        void Transaction(string product, string currency, float price, string receipt, string signature);

        void Screen(string name);

        void Event(string category, string action, string label, string value);

        void Event(string category, string action, string label, int value);

        void Event(string category, string action, string label);

        void Event(string category, string action);

        void Error(string name);

        void Heatmap(string category, Vector3 position);
    }
}