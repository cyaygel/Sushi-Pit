using System;
using UnityEngine;

namespace Utility
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var assets = Resources.LoadAll<T>("ScriptableObjects/");
                if (assets == null)
                {
                    throw new Exception($"Could not find any singleton scriptable object instance in the resources");
                }

                if (assets.Length > 1)
                {
                    Debug.LogError("Multiple instance of the singleton scriptable object found in the resources");
                }

                _instance = assets[0];
                return _instance;
            }
        }
#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            _instance = null;
        }
#endif
    }
}