namespace pointcache.EventBus {
    using UnityEngine;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// An abstract base class for creating events - derive from it to create your own game events to use
    /// in the generic event system, EventManager.
    /// </summary>
    public abstract class GameEvent {

        /// <summary>
        /// Restore event to its default state
        /// </summary>
        public virtual void Reset() { }
        public virtual void Raise() { }
    }

    public static class EventBus<T> where T : GameEvent, new() {

        private const int INITIALPOOLCAPACITY = 32;

        private static event Action<T> SubscribersWithArgs;
        private static event Action Subscribers;

        private static List<T> m_pool = new List<T>(INITIALPOOLCAPACITY);
        private static List<T> m_inuse = new List<T>(INITIALPOOLCAPACITY);


        public static T Next
        {

            get {
                if (m_pool.Count == 0) {
                    AddMoreEventsToPool();
                }

                T ev = m_pool[0];
                m_pool.RemoveAt(0);
                m_inuse.Add(ev);
                ev.Reset();
                return ev;
            }

        }

        static EventBus() {

            EventBusHelper.OnReset += Reset;

        }

        public static void Subscribe(Action<T> handler) {
            SubscribersWithArgs += handler;
        }

        public static void UnSubscribe(Action<T> handler) {
            SubscribersWithArgs -= handler;
        }

        public static void Subscribe(Action handler) {
            Subscribers += handler;
        }

        public static void UnSubscribe(Action handler) {
            Subscribers -= handler;
        }

        public static void Raise(T e) {

            if (SubscribersWithArgs != null) {
                SubscribersWithArgs(e);


            }

            if (Subscribers != null) {
                Subscribers();
            }

            if (e != null) {
                m_inuse.Remove(e);
                m_pool.Add(e);
            }
        }

        public static void Clear() {
            Subscribers = null;
            SubscribersWithArgs = null;
        }

        private static void Reset() {
            m_pool.AddRange(m_inuse);
            m_inuse.Clear();
        }

        private static void AddMoreEventsToPool() {

            for (int i = 0; i < INITIALPOOLCAPACITY; i++) {
                m_pool.Add(new T());
            }

        }
    }
}