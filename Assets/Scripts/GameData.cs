using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    //
    //Fields
    //

    OptionData optionData;
    public int lastScore;

    //static
    static GameData singleton = null;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static GameData Singleton
    {
        get
        {
            //make sure singleton is not null
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameData>();

                //if there is no game data in the scene
                if (singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Game Data");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<GameData>();
                }

                singleton.Initialize();
            }

            return singleton;
        }
    }

    public OptionData GameOptionData
    {
        get
        {
            return optionData;
        }
        set
        {
            optionData = value;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    void Start()
    {
        if (singleton != null)
        {
            if (this != singleton)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            singleton = this;
            Initialize();
        }
    }

    void Initialize()
    {
        optionData.Load();
        DontDestroyOnLoad(gameObject);
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void SaveOption(OptionData data)
    {
        optionData = data;
        optionData.Save();
    }
}
