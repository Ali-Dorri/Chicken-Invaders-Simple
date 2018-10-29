using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that want to unify a queue should implement this interface(Each object accesses to next
/// object in the queue)./// </summary>
public interface IQueue
{
    object Next { get; set; }
}

/// <summary>
/// Objects that want to unify a double access queue should implement this interface(Each object accesses to next and
/// previous object in the queue).
/// </summary>
public interface IDoubleAccessQueue : IQueue
{
    object Previous { get; set; }
}

/// <summary>
/// Objects that want to unify a queue should implement this interface(Each object accesses to next
/// object in the queue)./// </summary>
public interface IQueue<T> : IQueue
{
    new T Next { get; set; }
}

/// <summary>
/// Objects that want to unify a double access queue should implement this interface(Each object accesses to next and
/// previous object in the queue).
/// </summary>
public interface IDoubleAccessQueue<T> : IQueue<T>, IDoubleAccessQueue
{
    new T Previous { get; set; }
}

public class InvalisCastInQueueException : System.InvalidCastException
{

    public InvalisCastInQueueException(object sender) : base()
    {
        Debug.LogError(string.Format("You should assign an object with {0} type to it's next or previous!", sender));
    }

    public InvalisCastInQueueException(object sender, string message) : base(message)
    {
        Debug.LogError(string.Format("You should assign an object with {0} type to it's next or previous!", sender));
        Debug.LogError(message);
    }
}