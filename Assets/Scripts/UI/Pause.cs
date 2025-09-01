using UnityEngine;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    public bool Paused;
    public GameObject PausedUI;

    [SerializeField] private AudioSource audioSource;
    void Update()
    {
        if(FindAnyObjectByType<PlayerHealth>() == null) { return; }

        if(Input.GetKeyDown(KeyCode.Escape) && !FindAnyObjectByType<PlayerHealth>().IsDead && FindAnyObjectByType<PlayerHealth>().CanRespawn && !FindAnyObjectByType<UIManager>().Fading && !FindAnyObjectByType<NPC>().Talking)
        {
            if (Paused) 
            { 
                Resume();
                Paused = false;
            }
            else
            { 
                ShowUI();
                Paused = true;
            }
        }
    }


    private void ShowUI()
    {
        PlayClickSound();
        Time.timeScale = 0;
        PausedUI.SetActive(true);
    }

    public void Resume()
    {
        PlayClickSound();
        Time.timeScale = 1;
        Paused = false;
        PausedUI.SetActive(false);
    }
    public void Credits()
    {
        PlayClickSound();
        print("Credits");
    }
    public void Restart()
    {
        PlayClickSound();
        FindAnyObjectByType<PlayerHealth>().Death();
        Resume();
        Paused = false;
    }
    public void Quit()
    {
        PlayClickSound();
        print("Quit");
        Application.Quit();
    }

    private void PlayClickSound()
    {
        // this makes the sound sound less repetitive by changing how it sounds
        audioSource.pitch = 0.8f + UnityEngine.Random.Range(0, 0.2f);
        audioSource.volume = 0.6f + UnityEngine.Random.Range(0, 0.2f);
        audioSource.Play();
    }
}
