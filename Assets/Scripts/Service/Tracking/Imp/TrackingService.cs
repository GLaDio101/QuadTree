using System.Collections.Generic;
using UnityEngine;

namespace Service.Tracking.Imp
{
    public class TrackingService : ITrackingService
    {
        private List<ITrackingService> _services;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _services = new List<ITrackingService> {new UnityTrackingService()};
        }


        public void Earn(string source, string currency, float price)
        {
            foreach (var service in _services)
            {
                service.Earn(source, currency, price);
            }
        }

        public void Purchase(string item, string currency, float price)
        {
            foreach (var service in _services)
            {
                service.Purchase(item, currency, price);
            }
        }

        public void Transaction(string product, string currency, float price, string receipt, string signature)
        {
            foreach (var service in _services)
            {
                service.Transaction(product, currency, price, receipt, signature);
            }
        }

        public void Screen(string name)
        {
            if (string.IsNullOrEmpty(name))
                name = string.Empty;

            foreach (var service in _services)
            {
                service.Screen(name);
            }
        }

        public void Event(string category, string action, string label, int value)
        {
            if (string.IsNullOrEmpty(label))
                label = string.Empty;

            foreach (var service in _services)
            {
                service.Event(category, action, label, value);
            }
        }

        public void Event(string category, string action, string label)
        {
            if (string.IsNullOrEmpty(label))
                label = string.Empty;

            foreach (var service in _services)
            {
                service.Event(category, action, label);
            }
        }

        public void Event(string category, string action)
        {
            foreach (var service in _services)
            {
                service.Event(category, action);
            }
        }

        public void Error(string name)
        {
            foreach (var service in _services)
            {
                service.Error(name);
            }
        }

        public void Event(string category, string action, string label, string value)
        {
            if (string.IsNullOrEmpty(label))
                label = string.Empty;

            foreach (var service in _services)
            {
                service.Event(category, action, label, value);
            }
        }

        public void Heatmap(string category, Vector3 position)
        {
            foreach (var service in _services)
            {
                service.Heatmap(category, position);
            }
        }
    }
}