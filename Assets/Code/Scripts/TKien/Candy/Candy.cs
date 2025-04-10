using System;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public static event Action<Collider2D> OnCandyCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Candy] Candy va chạm với Star Object: {collision.name}");
            OnCandyCollision?.Invoke(collision);
        }
    }

    private void OnEnable()
    {
        OnCandyCollision += HandleCandyCollision;
    }

    private void OnDisable()
    {
        OnCandyCollision -= HandleCandyCollision;
    }

    private void HandleCandyCollision(Collider2D collision)
    {
        try
        {
            Debug.Log($"[Candy] Xử lý va chạm với Star Object: {collision.name}");
            Star.IncreaseStars();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Candy] Lỗi khi xử lý va chạm với Star Object: {collision.name} - {ex.Message}");
        }
    }
}