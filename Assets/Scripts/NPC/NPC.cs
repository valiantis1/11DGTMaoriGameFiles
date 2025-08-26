using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] private bool imageAndText; // when on will shows both images and text
    [SerializeField] private bool invisTalking; // when on means that a god is talking

    [SerializeField] private bool godTalking; // is godTalking and invisTalking is true then the god is talking. But if only the invisTalking is true then the narrator is talking

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject imageBox;
    [SerializeField] private GameObject imageBoxBackground;
    [SerializeField] private GameObject askTalkText;
    [SerializeField] private GameObject exclamationMark;
    public bool Talking; // when this is on other scripts can see it and stop moving, stop pausing, etc.
    [SerializeField] private float waitTime; // time between letters show

    [SerializeField] private string nameOfNpc;
    [SerializeField] List<String> talkingLines = new List<String>();
    [SerializeField] List<Sprite> changeImage = new List<Sprite>(); // this will have the same amount as the talkinglines. But if i want the image to change i will put an image where the line will be.

    [NonSerialized] public bool StopTalk;
    private bool _talkedBefore; //this is used for the gods because i only want them to talk once
    private bool _invisCanTalk;


    // Update is called once per frame
    void Update()
    {
        if(askTalkText.activeSelf && !Talking && Input.GetKeyDown(KeyCode.E) && !FindAnyObjectByType<Pause>().Paused && !invisTalking)
        {
            Talking = true;
            exclamationMark.SetActive(false);
            askTalkText.SetActive(false);
            textBox.SetActive(true);
            nameText.text = nameOfNpc + ":";
            if (imageAndText)
            {
                imageBox.SetActive(true);
                StartCoroutine(ImageAndTalk());
                return;
            }
            StartCoroutine(Talk());
        }
        if(invisTalking && !Talking && !_talkedBefore && _invisCanTalk)
        {
            Talking = true;
            _talkedBefore = true;
            nameText.text = nameOfNpc + ":";
            StartCoroutine(InvisTalk());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Talking || invisTalking) 
        {
            _invisCanTalk = true;
            return; 
        }
        askTalkText.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        askTalkText.SetActive(false);
    }

    private IEnumerator Talk()
    {
        for (int i = 0; i < talkingLines.Count; i++)
        {
            text.maxVisibleCharacters = 0;
            text.text = talkingLines[i];
            for (int j = 0; j <= talkingLines[i].Length; j++)
            {
                text.maxVisibleCharacters = j;
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitUntil(NextPage);
        }
        Talking = false;
        textBox.SetActive(false);
    }

    private IEnumerator ImageAndTalk()
    {
        bool ImageOn = false;
        Image image = imageBox.GetComponent<Image>(); //makes the code look shorter, also might be less on cpu.

        for (int i = 0; i < talkingLines.Count; i++) // runs the code for each line.
        {
            if (changeImage[i] != null) // checks if there is an image to swap to.
            {
                if(ImageOn) //This is to see if i am swaping from image to image, or no image to image.
                {
                    imageBoxBackground.SetActive(true);
                    while (image.color.a >= 0) //fades the image
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
                        yield return new WaitForSeconds(0.03f);
                    }
                }
                image.sprite = changeImage[i];
                while (image.color.a <= 1) //fades the image
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a + 0.03f));
                    yield return new WaitForSeconds(0.03f);
                }
                ImageOn = true;
            }

            text.maxVisibleCharacters = 0; // hides the text
            text.text = talkingLines[i];
            for (int j = 0; j < talkingLines[i].Length; j++)
            {
                text.maxVisibleCharacters = j; // one by one shows the letters
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitUntil(NextPage);
        }
        if (ImageOn)
        {
            textBox.SetActive(false);
            while (image.color.a >= 0) // hides image
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
                yield return new WaitForSeconds(0.03f);
            }
            image = imageBoxBackground.GetComponent<Image>();
            while (image.color.a >= 0) // hides image
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
                yield return new WaitForSeconds(0.03f);
            }
            imageBoxBackground.SetActive(false);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }
        yield return new WaitForSeconds(0.5f);
        Talking = false;
    }
    private IEnumerator InvisTalk()
    {
        Image image = null;
        Color oldColor = Color.white;
        if (godTalking)
        {
            oldColor = imageBoxBackground.GetComponent<Image>().color;
            image = imageBoxBackground.GetComponent<Image>();
            image.color = new Color(1, 1, 1, 0);
            imageBoxBackground.SetActive(true);

            while (image.color.a <= 1) //fades the image
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a + 0.03f));
                yield return new WaitForSeconds(0.03f);
            }
        }

        textBox.SetActive(true);

        for (int i = 0; i < talkingLines.Count; i++)
        {
            text.maxVisibleCharacters = 0;
            text.text = talkingLines[i];
            for (int j = 0; j <= talkingLines[i].Length; j++)
            {
                text.maxVisibleCharacters = j;
                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitUntil(NextPage);
        }

        textBox.SetActive(false);
        Talking = false;

        if (!godTalking) { yield break; }

        while (image.color.a >= 0) // hides image
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
            yield return new WaitForSeconds(0.03f);
        }
        imageBoxBackground.SetActive(false);
        image.color = oldColor;
    }

    private bool NextPage()
    {
        if(Input.anyKey || Input.GetMouseButtonDown(0))
        {
            return true;
        }
        return false;
    }
}
