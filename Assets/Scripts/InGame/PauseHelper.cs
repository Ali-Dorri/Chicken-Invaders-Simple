using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelper : MonoBehaviour, IDoubleAccessQueue<PauseHelper>
{
    //
    //Fields
    //

    IPausable[] pausables;
    PauseHelper next = null;
    PauseHelper previous = null;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public PauseHelper Previous
    {
        get
        {
            return previous;
        }

        set
        {
            previous = value;
        }
    }

    public PauseHelper Next
    {
        get
        {
            return next;
        }
        set
        {
            next = value;
        }
    }
    object IDoubleAccessQueue.Previous
    {
        get
        {
            return Previous;
        }
        set
        {
            if(!(value is PauseHelper))
            {
                throw new InvalisCastInQueueException(this);
            }

            Previous = (PauseHelper)value;
        }
    }
    object IQueue.Next
    {
        get
        {
            return Next;
        }
        set
        {
            if (!(value is PauseHelper))
            {
                throw new InvalisCastInQueueException(this);
            }

            Next = (PauseHelper)value;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    void Start ()
    {
        List<IPausable> pausablesList = new List<IPausable>();

        //add IPausables in gameObject to pausablesList
        pausablesList.AddRange(GetComponents<IPausable>());

        //add pausableAnimators to pausablesList if there is any animator in gameObject
        Animator[] animators = GetComponentsInChildren<Animator>();
        if(animators != null)
        {
            int length = animators.Length;
            PausableAnimator[] pausableAnimators = new PausableAnimator[length];
            for (int i = 0; i < length; i++)
            {
                pausableAnimators[i] = new PausableAnimator(animators[i]);
            }

            pausablesList.AddRange(pausableAnimators);
        }

        //assign IPausables to pausables
        pausables = pausablesList.ToArray();
        
        Initialize();
	}

    public void Initialize()
    {
        GamePauser.Singleton.AddPauseHelper(this);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void PauseOrResume(bool isPaused)
    {
        foreach(IPausable pausable in pausables)
        {
            pausable.PauseOrResume(isPaused);
        }
    }

    private void OnDestroy()
    {
        GamePauser gamePauser = GamePauser.Singleton;

        if (gamePauser != null)
        {
            gamePauser.RemovePauseHelper(this);
        }
    }
}
