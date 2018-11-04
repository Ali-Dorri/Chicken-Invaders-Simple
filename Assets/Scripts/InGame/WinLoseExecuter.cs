using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseExecuter : MonoBehaviour
{
    //
    //Fields
    //

    static WinLoseExecuter singleton = null;
    [SerializeField] float fadeTime = 1;
    [SerializeField] float waitTime = 2;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    public static WinLoseExecuter Singleton
    {
        get
        {
            //make sure singleton is not null
            if (singleton == null)
            {
                singleton = FindObjectOfType<WinLoseExecuter>();

                //if there is no level manager in the scene
                if (singleton == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("Prefabs/Win Lose Executer");
                    prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                    singleton = prefab.GetComponent<WinLoseExecuter>();
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
        //singleton process
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

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void Win()
    {
        Text text = GetWinText();
        SetAlpha(text, 0);
        StartCoroutine(FadeTextAndLoadScene(text, LevelManager.Singleton.LoadWinScene));
    }

    public void Lose()
    {
        Text text = GetLoseText();
        SetAlpha(text, 0);
        StartCoroutine(FadeTextAndLoadScene(text, LevelManager.Singleton.LoadLoseScene));
    }

    IEnumerator FadeTextAndLoadScene(Text text, UnityEngine.Events.UnityAction action)
    {
        float timeElapsed = 0;

        for(;timeElapsed < fadeTime; timeElapsed += Time.deltaTime)
        {
            SetAlpha(text, text.color.a + Time.deltaTime / fadeTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        action();
    }

    void SetAlpha(Text text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    Text GetLoseText()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Game Lose Text");
        return CreateText(prefab);
    }

    Text GetWinText()
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Game Win Text");
        return CreateText(prefab);       
    }

    Text CreateText(GameObject prefab)
    {
        Transform canvasTransform = FindObjectOfType<Canvas>().transform;
        prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity, canvasTransform);
        prefab.transform.localPosition = Vector3.zero; ;
        canvasTransform.Find("Pause Panel").SetSiblingIndex(transform.GetSiblingIndex() - 1);      

        return prefab.GetComponent<Text>();
    }
}
