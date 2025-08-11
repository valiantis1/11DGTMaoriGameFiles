using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image Panel;
    public bool Fading; // 'Fading' is made for the other scripts (eg. so the player can move while the screen is fading colours.)
    private bool CanTitleFade = true;

    private bool Death;

    [SerializeField] private float deathWait;

    [SerializeField] private float TitleShowTime;

    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject StoryBackGround;
    [SerializeField] private List<GameObject> Stories;

    private PlayerDeathManager playerdeathmanager;
    void Awake()
    {
        playerdeathmanager = FindAnyObjectByType<PlayerDeathManager>();
        Fading = true;
        Panel = GetComponent<Image>();
        Panel.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 1); //this is the perfect colour to fit in with the background of the starting loading screen.
        TitleScreen.SetActive(true);
        StartCoroutine(FadeOutAll(Panel));
    }

    private IEnumerator FadeOutAll(Image image)
    {
        Fading = true;
        while (image.color.a >= 0)
        {
            Panel.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, (image.color.a - 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
        image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 0);
        Fading = false;

        if(CanTitleFade)
        {
            Fading = true;
            yield return new WaitForSeconds(TitleShowTime);
            StartCoroutine(FadeInAll(Panel));
        }
    }

    private IEnumerator FadeInAll(Image image)
    {
        if (Death)
            yield return new WaitForSeconds(deathWait);
        Fading = true;
        while (image.color.a <= 1)
        {
            image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, (image.color.a + 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
        image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 1);
        Fading = false;

        if (CanTitleFade)
        {
            TitleScreen.SetActive(false);
            StartCoroutine(FadeOutAll(Panel));
            CanTitleFade = false;
        }
        if(Death)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(FadeOutAll(image));
            playerdeathmanager.SpawnPlayer();
            Death = false;
        }
    }

    public void PlayerDeath()
    {
        if(Death) { return; }
        Death = true;
        StartCoroutine(FadeInAll(Panel));
    }
}
