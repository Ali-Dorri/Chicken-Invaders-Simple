using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableAnimator : IPausable
{
    //
    //Fields
    //

    readonly Animator animator;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public Animator TheAnimator
    {
        get
        {
            return animator;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    public PausableAnimator(Animator animator)
    {
        this.animator = animator;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void PauseOrResume(bool isPaused)
    {
        if (isPaused)
        {
            //stop animation
            animator.StartPlayback();
        }
        else
        {
            //resume animation
            animator.StopPlayback();
        }
    }
}
