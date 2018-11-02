using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerAttacher : MonoBehaviour
{
    //
    //Concept Definition
    //

    public enum LoadType { MainMenu, Option, Game, Quit}

    /////////////////////////////////////////////////////////////////////////////

    //
    //Fields
    //

    [SerializeField] LoadType loadType;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        Button button = GetComponent<Button>();

        //add the proper method of the level manager to the button on click event
        if(button != null)
        {
            LevelManager levelManager = LevelManager.Singleton;

            if (loadType == LoadType.MainMenu)
            {
                AddListenor(button, levelManager.LoadMainMenu);
            }
            else if (loadType == LoadType.Option)
            {
                AddListenor(button, levelManager.LoadOption);
            }
            else if (loadType == LoadType.Game)
            {
                AddListenor(button, levelManager.LoadGame);
            }
            else
            {
                AddListenor(button, levelManager.Quit);
            }
        }

        //remove this component
        Destroy(this);
    }

    void AddListenor(Button button, UnityEngine.Events.UnityAction load)
    {
        button.onClick.AddListener(load);
    }
}
