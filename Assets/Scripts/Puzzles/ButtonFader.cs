using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonFader : MonoBehaviour
{
    private List<GameObject> Gates;
    [SerializeField] private AudioSource audioSource;

    [System.Obsolete]
    void Awake()
    {
        Gates = new List<GameObject>(GameObject.FindGameObjectsWithTag("Gate"));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Gates.Count; i++)
        {
            if (Gates[i].GetComponent<BoxCollider2D>().enabled) //checks if the animation has been played.
            {
                if (!Gates[i].activeSelf)
                {
                    audioSource.Play(); //plays unlocking sound
                    Gates[i].SetActive(true);
                    Gates[i].GetComponent<Animator>().Play("ButtonFade"); //this turns of the BoxCollider2D and hides the gate
                }
            }
        }
    }
}
