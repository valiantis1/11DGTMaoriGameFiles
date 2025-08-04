using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UIManager : MonoBehaviour
{
    private Image Panel;
    public bool Fading;
    private bool CanTitleFade = true;
    private bool CanStartMenuFade;

    [SerializeField] private float TitleShowTime;

    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject StoryBackGround;
    [SerializeField] private List<GameObject> Stories;
    void Awake()
    {
        Fading = true;
        Panel = GetComponent<Image>();
        Panel.color = new Color(0, 0, 0, 1);
        TitleScreen.SetActive(true);
        StartCoroutine(FadeOutAll(Panel));
    }

    private IEnumerator FadeOutAll(Image image)
    {
        Fading = true;
        while (image.color.a >= 0)
        {
            Panel.color = new Color(0, 0, 0, (image.color.a - 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
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
        Fading = true;
        while (image.color.a <= 1)
        {
            image.color = new Color(0, 0, 0, (image.color.a + 0.03f));
            yield return new WaitForSeconds(0.01f);
        }
        Fading = false;

        if (CanTitleFade)
        {
            TitleScreen.SetActive(false);
            //CanStartMenuFade = true;
            //StoryBackGround.SetActive(true);
            StartCoroutine(FadeOutAll(Panel));
            CanTitleFade = false;
        }
    }
}
