using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackArray<T> : IStack<T>
{
    //
    //Fields
    //

    T[] datas;
    int topIndex = -1;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public T Top
    {
        get
        {
            return datas[topIndex];
        }
    }

    public int Size
    {
        get
        {
            return datas.Length;
        }
    }

    public bool IsEmpty
    {
        get
        {
            if (topIndex > -1)
                return false;

            return true;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    public StackArray(int size)
    {
        datas = new T[size];
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    /// <summary>
    /// Pops the top data from stack. Befor using it check if it is empty otherwise it may throw IndexOutOfRangeException.
    /// </summary>
    /// <returns></returns>
    public T Pop()
    {
        T data = datas[topIndex];
        datas[topIndex] = default(T);
        if(topIndex > -1)
        {
            topIndex--;
        }
        
        return data;
    }

    public void Push(T data)
    {
        //stack has space
        if(topIndex < datas.Length - 1)    
        {
            topIndex++;
            datas[topIndex] = data;
        }
        //stack is full
        else
        {
            //make new array with new length two times previous length
            int newLength = datas.Length * 2;
            T[] tempArray = datas;
            datas = new T[newLength];
            topIndex++;
            for(int i = 0; i < topIndex; i++)
            {
                datas[i] = tempArray[i];
            }

            //add the data
            datas[topIndex] = data;
        }
    }
}
