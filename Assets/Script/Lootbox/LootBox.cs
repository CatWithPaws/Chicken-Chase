using System;
using UnityEngine;
using System.Collections.Generic;
public class Lootbox<T> 
    {
        public string Name;
        public List<LootBoxItem<T>> ItemsPool;
        public float MaxTreshHold = 0;

        private int fullNum;
        private int powerOfTen;

        public Lootbox(string name, List<LootBoxItem<T>> itemsPool)
        {
            Name = name;
            ItemsPool = itemsPool;

            CalculateTreshHolds(itemsPool);
        }


        protected virtual void CalculateTreshHolds(List<LootBoxItem<T>> itemsPool)
        {
            foreach (var Item in itemsPool)
            {
                MaxTreshHold = Item.CalculateTreshHolds(MaxTreshHold);
            }

            string MaxTreshHoldString = MaxTreshHold.ToString();
            var localTreshHold = MaxTreshHold;

            int length = MaxTreshHoldString.Substring(MaxTreshHoldString.IndexOf(",") + 1).Length - 1;

            if (length > 5)
            {
                length = 5;
                localTreshHold = (float)Math.Round(localTreshHold, length);
            }

            powerOfTen = (int)Math.Pow(10, length);
            fullNum = (int)(localTreshHold * powerOfTen);
        }


        public T GetRandomThing()
        {
            int rndNum = new System.Random().Next(fullNum);

            float rndNormalNum = (float)rndNum / (float)powerOfTen;

            foreach (var Item in ItemsPool)
            {
                if (Item.IsInRange(rndNormalNum))
                {
                    return Item.Item;
                }
            }
            return default;
        }


    }

    public class LootBoxItem<T>
    {
        public T Item;

        public float Chance;
        public float LowerTreshHold;
        public float UpperTreshHold;

        public LootBoxItem(T item, float chance)
        {
            Item = item;
            Chance = chance;
        }

        public float CalculateTreshHolds(float startTreshHold)
        {
            LowerTreshHold = startTreshHold;
            UpperTreshHold = startTreshHold + Chance;
            return UpperTreshHold;
        }

        public bool IsInRange(float num)
        {
            return num >= LowerTreshHold && num < UpperTreshHold;
        }
    }