using System;
using UnityEngine;

public class Button : MonoBehaviour
{
    private ButtonManager _buttonManager;
    [SerializeField] private Sprite buttonDown;

    private void Awake()
    {
        _buttonManager = GetComponentInParent<ButtonManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = buttonDown;
        transform.position= new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        _buttonManager.Pressed(gameObject);
    }
}
