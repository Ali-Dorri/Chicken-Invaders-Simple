﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySoundHandler : MonoBehaviour
{
    //
    //Fields
    //

    AudioSource audioSource;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip killedSound;

    /////////////////////////////////////////////////////////////////////////////

    //
    //Properties
    //

    /////////////////////////////////////////////////////////////////////////////

    //
    //Initializers
    //

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    /////////////////////////////////////////////////////////////////////////////

    //
    //Methods
    //

    public void PlayShoot()
    {
        PlayClip(shootSound);
    }

    public void PlayDamage()
    {
        PlayClip(damageSound);
    }

    public void PlayKilled()
    {
        AudioSource.PlayClipAtPoint(killedSound, Vector3.zero);
    }

    void PlayClip(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
