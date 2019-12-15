using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.Testing.Integration
{
    public class IntegrationTestView : MonoBehaviour
    {
        // ReSharper disable once CollectionNeverUpdated.Local
        private List<Interaction> _availableInteractions;

        private List<string> _selectedInteractions;

        [UsedImplicitly]
        private void Start()
        {
            _availableInteractions = new List<Interaction>();
            _selectedInteractions = new List<string>();
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            yield return new WaitForSeconds(1f);

            GetAvailableInteractions();

            if (CheckIfSystemIdle())
                SelectAndApplyInteraction();

            StartCoroutine(Loop());
        }

        private bool CheckIfSystemIdle()
        {
            return true;
        }

        public void GetSelectedInteractions()
        {
            foreach (string interaction in _selectedInteractions)
            {
                Debug.Log(interaction);
            }
        }

        private void SelectAndApplyInteraction()
        {
            if (_availableInteractions.Count == 0)
                return;

            Interaction interaction = _availableInteractions[UnityEngine.Random.Range(0, _availableInteractions.Count)];
            Array enumValues = Enum.GetValues(interaction.EventType);

            object enumValue = null;
            foreach (object value in enumValues)
            {
                if (value.ToString() == interaction.EventName)
                    enumValue = value;
            }

            _selectedInteractions.Add(interaction.Key);

            if (interaction.Parameter != null)
                interaction.Dispatcher.Dispatch(enumValue, CreateRandomParameter(interaction.Parameter));
            else
                interaction.Dispatcher.Dispatch(enumValue);

            GetSelectedInteractions();
        }

        private object CreateRandomParameter(TestParameter parameter)
        {
            if (parameter.ParamType == typeof(bool))
                return UnityEngine.Random.Range(0, 2) == 1 ? true : false;

            return null;
        }

        private void GetAvailableInteractions()
        {
//            EventView[] eventViews = GameObject.FindObjectsOfType<EventView>();
//            _availableInteractions.Clear();
//            foreach (EventView eventView in eventViews)
//            {
//                Type type = eventView.GetType();
//                IntegrationTestable attribute = type.GetCustomAttribute<IntegrationTestable>();
//
//                if (attribute != null)
//                {
//                    if (!attribute.EventType.IsEnum)
//                    {
//                        Debug.LogWarning(type.FullName + " test event is not enum.");
//                        continue;
//                    }
//
//                    FieldInfo[] fields = attribute.EventType.GetFields(BindingFlags.Public | BindingFlags.Static);
//                    for (int i = 0; i < fields.Length; i++)
//                    {
//                        FieldInfo fieldInfo = fields[i];
//                        NonInteractable userInteraction = fieldInfo.GetCustomAttribute<NonInteractable>();
//                        if (userInteraction == null)
//                        {
//                            Interaction interaction = new Interaction()
//                            {
//                                Dispatcher = eventView.dispatcher,
//                                EventName = fieldInfo.Name,
//                                EventType = attribute.EventType,
//                                Parameter = fieldInfo.GetCustomAttribute<TestParameter>(),
//                                Key = type.Name + "_" + attribute.EventType.Name + "_" + fieldInfo.Name
//                            };
//
//                            _availableInteractions.Add(interaction);
//                        }
//                    }
//                }
//            }
        }

        private static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
