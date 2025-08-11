using UnityEngine;

public class Button : MonoBehaviour
{
    private ButtonManager buttonManager;
    public Sprite ButtonDown;

    private void Awake()
    {
        buttonManager = GetComponentInParent<ButtonManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = ButtonDown;
        transform.position= new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
        buttonManager.Pressed(gameObject);
    }
}
