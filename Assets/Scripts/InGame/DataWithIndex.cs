using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataWithIndex<T>
{
    T data;
    int index;

    public T Data
    {
        get
        {
            return data;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }
    }

    public DataWithIndex(T data, int index)
    {
        this.data = data;
        this.index = index;
    }
}
