using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainBossFightMusic;
    [SerializeField] private AudioClip fightingMusic;
    [SerializeField] private AudioClip nearTownMusic;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float enemyMusicDistance;
    [SerializeField] private float townMusicDistance;
    private Pause _pause;
    private bool _fading;
    private bool _fighting;
    private bool _townClose;
    private bool _talking;
    private bool _start; // this is a fix for an error with playing the fighting music at the start
    private bool _fightingBoss;

    // storing time positions for music
    private float _menuMusicTimePosition;
    private float _mainBossFightMusicTimePosition;
    private float _fightingMusicTimePosition;
    private float _nearTownMusicTimePosition;
    private float _backgroundMusicTimePosition;

    [System.Obsolete]
    void Awake()
    {
        _pause = FindAnyObjectByType<Pause>();
        StartCoroutine(EnemiesDistanceLoop());
        StartCoroutine(TownDistanceLoop());
    }

    private void Update()
    {
        if (!_start) { return; }
        _fightingBoss = FindAnyObjectByType<FinalBoss>().IsFighting;
        if (_pause.Paused && audioSource.clip != menuMusic && !_fading)
        {
            StartCoroutine(FadeMusic(menuMusic));
        }
        else if (!_pause.Paused && _fightingBoss && audioSource.clip != mainBossFightMusic && !_fading)
        {
            StartCoroutine(FadeMusic(mainBossFightMusic));
        }
        else if (!_pause.Paused && !_fightingBoss && _fighting && audioSource.clip != fightingMusic && !_fading)
        {
            StartCoroutine(FadeMusic(fightingMusic));
        }
        else if (!_pause.Paused && !_fightingBoss && !_fighting && _townClose && audioSource.clip != nearTownMusic && !_fading)
        {
            StartCoroutine(FadeMusic(nearTownMusic));
        }
        else if (!_pause.Paused && !_fightingBoss && !_fighting && !_townClose && audioSource.clip != backgroundMusic && !_fading)
        {
            StartCoroutine(FadeMusic(backgroundMusic));
        }
        _fightingBoss = false;
    }

    [System.Obsolete] // i am using an older why to find all of the enemies by using FindObjectsOfType<Enemy>()
    private IEnumerator EnemiesDistanceLoop() //finds the distance to the closet enemy to see if the game should play fighting music
    { // this is inside of a Ienumerator to stop lag
        yield return new WaitForSeconds(0.2f);
        List<Enemy> enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        float closestEnemy = Mathf.Infinity;

        for (int i = 0; i < enemies.Count; i++)
        {
            float distance = enemies[i].Distance;
            if (distance < closestEnemy)
            {
                closestEnemy = distance;
            }
        }
        _talking = false;
        //if the player is talking the fighting music will stop
        List<NPC> npc = new List<NPC>(FindObjectsByType<NPC>(FindObjectsSortMode.None));
        for (int i = 0; i < npc.Count; i++)
        {
            if (npc[i].Talking)
            {
                _talking = true;
            }
        }

        if (closestEnemy < enemyMusicDistance && !_talking)
        {
            _fighting = true;
        }
        else
        {
            _fighting = false;
        }
        StartCoroutine(EnemiesDistanceLoop());
    }

    [System.Obsolete]
    private IEnumerator TownDistanceLoop() //finds the distance to the closet town to see if the game should play fighting music
    { // this is inside of a Ienumerator to stop lag
        yield return new WaitForSeconds(1f);
        _start = true;
        List<GameObject> towns = new List<GameObject>(GameObject.FindGameObjectsWithTag("Town")); //finds the Game objects with the tag Town
        float closestTown = Mathf.Infinity;

        for (int i = 0; i < towns.Count; i++)
        {
            try
            {
                float distance = Vector3.Distance(towns[i].transform.position, FindAnyObjectByType<PlayerMovement>().gameObject.transform.position);
                if (distance < closestTown)
                {
                    closestTown = distance;
                }
            }
            catch { } // stop errors because when the player is dead there will be no PlayerMovement script to find

        }

        if (closestTown < townMusicDistance)
        {
            _townClose = true;
        }
        else
        {
            _townClose = false;
        }
        StartCoroutine(TownDistanceLoop());
    }
    private IEnumerator FadeMusic(AudioClip audioClip)
    {
        float baseVolume = audioSource.volume;

        SaveTimePosition(audioSource.clip, audioSource.time);

        _fading = true;
        while(audioSource.volume > 0) // decreases volume
        {
            audioSource.volume -= 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }

        audioSource.clip = audioClip;
        audioSource.time = GetTimePosition(audioClip);
        audioSource.Play();

        while (audioSource.volume <= baseVolume) // increases volume
        {
            audioSource.volume += 0.01f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        audioSource.volume = baseVolume; //resets it
        _fading = false;
    }

    private void SaveTimePosition(AudioClip clip, float timePosition)
    {
        if (clip == menuMusic)
            _menuMusicTimePosition = timePosition;
        else if (clip == backgroundMusic)
            _backgroundMusicTimePosition = timePosition;
        else if (clip == mainBossFightMusic)
            _mainBossFightMusicTimePosition = timePosition;
        else if (clip == fightingMusic)
            _fightingMusicTimePosition = timePosition;
        else if (clip == nearTownMusic)
            _nearTownMusicTimePosition = timePosition;
    }

    private float GetTimePosition(AudioClip clip)
    {
        if (clip == menuMusic)
            return _menuMusicTimePosition;
        else if (clip == backgroundMusic)
            return _backgroundMusicTimePosition;
        else if (clip == mainBossFightMusic)
            return _mainBossFightMusicTimePosition;
        else if (clip == fightingMusic)
            return _fightingMusicTimePosition;
        else if (clip == nearTownMusic)
            return _nearTownMusicTimePosition;

        return 0f; // Default for unknown clips
    }

}
