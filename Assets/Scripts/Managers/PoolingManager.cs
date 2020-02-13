using System.Collections.Generic;
using MynetDemo.Core;
using UnityEngine;

namespace MynetDemo.Manager
{
    public class PoolingManager : SingletonComponent<PoolingManager>
    {
        readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();

        GameObject Create(GameObject obj)
        {
            GameObject newObject = Instantiate(obj);
            newObject.name = obj.name;
            return newObject;
        }

        void AssignTransform(Transform obj, Transform parent, Vector3 position, Quaternion rotation)
        {
            obj.SetParent(parent);
            obj.position = position;
            obj.rotation = rotation;
        }

        public void Add(GameObject obj)
        {
            obj.transform.SetParent(transform);

            if (!_pool.ContainsKey(obj.name))
            {
                _pool.Add(obj.name, new Queue<GameObject>());
            }

            _pool[obj.name].Enqueue(obj);

            obj.SetActive(false);
        }

        public GameObject Get(GameObject obj, Transform parent, Vector3 position, Quaternion rotation)
        {
            GameObject newObject;

            if (!_pool.ContainsKey(obj.name))
            {
                newObject = Create(obj);
            }
            else if (_pool[obj.name].Count == 0)
            {
                newObject = Create(obj);
            }
            else
            {
                newObject = _pool[obj.name].Dequeue();
                newObject.SetActive(true);
            }

            AssignTransform(obj.transform, parent, position, rotation);

            return newObject;
        }
    }
}
