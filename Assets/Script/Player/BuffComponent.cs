using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;
using TMPro;

public class BuffComponent : MonoBehaviour
{
    public List<Buff> Buffs = new List<Buff>();

    private PlayerController player => GameData.Instance.Player;

    private Timer timer;

    private int TimerTickPeriod = 100;

    [SerializeField] private TextMeshProUGUI DebugBuffInfo;

    private void Awake()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        timer = new Timer(TimerTickPeriod);
        //timer.Elapsed += OnSecondPassed;
        timer.AutoReset = true;
        //timer.Start();
    }

    private void OnDestroy()
    {
        timer.Dispose();
        Buffs.Clear();
    }

    private void Update()
    {
        DebugBuffInfo.text = "";
        foreach (Buff buff in Buffs.ToArray())
        {
            buff.Duration -= Time.deltaTime;

            if(buff.Duration <= 0)
            {
                buff.OnBuffEnd(player);
                RemoveBuff(buff);
                continue;
            }

            DebugBuffInfo.text += "BuffType: " + buff.Type + "\n";
            DebugBuffInfo.text += "Duration: " + buff.Duration + "\n\n";
        }
    }

    public void AddBuff(Buff buff)
    {
        Buff existedBuff = Buffs.FirstOrDefault(i => i.Type == buff.Type);
        if (existedBuff != null)
        {
            existedBuff.Duration = buff.Duration;
            print("Buff extended " + buff.Type);
        }
        else
        {
            Buffs.Add(buff);
            buff.OnBuffStart(player);
            print("Buff Added " + buff.Type);
        }


    }

    private void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
        print("Buff removed " + buff.Type);
    }
}