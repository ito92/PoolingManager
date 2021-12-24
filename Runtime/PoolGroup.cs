using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaBois.Pooling
{
    public class PoolGroup<T> where T : Object
    {
        private T _prefab;
        private Queue<T> _availablePool = new Queue<T>();
        private List<T> _usedPool = new List<T>();

        public PoolGroup(T prefab)
        {
            _prefab = prefab;
        }

        public T Get()
        {
            T obj;
            if(_availablePool.Count == 0)
            {
                obj = Object.Instantiate(_prefab);
            }
            else
            {
                obj = _availablePool.Dequeue();
            }

            _usedPool.Add(obj);
            return obj;
        }

        public bool Return(T obj)
        {
            if (_usedPool.Contains(obj))
            {
                _usedPool.Remove(obj);
                _availablePool.Enqueue(obj);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Belongs(T obj)
        {
            return _availablePool.Contains(obj) || _usedPool.Contains(obj);
        }

        public bool IsPrefab(T obj)
        {
            return obj == _prefab;
        }
    }
}