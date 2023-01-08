using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sounds Music")]
    [SerializeField] AudioClip levelMusic;

    [Header("Sounds Player")]
    [SerializeField] AudioClip[] playerJumps;
    [SerializeField] AudioClip playerDies;

    [Header("Sounds Tree")]
    [SerializeField] AudioClip treeGrows;

    [Header("Sounds Seed")]
    [SerializeField] AudioClip seedCollect;

    AudioSource musicAudioSource;
    AudioSource playerAudioSource;
    AudioSource treeAudioSource;
    AudioSource seedAudioSource;

    private void Awake()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            switch (audioSource.name)
            {
                case "MusicAudioSource":
                    musicAudioSource = audioSource;
                    break;
                case "PlayerAudioSource":
                    playerAudioSource = audioSource;
                    break;
                case "TreeAudioSource":
                    treeAudioSource = audioSource;
                    break;
                case "SeedAudioSource":
                    seedAudioSource = audioSource;
                    break;
                default:
                    break;
            }

            if (audioSource.name == "MusicAudioSource")
            {
                musicAudioSource = audioSource;
                musicAudioSource.clip = levelMusic;
            } else if ( audioSource.name == "PlayerAudioSource")
            {
                playerAudioSource = audioSource;
            }
            else if (audioSource.name == "PlayerAudioSource")
            {
                playerAudioSource = audioSource;
            }
            else if (audioSource.name == "PlayerAudioSource")
            {
                playerAudioSource = audioSource;
            }
        }
    }

    public void PlayMusic() {
        if (musicAudioSource.clip == null)
        {
            musicAudioSource.clip = levelMusic;
        }
        musicAudioSource.Play();
    }

    public void PlayPlantTree()
    {
        if (treeAudioSource.clip == null)
        {
            treeAudioSource.clip = treeGrows;
        }
        treeAudioSource.Play();
    }

    public void PlayCollectSeed()
    {
        if (seedAudioSource.clip == null)
        {
            seedAudioSource.clip = seedCollect;
        }
        seedAudioSource.Play();
    }

    public void PlayPlayerJump()
    {
        playerAudioSource.clip = playerJumps[Random.Range(0, 5)];
        playerAudioSource.Play();
    }

    public void PlayPlayerDie()
    {
        // player can always have a clip e.g. jump sounds
        playerAudioSource.clip = playerDies;
        playerAudioSource.Play();
    }

}
