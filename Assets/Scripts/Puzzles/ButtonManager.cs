using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Buttons = new List<GameObject>();
    [SerializeField] private List<GameObject> Gates = new List<GameObject>();
    private bool IsAButtonPressed;
    public void Pressed(GameObject ButtonPressed)
    {
        IsAButtonPressed = false;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if(ButtonPressed.name == Buttons[i].name)
            {
                Buttons[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            if(Buttons[i].GetComponent<BoxCollider2D>().enabled == true)
            {
                IsAButtonPressed = true;
            }
        }
        if(!IsAButtonPressed)
        {
            for (int i = 0; i < Gates.Count; i++)
            {
                Gates[i].SetActive(false);
            }
        }
    }
}
