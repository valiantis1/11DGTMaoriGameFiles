using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject Player;
    public float Smoothing;

    private void Awake()
    {
        Player = FindAnyObjectByType<PlayerMovement>().gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Makes errors not happen because when the player is deleted it still tries to access the player.
        if (Player != null)
        {
            //because I move the player a little when its dead, (to make the animation in the right spot) the camera moves.
            if(Player.GetComponent<PlayerHealth>().IsDead) { return; }
            //makes a vector3 of the player pos (but not the z or it breaks)
            Vector3 PlayerTrans = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
            //smoothly moves the camera
            transform.position = Vector3.Lerp(transform.position, PlayerTrans, Smoothing * Time.deltaTime);
        }
    }
}
