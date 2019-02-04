using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TemporarySound : MonoBehaviour
{
    //
    //Fields
    //  

    static GameObject prefab;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public static void CreateSound(AudioClip clip)
    {
        GameObject tempGameObject = Instantiate(GetPrefab(), Vector3.zero, Quaternion.identity);
        AudioSource audio = tempGameObject.GetComponent<AudioSource>();
        audio.clip = clip;
        audio.volume = GameData.Singleton.GameOptionData.masterVolume;
        TemporarySound tempSound = tempGameObject.GetComponent<TemporarySound>();
        tempSound.StartCoroutine(tempSound.PlaySound(audio));
    }

    static GameObject GetPrefab()
    {
        if (prefab != null)
        {
            return prefab;
        }

        prefab = Resources.Load<GameObject>("Prefabs/Temporary Sound");
        return prefab;
    }

    IEnumerator PlaySound(AudioSource audio)
    {
        audio.Play();

        while (audio.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    
}
