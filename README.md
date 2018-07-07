# BasicEventBus
Basic event bus for unity

 * Structs for events - no garbage
 * Interface based subscription
 * High performance (1mil events risen per frame at 60 fps on my machine)

 # Create an event
 
 Create new struct, assign `IEvent` interface
 
 ```csharp
 
public struct TestEvent : IEvent
{
    public string a;
    public float b;

}


```

# Raising an event

Slower method (has to do lookup to resolve subscribers)

```cs

EventBus.Raise(new TestEvent()
{
    b = 7,
    a = "Hello"
});
                
```

Fast method (directly invoke raise on the generic bus)

```cs

EventBus<TestEvent>.Raise(new TestEvent()
{
    b = 7,
    a = "Hello"
});
                
```

# Subscribing

1. Implement desired interfaces on a class

```cs


public class EventBusTest : MonoBehaviour,
    IEventReceiver<TestEvent>,
    IEventReceiver<OnResourceDrop>
{


```

2. Feed instance of it to `EventBus.Register()`

```cs

    void Start()
    {
        EventBus.Register(this);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister(this);
    }

```

It will automatically subscribe everything, reflection is used only once on application start, after that everything is cached and mapped.

