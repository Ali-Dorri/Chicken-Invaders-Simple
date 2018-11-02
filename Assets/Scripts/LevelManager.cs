using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //
    //Fields
    //

    static LevelManager singleton = null;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static LevelManager Singleton
    {
        get
        {
            //make sure singleton is not null
            if(singleton == null)
            {
                singleton = FindObjectOfType<LevelManager>();

                //if there is no level manager in the scene
                if (singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Level Manager");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<LevelManager>();
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
        if(singleton != null)
        {
            if (this == singleton)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadOption()
    {
        SceneManager.LoadScene("Option");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if(this == singleton)
        {
            singleton = null;
        }
    }
}
