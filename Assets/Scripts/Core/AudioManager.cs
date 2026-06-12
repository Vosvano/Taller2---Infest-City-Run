using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio clips
    [SerializeField] private AudioClip coinCollectClip, gameOverClip, backgroundMusicClip, enemyHitClip, shootClip;
    
    [Header("Audio Sources Separados")]
    [SerializeField] private AudioSource musicSource; 
    [SerializeField] private AudioSource sfxSource;   

    public static AudioManager Instance { get; private set; }


    void Awake()
    {   
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }
    public void PlayCoinCollect()
    {
        sfxSource.PlayOneShot(coinCollectClip);
    }
    public void PlayGameOver()
    {
        sfxSource.PlayOneShot(gameOverClip);
    }
    public void PlayBackgroundMusic()
    {
        musicSource.clip = backgroundMusicClip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlayEnemyHit()
    {
        sfxSource.PlayOneShot(enemyHitClip);
    }
    public void PlayShoot()
    {
        sfxSource.PlayOneShot(shootClip);
    }
    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
}
