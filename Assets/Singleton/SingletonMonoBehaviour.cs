using System;
using System.Dynamic;
using UnityEngine;

namespace Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    Type type = typeof(T);
                    _instance = (T)FindFirstObjectByType(type);
                    if (_instance == null) {
                        Debug.LogError($"There is no instance of {type.Name} on this scene");
                    }
                }
                
                return _instance;
            }
        }

        public static bool isExists => _instance != null;

        public static void DestroyInstance()
        {
            if (_instance == null)
            {
                Destroy(_instance.gameObject);
            }
            _instance = null;
        }

        protected void OnDestroy()
        {
            _instance = null;
        }
    }
}