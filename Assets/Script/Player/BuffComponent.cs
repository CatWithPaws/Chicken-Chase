using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Timers;
using TMPro;

public class BuffComponent : MonoBehaviour
{
    public List<Buff> Buffs = new List<Buff>();

    private PlayerController player => GameData.Player;



    [SerializeField] private TextMeshProUGUI DebugBuffInfo;

    public delegate void OnBuff(BuffType buff);

    public static OnBuff OnAddNewBuff;
    public static OnBuff OnRemoveBuff;


    private void OnDestroy()
    {
        RemoveAllBuffs();
        OnAddNewBuff = null;
        OnRemoveBuff = null;
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
        }
    }

    public void RemoveAllBuffs()
    {
        foreach(Buff buff in Buffs.ToArray())
        {
            RemoveBuff(buff);
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

        OnAddNewBuff(buff.Type);
    }

    private void RemoveBuff(Buff buff)
    {
        Buffs.Remove(buff);
        OnRemoveBuff(buff.Type);
        print("Buff removed " + buff.Type);
    }
}