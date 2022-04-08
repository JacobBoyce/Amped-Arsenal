using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TickSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }
    public static event EventHandler<OnTickEventArgs> OnSubTick;
    public static event EventHandler<OnTickEventArgs> OnFullTick;
    private const float TICK_TIMER_MAX = .2F;

    private int tick;
    private float tickTimer;

    private void Awake()
    {
        tick = 0;
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if(tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            if(OnSubTick != null) OnSubTick(this, new OnTickEventArgs{tick = tick});
            /*
            How to subscribe to this event
            TickSystem.OnTick += delegate (object sender, TickSystem.OnTickEventArgs e) {do something when tick happens}
            */

            //When ticks equal 1 sec
            if(tick % 5 == 0)
            {
                if(OnFullTick != null) OnFullTick(this, new OnTickEventArgs{tick = tick});
            }
        }
    }
}
