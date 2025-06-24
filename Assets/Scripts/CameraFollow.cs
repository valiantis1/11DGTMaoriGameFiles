using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject PlayerPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(PlayerPos.transform.position.x, PlayerPos.transform.position.y, transform.position.z);
    }
}
