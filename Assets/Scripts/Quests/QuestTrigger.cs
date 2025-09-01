using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private string whatPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindAnyObjectByType<Quests>().Trigger(whatPoint);
    }
}
