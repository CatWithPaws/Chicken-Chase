using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PoolObject<T>
{
    private Queue<T> objectList = new Queue<T>();

    public void AddItem(T item)
    {
        objectList.Enqueue(item);
    }

    public T PickAvailableItem()
    {
        if (objectList.Count > 0) 
        {
            var item = objectList.Dequeue();
            return item;
        }
        else
        {
            throw new System.Exception("Pool is Empty");
        }
    }
}

