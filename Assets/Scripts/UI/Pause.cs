using UnityEngine;

public class Pause : MonoBehaviour
{
    public bool Paused;
    public GameObject PausedUI;

    void Update()
    {
        if(FindAnyObjectByType<PlayerHealth>() == null) { return; }

        if(Input.GetKeyDown(KeyCode.Escape) && !FindAnyObjectByType<PlayerHealth>().IsDead && FindAnyObjectByType<PlayerHealth>().CanRespawn && !FindAnyObjectByType<UIManager>().Fading && !FindAnyObjectByType<NPC>().talking)
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
        Time.timeScale = 0;
        PausedUI.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Paused = false;
        PausedUI.SetActive(false);
    }
    public void Credits()
    {
        print("Credits");
    }
    public void Restart()
    {
        FindAnyObjectByType<PlayerHealth>().Death();
        Resume();
        Paused = false;
    }
    public void Quit()
    {
        print("Quit");
        Application.Quit();
    }
}
