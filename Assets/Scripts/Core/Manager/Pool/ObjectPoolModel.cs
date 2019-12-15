using System.Collections.Generic;
using strange.extensions.context.api;
using UnityEngine;

namespace Core.Manager.Pool
{
    public class ObjectPoolModel : IObjectPoolModel
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        private Dictionary<string, ObjectPoolVo> _poolVos;

        private Dictionary<string, Queue<GameObject>> _objectQueues;

        private GameObject container;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _poolVos = new Dictionary<string, ObjectPoolVo>();
            _objectQueues = new Dictionary<string, Queue<GameObject>>();
            container = new GameObject("PoolObjects");
            container.transform.SetParent(contextView.transform);
        }

        public void Pool(string key, GameObject template, int count, int sleepPadding = 1)
        {
            if (_poolVos.ContainsKey(key))
            {
                Debug.LogWarning("Already have " + key);
                return;
            }

            var vo = new ObjectPoolVo
            {
                Key = key,
                Count = count,
                Template = template
            };

            _poolVos.Add(vo.Key, vo);
            var queue = new Queue<GameObject>();
            _objectQueues.Add(vo.Key, queue);

            for (var i = 0; i < vo.Count; i++)
            {
                var obj = GameObject.Instantiate(vo.Template);
                obj.name = vo.Key;
                obj.transform.SetParent(container.transform);
                obj.transform.localPosition = Vector3.one * -5 * sleepPadding;
                obj.transform.localEulerAngles = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
        }

        public void HideAll()
        {
            foreach (KeyValuePair<string, Queue<GameObject>> pair in _objectQueues)
            {
                foreach (GameObject gameObject in pair.Value)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public GameObject Get(string key)
        {
            if (!_objectQueues.ContainsKey(key))
                return null;

            if (_objectQueues[key].Count == 0)
            {
                Debug.LogWarning("No object in pool with key " + key + ". Increase pool count.");
                return null;
            }
            var newObj = _objectQueues[key].Dequeue();
            newObj.SetActive(true);
            return newObj;
        }

        public void Return(string key, GameObject used, int sleepPadding = 1)
        {
            if (!_objectQueues.ContainsKey(key))
                return;

            var poolables = used.GetComponents<IPoolable>();
            foreach (IPoolable poolable in poolables)
            {
                poolable.Sleep();
            }

            used.transform.SetParent(container.transform);
            used.transform.localPosition = Vector3.one * -5 * sleepPadding;
            used.transform.localEulerAngles = Vector3.zero;
            used.transform.localScale = Vector3.one;
            used.SetActive(false);
            _objectQueues[key].Enqueue(used);
        }

        public bool Has(string key)
        {
            return _poolVos.ContainsKey(key);
        }
    }
}