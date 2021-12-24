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
        private Dictionary<GameObject, PoolGroup<GameObject>> _gameObjectGroups = new Dictionary<GameObject, PoolGroup<GameObject>>();

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

        public PoolGroup<T> CreatePool<T>(T prefab) where T : Component
        {
            PoolGroup<T> group = new PoolGroup<T>(prefab);
            return group;
        }

        public T Spawn<T>(T prefab, bool activate = true) where T : Component
        {
            PoolGroup<Component> group;
            if(!_componentGroups.TryGetValue(prefab, out group))
            {
                group = new PoolGroup<Component>(prefab);
                _componentGroups.Add(prefab, group);
                T pooledObj = group.Get() as T;
                _componentGroups.Add(pooledObj, group);
                if (activate)
                {
                    pooledObj.gameObject.SetActive(true);
                }
                return pooledObj;
            }
            else
            {
                T pooledObj = group.Get() as T;
                if (!_componentGroups.ContainsKey(pooledObj))
                {
                    _componentGroups.Add(pooledObj, group);
                }
                if (activate)
                {
                    pooledObj.gameObject.SetActive(true);
                }
                return pooledObj;
            }
        }

        public bool Return<T>(T obj, bool deactivate = true) where T : Component
        {
            PoolGroup<Component> group;
            if (_componentGroups.TryGetValue(obj, out group))
            {
                bool returned = group.Return(obj);
                if(deactivate && returned)
                {
                    obj.gameObject.SetActive(false);
                }
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