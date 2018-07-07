

using System;
using System.Collections.Generic;
using pEventBus;

public struct OnMapGenerated : IEvent
{
    public int mapsize;
}
