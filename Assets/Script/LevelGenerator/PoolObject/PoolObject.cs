using System.Collections.Generic;
using UnityEngine;

public class PoolObject<T>
{
    private Queue<T> objectList = new Queue<T>();

    public int PoolSize => objectList.Count;

    public void AddItem(T item)
    {
        if (!objectList.Contains(item))
        {
            objectList.Enqueue(item);
        }
       
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

