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
    [SerializeField] private bool ImageAndText;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject TextBox;
    [SerializeField] private GameObject ImageBox;
    [SerializeField] private GameObject ImageBoxBackground;
    [SerializeField] private GameObject AskTalkText;
    [SerializeField] private GameObject ExclamationMark;
    public bool talking;
    [SerializeField] private float WaitTime;

    [SerializeField] List<String> TalkingLines = new List<String>();
    [SerializeField] List<Sprite> ChangeImage = new List<Sprite>(); // this will have the same amount as the talkinglines. But if i want the image to change i will put an image where the line will be.

    [NonSerialized] public bool StopTalk;

    // Update is called once per frame
    void Update()
    {
        if(AskTalkText.activeSelf && !talking && Input.GetKeyDown(KeyCode.E) && !FindAnyObjectByType<Pause>().Paused)
        {
            talking = true;
            ExclamationMark.SetActive(false);
            AskTalkText.SetActive(false);
            TextBox.SetActive(true);
            if(ImageAndText)
            {
                ImageBox.SetActive(true);
                StartCoroutine(ImageAndTalk());
                return;
            }
            StartCoroutine(Talk());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(talking) { return; }
        AskTalkText.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        AskTalkText.SetActive(false);
    }

    private IEnumerator Talk()
    {
        for (int i = 0; i < TalkingLines.Count; i++)
        {
            Text.maxVisibleCharacters = 0;
            Text.text = TalkingLines[i];
            for (int j = 0; j < TalkingLines[i].Length; j++)
            {
                Text.maxVisibleCharacters = j;
                yield return new WaitForSeconds(WaitTime);
            }
            yield return new WaitUntil(NextPage);
        }
        talking = false;
        TextBox.SetActive(false);
    }

    private IEnumerator ImageAndTalk()
    {
        bool ImageOn = false;
        Image image = ImageBox.GetComponent<Image>(); //makes the code look shorter, also might be an FPS boost.

        for (int i = 0; i < TalkingLines.Count; i++) // runs the code for each line.
        {
            if (ChangeImage[i] != null) // checks if there is an image to swap to.
            {
                if(ImageOn) //This is to see if i am swaping from image to image, or no image to image.
                {
                    ImageBoxBackground.SetActive(true);
                    while (image.color.a >= 0) //fades the image
                    {
                        image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
                        yield return new WaitForSeconds(0.03f);
                    }
                }
                image.sprite = ChangeImage[i];
                while (image.color.a <= 1) //fades the image
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a + 0.03f));
                    yield return new WaitForSeconds(0.03f);
                }
                ImageOn = true;
            }

            Text.maxVisibleCharacters = 0; // hides the text
            Text.text = TalkingLines[i];
            for (int j = 0; j < TalkingLines[i].Length; j++)
            {
                Text.maxVisibleCharacters = j; // one by one shows the letters
                yield return new WaitForSeconds(WaitTime);
            }
            yield return new WaitUntil(NextPage);
        }
        if (ImageOn)
        {
            ImageBoxBackground.SetActive(false);
            TextBox.SetActive(false);
            while (image.color.a >= 0) // hides image
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (image.color.a - 0.03f));
                yield return new WaitForSeconds(0.03f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        talking = false;
    }

    private bool NextPage()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }
}
