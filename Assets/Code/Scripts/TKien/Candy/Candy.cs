using System;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public static event Action<Collider2D> OnCandyCollision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Kien] Bro, Candy va chạm với Start Object: {collision.name}");
            OnCandyCollision?.Invoke(collision);
        }
    }

    public void OnEnable()
    {
        OnCandyCollision += HandleCandyCollision;
    }

    public void OnDisable()
    {
        OnCandyCollision -= HandleCandyCollision;
    }

    private void HandleCandyCollision(Collider2D collision)
    {
        try
        {
            Debug.Log($"[Kien] Bro, Candy va chạm với Start Object: {collision.name}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Kien] Bro,Lỗi Candy va chạm với Start Object: {collision.name}");
        }
    }
}