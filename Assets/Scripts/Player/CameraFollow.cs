using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject PlayerPos;
    public float Smoothing;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 PlayerTrans = new Vector3(PlayerPos.transform.position.x, PlayerPos.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, PlayerTrans, Smoothing * Time.deltaTime);
    }
}
