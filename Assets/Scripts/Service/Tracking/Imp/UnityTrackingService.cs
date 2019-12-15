using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Service.Tracking.Imp
{
    public class UnityTrackingService : ITrackingService
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public void Earn(string source, string currency, float price)
        {
            _data.Clear();
            _data["value"] = price;
            _data["source"] = source;
            Analytics.CustomEvent("Earn_" + currency, _data);
        }

        public void Purchase(string item, string currency, float price)
        {
            _data.Clear();
            _data["value"] = price;
            _data["item"] = item;
            Analytics.CustomEvent("Spent_" + currency, _data);
        }

        public void Transaction(string product, string currency, float price, string receipt, string signature)
        {
            Analytics.Transaction(product, (decimal)price, currency, receipt, signature);
        }

        public void Screen(string name)
        {
            _data.Clear();
            _data["name"] = name;
            Analytics.CustomEvent("Screen", _data);
        }

        public void Event(string category, string action, string label, int value)
        {
            _data.Clear();
            _data["action"] = action;
            _data["label"] = label;
            _data["value"] = value;
            Analytics.CustomEvent(category, _data);
        }

        public void Event(string category, string action, string label)
        {
            _data.Clear();
            _data["action"] = action;
            _data["label"] = label;
            Analytics.CustomEvent(category, _data);
        }

        public void Event(string category, string action)
        {
            _data.Clear();
            _data["action"] = action;
            Analytics.CustomEvent(category, _data);
        }

        public void Error(string name)
        {
            _data.Clear();
            _data["name"] = name;
            Analytics.CustomEvent("Error", _data);
        }

        public void Event(string category, string action, string label, string value)
        {
            _data.Clear();
            _data["action"] = action;
            _data["label"] = label;
            _data["value"] = value;
            Analytics.CustomEvent(category, _data);
        }

        public void Heatmap(string category, Vector3 position)
        {
            //_data.Clear();
            //_data["x"] = position.x;
            //_data["y"] = position.y;
            //_data["z"] = position.z;
            //_data["t"] = Time.timeSinceLevelLoad;
            //Analytics.CustomEvent("Heatmap." + category, _data);
        }
    }
}