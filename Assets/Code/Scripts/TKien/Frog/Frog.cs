using UnityEngine;

public class Frog : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Kien], Frog va chạm với Start Object: {collision.name}");
            HandleStartCollision(collision);
        }
    }

    private void HandleStartCollision(Collider2D collision)
    {
        Debug.Log($"[Kien], Xử Frog va chạm với Start Object: {collision.name}");
    }
}
