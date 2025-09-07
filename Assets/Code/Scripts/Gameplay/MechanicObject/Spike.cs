using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log($"[Spike] Spike va chạm với Candy Object: {collision.name}");
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.TriggerSpike);
        }
    }
}
