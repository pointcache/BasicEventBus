using System.Collections;
using System.Collections.Generic;
using pEventBus;
using UnityEngine;

public struct TestEvent : IEvent
{
    public string a;
    public float b;

}

public class EventBusTest : MonoBehaviour,
    IEventReceiver<TestEvent>
{

    public int PerFrame = 1000;
    public bool testPerformance;

    void Start()
    {
        EventBus.Register(this);
    }

    private void OnDestroy()
    {
        EventBus.UnRegister(this);
    }


    // Update is called once per frame
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

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 30), "Raise Test"))
        {
            EventBus<TestEvent>.Raise(new TestEvent()
            {
                b = 7,
                a = "Hello"
            });
        }
    }

    public void OnEvent(TestEvent e)
    {
        //print(e.a);
    }


}
