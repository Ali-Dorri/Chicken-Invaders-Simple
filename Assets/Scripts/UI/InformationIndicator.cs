using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationIndicator : MonoBehaviour
{
    //
    //Fields
    //

    Text killsNumber;
    Text health;
    int numberOfKills = 0;

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
                killsNumber = information.GetComponent<Text>();
            }
            else if(information.name == "Health")
            {
                health = information.GetComponent<Text>();
            }
        }
    }

    void InitializeInformation()
    {
        killsNumber.text = numberOfKills.ToString();
        health.text = Resources.Load<GameObject>("Prefabs/Player").GetComponent<PlayerShip>().Health.ToString();
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void AddKillsNumber()
    {
        killsNumber.text = (++numberOfKills).ToString();
    }

    public void SetHealth(float number)
    {
        health.text = number.ToString();
    }

}
