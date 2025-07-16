using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private CapsuleCollider2D Collider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        RaycastHit2D hit =
          Physics2D.BoxCast(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
          new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z),
          0, Vector2.left, 0, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //draws a debug hitbox that can only been seen in the scene window
        Gizmos.DrawWireCube(Collider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
          new Vector3(Collider.bounds.size.x * range, Collider.bounds.size.y, Collider.bounds.size.z));
    }
}
