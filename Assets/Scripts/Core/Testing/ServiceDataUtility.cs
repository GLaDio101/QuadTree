using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.Testing
{
    public class ServiceDataUtility : MonoBehaviour
    {
        private static List<MockDataSet> MockDataSets;

        public static T GetDataVoByKey<T>(string key)
        {
            MockDataSets = FindObjectsOfType<MockDataSet>().ToList();

            var activeTicketsDataVo = MockDataSets.Find(set => set.Key == key)
                .GetActiveDataVos();
            var response =
                JsonConvert.DeserializeObject<T>(activeTicketsDataVo.TextData.text);

            return response;
        }


        public static string GetDataVoByKey(string key)
        {
            MockDataSets = FindObjectsOfType<MockDataSet>().ToList();
            var mockDataSet = MockDataSets.Find(set => set.Key == key);
            if (mockDataSet == null)
            {
                Debug.LogWarning("Service Key not found in mockdatas Key : " + key);
                return string.Empty;
            }

            var activeTicketsDataVo = mockDataSet.GetActiveDataVos();

            return activeTicketsDataVo.TextData.text;
        }
    }
}