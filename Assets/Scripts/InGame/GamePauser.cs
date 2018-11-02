using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : MonoBehaviour
{
    //
    //Fields
    //

    bool isPaused = false;
    PauseHelper firstPauseHelper = null;
    PauseHelper lastPauseHelper = null;
    PauseMenu pauseMenu;

    //static
    static GamePauser singleton;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static GamePauser Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = FindObjectOfType<GamePauser>();
            }
  
            return singleton;    
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        if(GamePauser.singleton == null)
        {
            singleton = this;
        }
        else
        {
            if (!ReferenceEquals(this, singleton))
            {
                Destroy(gameObject);
            }
        }

        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseOrResume();
            if (isPaused)
            {
                pauseMenu.MovePanelPause();
            }
            else
            {
                pauseMenu.MovePanelResume();
            }
        }
	}

    public void AddPauseHelper(PauseHelper pauseHelper)
    {
        if(lastPauseHelper != null /*&& firstPauseHelper != null*/)
        {
            lastPauseHelper.Next = pauseHelper;
            pauseHelper.Previous = lastPauseHelper;

            while(lastPauseHelper.Next != null)
            {
                lastPauseHelper = lastPauseHelper.Next;
            }
        }
        else    //lastPauseHelper == null && firstPauseHelper == null
        {
            lastPauseHelper = pauseHelper;
            firstPauseHelper = pauseHelper;

            while (lastPauseHelper.Next != null)
            {
                lastPauseHelper = lastPauseHelper.Next;
            }
        }


    }

    public void RemovePauseHelper(PauseHelper pauseHelper)
    {
        if(pauseHelper == lastPauseHelper)
        {
            if (pauseHelper == firstPauseHelper)
            {
                pauseHelper.Next = null;
                pauseHelper.Previous = null;
                lastPauseHelper = null;
                firstPauseHelper = null;
            }
            else
            {
                pauseHelper.Previous.Next = null;
                lastPauseHelper = pauseHelper.Previous;
                pauseHelper.Previous = null;
            }
        }
        else
        {
            if(pauseHelper == firstPauseHelper)
            {
                pauseHelper.Next.Previous = null;
                firstPauseHelper = pauseHelper.Next;
                pauseHelper.Next = null;
            }
            else
            {
                pauseHelper.Next.Previous = pauseHelper.Previous;
                pauseHelper.Previous.Next = pauseHelper.Next;
                pauseHelper.Next = null;
                pauseHelper.Previous = null;
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        PauseOrResume();
    }

    public void Resume()
    {
        isPaused = false;
        PauseOrResume();
    }

    public void PauseOrResume()
    {
        if(firstPauseHelper != null)
        {
            PauseHelper pauseHelper = firstPauseHelper;

            while (pauseHelper.Next != null)
            {
                pauseHelper.PauseOrResume(isPaused);
                pauseHelper = pauseHelper.Next;
            }

            pauseHelper.PauseOrResume(isPaused);
        }
    }
}
