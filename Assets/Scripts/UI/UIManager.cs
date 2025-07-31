using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.PackageManager;

public class UIManager : MonoBehaviour
{
    private Image Panel;
    private bool Fading;
    private bool CanTitleFade = true;
    private bool CanStartMenuFade;

    [SerializeField] private float TitleShowTime;

    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject StartMenu;
    void Awake()
    {
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
            yield return new WaitForSeconds(TitleShowTime);
            StartCoroutine(FadeInAll(Panel));
        }
        if(CanStartMenuFade)
        {

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
            CanStartMenuFade = true;
            StartCoroutine(FadeOutAll(Panel));
            CanTitleFade = false;
        }
    }
}
