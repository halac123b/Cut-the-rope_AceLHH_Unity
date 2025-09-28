using UnityEngine;

public class BalloonCollision : MonoBehaviour
{
    [SerializeField] private float pushForce = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Candy"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 pushDir = (collision.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            }

            gameObject.SetActive(false); 
        }
    }
}