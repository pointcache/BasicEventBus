using System;
using pointcache.EventBus;

namespace Tungsten.Events.Camera {
    public class ViewDragStart : GameEvent {

        public override void Raise() {
            EventBus<ViewDragStart>.Raise(this);
        }

    }
    public class ViewDragEnd : GameEvent {

        public override void Raise() {
            EventBus<ViewDragEnd>.Raise(this);
        }
    }
}

namespace Tungsten.Events.Game.Construction {

    public class StartBuildingRequest : GameEvent {

        public string Tag;

        public override void Reset() {
            Tag = "";
        }

        public override void Raise() {
            EventBus<StartBuildingRequest>.Raise(this);
        }
    }

}

namespace Tungsten.Events.Game.Resources {
    public class OnResourceDrop : GameEvent {
        public string ResType;
        public float Amount;

        public override void Reset() {
            ResType = string.Empty;
            Amount = 0;
        }

        public override void Raise() {
            EventBus<OnResourceDrop>.Raise(this);
        }
    }

}

namespace Tungsten.Events.Game.State {
    public class OnGameOver : GameEvent {
        public string Message;

        public override void Reset() {
            Message = string.Empty;
        }

        public override void Raise() {
            EventBus<OnGameOver>.Raise(this);
        }
    }

}

