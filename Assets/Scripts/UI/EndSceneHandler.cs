using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField] Text score;

	void Start ()
    {
        score.text = GameData.Singleton.lastScore.ToString();
	}
}
