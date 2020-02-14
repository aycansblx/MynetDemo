using System.Collections.Generic;
using MynetDemo.Core;
using UnityEngine;

namespace MynetDemo.Manager
{
    /// <summary>
    /// A simple pooling manager especially for arrows...
    /// </summary>
    public class PoolingManager : SingletonComponent<PoolingManager>
    {
        [SerializeField] Transform _activeObjectsParent;

        readonly Dictionary<string, Queue<GameObject>> _pool = new Dictionary<string, Queue<GameObject>>();

        /// <summary>
        /// Instantiates a new game object.
        /// </summary>
        /// <param name="obj">The object type to be instantiated</param>
        /// <returns></returns>
        GameObject Create(GameObject obj)
        {
            GameObject newObject = Instantiate(obj);
            newObject.name = obj.name;
            return newObject;
        }

        /// <summary>
        /// Assigns position and rotation to an activated game object.
        /// Also sets the parent. We want all active objects under the same parent...
        /// </summary>
        /// <param name="obj">Transform of the game object which is just activated.</param>
        /// <param name="position">World position.</param>
        /// <param name="rotation">Rotation.</param>
        void AssignTransform(Transform obj, Vector3 position, Quaternion rotation)
        {
            obj.SetParent(_activeObjectsParent);
            obj.position = position;
            obj.rotation = rotation;
        }

        /// <summary>
        /// Adds a game object to pool.
        /// Use this instead of Destroy().
        /// </summary>
        /// <param name="obj">Game object to be added to pool.</param>
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

        /// <summary>
        /// Gets a game object from the pool.
        /// </summary>
        /// <param name="obj">The object type we want to get.</param>
        /// <param name="position">World position.</param>
        /// <param name="rotation">Rotation</param>
        /// <returns>A game object which is just activated.</returns>
        public GameObject Get(GameObject obj, Vector3 position, Quaternion rotation)
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

            AssignTransform(newObject.transform, position, rotation);

            return newObject;
        }
    }
}
