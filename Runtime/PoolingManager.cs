using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaBois.Pooling
{
    public class PoolingManager : MonoBehaviour
    {
        private static PoolingManager _instance;
        public static PoolingManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private Dictionary<Component, PoolGroup<Component>> _componentGroups = new Dictionary<Component, PoolGroup<Component>>();

        private void Awake()
        {
            if (_instance)
            {
                if(_instance != this)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            _instance = this;
        }

        public PoolGroup<T> CreatePool<T>(T prefab, bool autoActivate, int limit) where T : Component
        {
            PoolGroup<Component> group = new PoolGroup<Component>(prefab, autoActivate, limit);
            _componentGroups.Add(prefab, group);
            return group as PoolGroup<T>;
        }

        public T Spawn<T>(T prefab) where T : Component
        {
            PoolGroup<Component> group;
            if(!_componentGroups.TryGetValue(prefab, out group))
            {
                group = new PoolGroup<Component>(prefab);
                _componentGroups.Add(prefab, group);
                T pooledObj = group.Get() as T;
                _componentGroups.Add(pooledObj, group);
                return pooledObj;
            }
            else
            {
                T pooledObj = group.Get() as T;
                if (!_componentGroups.ContainsKey(pooledObj))
                {
                    _componentGroups.Add(pooledObj, group);
                }
                return pooledObj;
            }
        }

        public bool Return<T>(T obj) where T : Component
        {
            PoolGroup<Component> group;
            if (_componentGroups.TryGetValue(obj, out group))
            {
                bool returned = group.Return(obj);
                return returned;
            }
            else
            {
                //print("Cannot return: " + obj);
            }

            return false;
        }
    }
}