namespace pointcache.EventBus {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;

    public class EventBusHelper : MonoBehaviour {

        #region SINGLETON
        private static EventBusHelper _instance;
        public static EventBusHelper instance
        {
            get {
                if (!_instance)
                    _instance = GameObject.FindObjectOfType<EventBusHelper>();
                return _instance;
            }
        }
        #endregion

        public static event Action OnReset;

        void OnEnable() {
            //Gets all events project wide and caches their Raise calls
            toraise = new Dictionary<Type, Action>();
            var events = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(GameEvent)));
            Type eventhubtype = typeof(EventBus<>);
            foreach (Type t in events) {
                Type genMyClass = eventhubtype.MakeGenericType(t);
                var mthd = genMyClass.GetMethod("Raise", BindingFlags.Public | BindingFlags.Static);
                register(t, () => mthd.Invoke(null, null));
            }
        }

        private void Update() {
            OnReset();
        }

        private static Dictionary<Type, Action> toraise;

        /// <summary>
        /// Performs lookup, if critical use GetRaiseDelegate to cache the event
        /// </summary>
        /// <param name="type"></param>
        public static void Raise(Type type) {
            Action target;
            toraise.TryGetValue(type, out target);
            if (target != null)
                target();
        }

        public static Action GetRaiseDelegate(Type type) {
            Action target;
            toraise.TryGetValue(type, out target);
            return target;
        }

        static void register(Type type, Action toraise) {
            EventBusHelper.toraise.Add(type, toraise);
        }
    }

}