using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    //
    //Fields
    //

    AudioSource audioSource;
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip winTrack;
    [SerializeField] AudioClip loseTrack;
    bool isInitialized = false;

    //static
    static BackgroundMusicPlayer singleton = null;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static BackgroundMusicPlayer Singleton
    {
        get
        {
            //make sure singleton is not null
            if (singleton == null)
            {
                singleton = FindObjectOfType<BackgroundMusicPlayer>();

                //if there is no level manager in the scene
                if (singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Background Music Player");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<BackgroundMusicPlayer>();
                }
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
        if (!isInitialized)
        {
            Initialize();
        }
    }

    void Initialize()
    {
        if (singleton != null)
        {
            if (this == singleton)
            {
                InitializeSingleton();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            singleton = this;
            InitializeSingleton();
        }
    }

    void InitializeSingleton()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        if (audioSource.clip == null)
        {
            SetMusicClipAccordingToActiveScene();
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    private void OnLevelWasLoaded(int level)
    {
        if (!isInitialized)
        {
            Initialize();
        }

        //change clip if needed
        if(this == singleton)
        {
            SetMusicClipAccordingToActiveScene();
        }  
    }

    void SetMusicClipAccordingToActiveScene()
    {
        string sceneName = LevelManager.GetActiveSceneName();

        if (sceneName == "Game")
        {
            ChangeClip(gameMusic, true);
        }
        else //Main Menu, Option, Win Scene, Lose Scene
        {
            ChangeClip(mainMusic, true);
        }
    }

    public void PlayWinTrack()
    {
        ChangeClip(winTrack, false);
    }

    public void PlayLoseTrack()
    {
        ChangeClip(loseTrack, false);
    }

    void ChangeClip(AudioClip clip, bool loop)
    {
        if (audioSource.clip != clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.loop = loop;
        }
    }
}
