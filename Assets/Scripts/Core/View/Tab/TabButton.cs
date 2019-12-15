using UnityEngine;
using UnityEngine.Events;

namespace Core.View.Tab
{
    public class TabButton : MonoBehaviour, ITabButton
    {
        public UnityEvent OnActivated;

        public UnityEvent OnDeactivated;

        public string Key { get; private set; }

        private ITabButtonList _list;

        public void Remove()
        {
            _list = null;
            Destroy(gameObject);
        }

        public void Setup(string value, ITabButtonList list)
        {
            _list = list;
            Key = value;
            Deactivate();
        }

        public void OnClicked()
        {
            _list.Selected = Key;
        }

        public void Activate()
        {
            OnActivated.Invoke();
        }

        public void Deactivate()
        {
            OnDeactivated.Invoke();
        }
    }
}