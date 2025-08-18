using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons = new List<GameObject>();
    [SerializeField] private List<GameObject> gates = new List<GameObject>();
    private bool IsAButtonPressed;
    public void Pressed(GameObject ButtonPressed)
    {
        IsAButtonPressed = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            if(ButtonPressed.name == buttons[i].name)
            {
                buttons[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            if(buttons[i].GetComponent<BoxCollider2D>().enabled == true)
            {
                IsAButtonPressed = true;
            }
        }
        if(!IsAButtonPressed)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                gates[i].SetActive(false);
            }
        }
    }
}
