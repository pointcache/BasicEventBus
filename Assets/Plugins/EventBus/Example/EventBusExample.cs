namespace EventBus.Example
{
    using System.Collections;
    using EventBus;
    using MyGame.Events;
    using UnityEngine;

    namespace MyGame.Events
    {
        public struct TestEvent : IEvent
        {
            public string a;
            public float b;
        }

        public struct EventThatWillBeAwaitedByCoroutine : IEvent
        {
            public string message;
        }
    }

    namespace MyGame.Events.UI
    {
        public struct OnClickNewGame : IEvent { }
        public struct OnWantsToDeleteInventoryItem : IEvent { }
        public struct OnWaitingForPlayerInput : IEvent { }
    }

    public class EventBusExample : MonoBehaviour
    {
        public int PerFrame = 1000;
        public bool testPerformance;

        EventBinding<TestEvent> _onTestEvent;
        float accumulate;

        private void Awake()
        {
            _onTestEvent = new EventBinding<TestEvent>(OnEvent);

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

        private IEnumerator CoroutineThatDispatchesEventAfterSomeTime()
        {
            yield return new WaitForSeconds(3);
            EventBus<EventThatWillBeAwaitedByCoroutine>.Raise(new EventThatWillBeAwaitedByCoroutine() { message = "Say yay!" });
        }

        private IEnumerator CoroutineThatWaitsForEvent()
        {
            var awaiter = EventBus<EventThatWillBeAwaitedByCoroutine>.NewAwaiter();
            while (!awaiter.EventRaised)
                yield return null;
            Debug.Log(awaiter.Payload.message);
            Debug.Log("Yay!");
        }

        private void OnEnable()
        {
            _onTestEvent.Listen = true;
        }

        private void OnDisable()
        {
            _onTestEvent.Listen = false;
        }

        public void OnEvent(TestEvent e) => accumulate += e.b;

        private void onEventButPrintTheStringInstead(TestEvent e) => Debug.Log(e.a);

        private void someUnrelatedMethodWithoutArgs() => Debug.Log("Hello?");

        private void SomeRandomMethod() => Debug.Log("I was called once");

        void Update()
        {
            if (testPerformance)
            {
                for (int i = 0; i < PerFrame; i++)
                {
                    EventBus<TestEvent>.Raise(new TestEvent()
                    {
                        b = 7,
                        a = "Hello"
                    });
                }
            }
        }
    }

}