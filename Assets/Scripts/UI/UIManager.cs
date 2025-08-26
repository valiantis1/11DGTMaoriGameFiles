using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Image _panel;
    public bool Fading; // 'Fading' is made for the other scripts (eg. so the player cant move while the screen is fading)
    private bool _canTitleFade = true;
    private bool _death;
    [SerializeField] private float deathWait;

    [SerializeField] private float titleShowTime;
    [SerializeField] private GameObject titleScreen;

    private PlayerDeathManager playerdeathmanager;
    void Awake()
    {
        playerdeathmanager = FindAnyObjectByType<PlayerDeathManager>();
        Fading = true;
        _panel = GetComponent<Image>();
        _panel.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 1); //this is the perfect colour to fit in with the background of the starting loading screen.
        titleScreen.SetActive(true);
        StartCoroutine(FadeOutAll(_panel));
    }

    private IEnumerator FadeOutAll(Image image) //fades out the screen, also stops player movement
    {
        Fading = true;
        while (image.color.a >= 0)
        {
            _panel.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, (image.color.a - 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
        image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 0);
        Fading = false;

        if(_canTitleFade)
        {
            Fading = true;
            yield return new WaitForSeconds(titleShowTime); //lets the title screen show for longer
            StartCoroutine(FadeInAll(_panel));
        }
    }

    private IEnumerator FadeInAll(Image image) //fades in the screen, also stops player movement
    {
        if (_death)
            yield return new WaitForSeconds(deathWait); // this gives time for the hearts be deleted

        Fading = true;
        while (image.color.a <= 1)
        {
            image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, (image.color.a + 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
        image.color = new Color(0.1372549f, 0.1215686f, 0.1254902f, 1);
        Fading = false;

        if (_canTitleFade)
        {
            titleScreen.SetActive(false);
            StartCoroutine(FadeOutAll(_panel));
            _canTitleFade = false;
        }
        if(_death) // if the player is dea this will play
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(FadeOutAll(image));
            playerdeathmanager.SpawnPlayer();
            _death = false;
        }
    }

    public void PlayerDeath()
    {
        if(_death) { return; }
        _death = true;
        StartCoroutine(FadeInAll(_panel));
    }
}
