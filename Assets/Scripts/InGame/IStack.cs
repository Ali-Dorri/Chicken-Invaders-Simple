using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStack<T>
{
    void Push(T data);
    T Pop();
    T Top { get; }
    int Size { get; }
    bool IsEmpty { get; }
}
