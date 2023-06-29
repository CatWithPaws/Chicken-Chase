using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;

public class BuffComponent : MonoBehaviour
{
    public List<Buff> Buffs = new List<Buff>();

    private PlayerController player => GameData.Instance.Player;

    private Timer timer;

    private int TimerTickPeriod = 10;

    private void Awake()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        timer = new Timer(TimerTickPeriod);
        timer.Elapsed += OnSecondPassed;
        timer.AutoReset = true;
        timer.Start();
    }

    private void OnSecondPassed(object sender, ElapsedEventArgs e)
    {
        foreach (Buff buff in Buffs)
        {
            buff.Duration -= TimerTickPeriod;

            if(buff.Duration <= 0)
            {
                buff.OnBuffEnd(player);
                RemoveBuff(buff);
            }
        }
    }

    public void AddBuff(Buff buff)
    {
        if (Buffs.Contains(buff))
        {
            Buff existedBuff = Buffs.First(i => i.Type == buff.Type);
            existedBuff.Duration = buff.Duration;
        }
        else
        {
            Buffs.Add(buff);
            buff.OnBuffStart(player);
        }
    }

    private void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
    }
}