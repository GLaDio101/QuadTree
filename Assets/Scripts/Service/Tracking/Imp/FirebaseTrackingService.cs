/*using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

namespace Assets.Scripts.Service.Tracking.Imp
{
    public class FirebaseTrackingService : ITrackingService
    {
        public FirebaseTrackingService()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        }

        public void Earn(string source, string currency, float price)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventEarnVirtualCurrency, new Parameter[]
            {
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, currency),
                new Parameter(FirebaseAnalytics.ParameterValue,price)
            });
        }

        public void Purchase(string item, string currency, float price)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSpendVirtualCurrency, new Parameter[]
            {
                new Parameter(FirebaseAnalytics.ParameterItemName, item),
                new Parameter(FirebaseAnalytics.ParameterVirtualCurrencyName, currency),
                new Parameter(FirebaseAnalytics.ParameterValue, price)
            });
        }

        public void Transaction(string product, string currency, float price, string receipt, string signature)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Product", product),
                new Parameter("Currency", currency),
                new Parameter("Price", price),
                new Parameter("Receipt", receipt),
                new Parameter("Signature", signature)
            };

            FirebaseAnalytics.LogEvent("Transaction", parameters.ToArray());
        }

        public void Screen(string name)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Name", name),
            };

            FirebaseAnalytics.LogEvent("Screen", parameters.ToArray());
        }

        public void Event(string category, string action, string label, int value)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Action", action),
                new Parameter("Label", label),
                new Parameter("Value", value),
            };

            FirebaseAnalytics.LogEvent(category, parameters.ToArray());
        }

        public void Event(string category, string action, string label)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Action", action),
                new Parameter("Label", label),
            };

            FirebaseAnalytics.LogEvent(category, parameters.ToArray());
        }

        public void Event(string category, string action)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Action", action),
            };

            FirebaseAnalytics.LogEvent(category, parameters.ToArray());
        }

        public void Error(string name)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Name", name),
            };

            FirebaseAnalytics.LogEvent("Error", parameters.ToArray());
        }

        public void Event(string category, string action, string label, string value)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("Action", action),
                new Parameter("Label", label),
                new Parameter("Value", value),
            };

            FirebaseAnalytics.LogEvent(category, parameters.ToArray());
        }

        public void Heatmap(string category, Vector3 position)
        {
        }
    }
}*/