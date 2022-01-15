using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaBois.Pooling
{
    public class PoolGroup<T> where T : Component
    {
        private T _prefab;
        private Queue<T> _availablePool = new Queue<T>();
        //private Queue<T> _backupPool = new Queue<T>();
        private List<T> _usedPool = new List<T>();
        private int _limit = -1;
        private bool _autoActivate;

        public PoolGroup(T prefab, bool autoActivate = true, int limit = -1)
        {
            _prefab = prefab;
            _limit = limit;
            _autoActivate = autoActivate;
        }

        public T Get()
        {
            T obj;
            if((_limit > -1 && _usedPool.Count >= _limit) || _availablePool.Count == 0)
            {
                if (_limit > -1 && _usedPool.Count >= _limit)
                {
                    if (_availablePool.Count == 0)
                    {
                        obj = Object.Instantiate(_prefab);
                    }
                    else
                    {
                        obj = _availablePool.Dequeue();
                    }
                    Return(_usedPool[0]);                    
                }
                else
                {
                    obj = Object.Instantiate(_prefab);
                }
            }
            else
            {
                obj = _availablePool.Dequeue();
            }

            _usedPool.Add(obj);

            if (_autoActivate)
            {
                obj.gameObject.SetActive(true);
            }

            return obj;
        }

        public bool Return(T obj)
        {
            if (_usedPool.Contains(obj))
            {
                _usedPool.Remove(obj);

                _availablePool.Enqueue(obj);

                if (_autoActivate)
                {
                    obj.gameObject.SetActive(false);
                }

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