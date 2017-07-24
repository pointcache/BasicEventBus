# BasicEventBus
Basic event bus for unity

 * Has a pool of event objects, that get cleaned up at the end of the frame
 * Raise with and without arguments
 * Subscribe methods with and without parameters

# Installation

 1. Drop files into project
 2. Drop `EventBusHelper` into the scene
 3. In Script Execution Order make sure EventBus is before everything else that uses events.
 
 # Usage
 
 ### Create an event
 
 Events are defined as classes derived from GameEvent.
 Add any data you want it to carry, and implement Reset and Raise;
 
 **Reset()** will be called before event is given to you by the pool
 **Raise()** is a convenience method that you yourself call, read below
 
 ```csharp
 
     public class StartBuildingRequest : GameEvent {

        public string Tag;

        public override void Reset() {
            Tag = "";
        }

        public override void Raise() {
            EventBus<StartBuildingRequest>.Raise(this);
        }
    }


```

### Raising an event
##### With arguments

1. Use `EventBus<T>.Next` to get an event object instance from the pool.
2. Set your event data
3. Call `Raise()` on it.


```csharp

    var ev = EventBus<OnChangeRallyPoint>.Next;
    ev.position = mouseclickpos;
    ev.Raise();
    
    ```
    
Alternatively you can raise it with

```csharp

EventBus<OnChangeRallyPoint>.Raise(ev);

```

but who likes typing?

##### Without arguments

Simply raise it with null.

```csharp

EventBus<OnChangeRallyPoint>.Raise(null);

```

### Observing

You can subscribe both methods with a GameEvent as argument and without

##### With argument

```csharp

void Awake() {
        EventBus<OnDeath>.Subscribe(OnDeath);
    }

    void OnDeath(OnDeath ev) {
        if (ev.dead.tag == GameConstants.PLAYER)
            GameOver();
    }
    
```


##### Without argument

```csharp


   private void Awake() {
        EventBus<OnGameOver>.Subscribe(OnGameOver);
    }

    private void OnDisable() {
        EventBus<OnGameOver>.UnSubscribe(OnGameOver);
    }

    void OnGameOver() {
        panel.gameObject.SetActive(true);
    }
    
    
```


# Dont forget to unsubscribe your methods! Don't be lazy and leave bugs in.
