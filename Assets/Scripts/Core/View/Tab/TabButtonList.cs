using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.View.Tab
{
    public class TabButtonList : MonoBehaviour, ITabButtonList
    {
        public GameObject ButtonTemplate;

        public Transform Container;

        public Action OnSelectedChanged { get; set; }

        private string _selected = string.Empty;

        public string Selected
        {
            get { return _selected; }
            set
            {
                if (value == null)
                    return;

                if (_selected == value)
                    return;

                if (!_buttonMap.ContainsKey(value))
                {
                    Debug.LogWarning("TabButtonList>Selected no item with key " + value);
                    return;
                }

                if (_buttonMap.ContainsKey(_selected))
                    _buttonMap[_selected].Deactivate();
                _selected = value;
                _buttonMap[_selected].Activate();
                OnSelectedChanged.Invoke();
            }
        }

        private Dictionary<string, ITabButton> _buttonMap;

        private void Awake()
        {
            _buttonMap = new Dictionary<string, ITabButton>();
        }

        public void SelectTabByIndex(int index)
        {
            var child = Container.GetChild(index);
            child.GetComponent<TabButton>().OnClicked();
        }

        public void Add(string value)
        {
            if (_buttonMap.ContainsKey(value))
            {
                Debug.LogWarning("TabButtonList>Add already added " + value);
                return;
            }

            if (ButtonTemplate == null)
            {
                Debug.LogWarning("TabButtonList>Add button template is null.");
                return;
            }

            GameObject button = Instantiate(ButtonTemplate, Container);
            ITabButton tabButton = button.GetComponent<ITabButton>();

            if (tabButton == null)
            {
                Debug.LogWarning("TabButtonList>Add button template not implements ITabButton.");
                return;
            }

            tabButton.Setup(value, this);
            _buttonMap.Add(value, tabButton);
        }

        public void Remove(string value)
        {
            if (!_buttonMap.ContainsKey(value))
            {
                Debug.LogWarning("TabButtonList>Remove already removed " + value);
                return;
            }

            ITabButton tabButton = _buttonMap[value];
            _buttonMap.Remove(tabButton.Key);
            tabButton.Remove();
        }

        public void DeselectAll()
        {
            _buttonMap[_selected].Deactivate();
            _selected = string.Empty;
            OnSelectedChanged.Invoke();
        }

        public void Clear()
        {
            foreach (string key in _buttonMap.Keys)
            {
                ITabButton tabButton = _buttonMap[key];
                tabButton.Remove();
            }

            _buttonMap.Clear();
        }
    }
}