using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataAccessLayer
{
    internal class CachedObject
    {
       
        public CachedObject(Guid key, 
            object value, 
            DateTime expiration
            ) 
        {
            this.key = key;
            this.value = value;
            this.expiration = expiration;
        }

        private Guid key;
        public Guid Key
        {
            get { return key; }
        }

        private object value;
        public object Value
        {
            get { return value; }
        }

        private DateTime expiration;
        public DateTime Expiration
        {
            get { return expiration; }
        }

    }

    internal static class ObjectCache
    {

        public const int MinutesToHold = 5;
        private static Dictionary<Guid, CachedObject> cache = new Dictionary<Guid,CachedObject>();

        public static readonly object SyncRoot = new object();

        public static T Get<T>(Guid key)
        {

            lock (SyncRoot)
            {
                if (cache.ContainsKey(key) &&
                    cache[key].Value.GetType() == typeof(T))
                {

                    CachedObject cachedObject = cache[key];

                    if (cachedObject.Value.GetType() != typeof(T))
                        return default(T);

                    if (cachedObject.Expiration <= DateTime.Now)
                        return default(T);

                    return (T)cachedObject.Value;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public static void Add(Guid key, object value)
        {
            Add(key, value, TimeSpan.FromMinutes(MinutesToHold));

        }

        public static void Add(Guid key, object value, TimeSpan expiration)
        {
            lock (SyncRoot)
            {
                Remove(key);

                CachedObject cachedObject = new CachedObject(
                    key, value, DateTime.Now.Add(expiration));

                cache.Add(key, cachedObject);
            }

        }

        public static void Remove(Guid key)
        {
            lock (SyncRoot)
            {
                if (cache.ContainsKey(key))
                    cache.Remove(key);
            }
        }

    }

}
