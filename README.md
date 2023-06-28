# BasicEventBus
Basic event bus for unity

 * Simple
 * Structs for events - no garbage
 * Binding based or direct subscription, no interfaces
 * High performance 

 # Create an event
 
 Create new struct, assign `IEvent` interface
 
 ```cs

public struct TestEvent : IEvent
{
    public string a;
    public float b;

}
```

# Raising an event

```cs

EventBus<TestEvent>.Raise(new TestEvent()
{
    b = 7,
    a = "Hello"
});
                
```

# Usage

Check EventBusExample.cs

1. Declare an event binding object

```cs

public class EventBusTest : MonoBehaviour
{
          EventBinding<TestEvent> _onTestEvent;


```

2. Initialize it in Awake/Start/Ctor

```cs

        private void Awake()
        {
            _onTestEvent = new EventBinding<TestEvent>(OnEvent);
            // Add couple more listeners to the same binding   
            _onTestEvent.Add(onEventButPrintTheStringInstead);
            _onTestEvent.Add(someUnrelatedMethodWithoutArgs);

            EventBus<TestEvent>.Raise(new TestEvent()
            {
                a = "Hello"
            });
                                              
            _onTestEvent.Remove(onEventButPrintTheStringInstead);
            _onTestEvent.Remove(someUnrelatedMethodWithoutArgs);

            // Callback will be called precisely once, then dropped
            EventBus<TestEvent>.AddCallback(SomeRandomMethod);

            StartCoroutine(CoroutineThatDispatchesEventAfterSomeTime());
            StartCoroutine(CoroutineThatWaitsForEvent());
        }
```

3. Control observing state through `Listen`

```cs

        private void OnEnable()
        {
            _onTestEvent.Listen = true;
        }

        private void OnDisable()
        {
            _onTestEvent.Listen = false;
        }
```


