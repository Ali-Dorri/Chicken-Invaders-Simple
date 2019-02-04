using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIndicator : MonoBehaviour
{
    //
    //Fields
    //

    Text scoreText;
    Text health;

    //statics
    static InformationIndicator singleton = null;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static InformationIndicator Singleton
    {
        get
        {
            //make sure singleton is not null
            if (singleton == null)
            {
                singleton = FindObjectOfType<InformationIndicator>();

                //if there is no level manager in the scene
                if (singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Information Indicator");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<InformationIndicator>();
                }
            }

            return singleton;
        }
    }

    int Score
    {
        get
        {
            return GameData.Singleton.lastScore;
        }
        set
        {
            GameData.Singleton.lastScore = value;
        }
    }

    int ScorePerKill
    {
        get
        {
            return GameData.Singleton.GameOptionData.ScorePerKill;
        }
        set
        {
            OptionData data = GameData.Singleton.GameOptionData;
            data.ScorePerKill = value;
            GameData.Singleton.GameOptionData = data;
        }
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        SingletonCheck();
        FindInformation();
        InitializeInformation();
    }

    void SingletonCheck()
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
        }
    }

    void FindInformation()
    {
        GameObject[] informations = GameObject.FindGameObjectsWithTag("InformationUI");
        foreach(GameObject information in informations)
        {
            if(information.name == "Kills Number")
            {
                scoreText = information.GetComponent<Text>();
            }
            else if(information.name == "Health")
            {
                health = information.GetComponent<Text>();
            }
        }
    }

    void InitializeInformation()
    {
        Score = 0;
        scoreText.text = "0";
        health.text = GameData.Singleton.GameOptionData.Health.ToString();       
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void IncreaseScore()
    {
        Score += ScorePerKill;
        scoreText.text = Score.ToString();
    }

    public void SetHealth(float number)
    {
        health.text = number.ToString();
    }

}
