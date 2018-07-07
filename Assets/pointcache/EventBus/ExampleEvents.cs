using System;
using pointcache.EventBus;


    public struct ViewDragStart : IEvent {

    }

    public struct OnResourceDrop : IEvent {

        public string ResType;
        public float Amount;

    }
