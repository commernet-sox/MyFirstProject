using System;
using System.Collections.Generic;

namespace CPC
{
    public class Singleton<T> : Singleton
    {
        private static T _instace;

        public static T Instance
        {
            get => _instace;
            set
            {
                _instace = value;
                AllSingletons[typeof(T)] = value;
            }
        }
    }

    public class SingletonList<T> : Singleton<IList<T>>
    {
        public SingletonList() => Singleton<IList<T>>.Instance = new List<T>();

        public static new IList<T> Instance => Singleton<IList<T>>.Instance;
    }

    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        public SingletonDictionary() => Singleton<IDictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();

        public static new IDictionary<TKey, TValue> Instance => Singleton<IDictionary<TKey, TValue>>.Instance;
    }

    public class Singleton
    {
        /// <summary>
        /// provides access to all "singletons" 
        /// </summary>
        public static IDictionary<Type, object> AllSingletons => new Dictionary<Type, object>();
    }
}
