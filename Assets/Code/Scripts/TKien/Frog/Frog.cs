using System;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public static event Action<Collider2D> OnCandyCollision;

    private void OnEnable()
    {
        OnCandyCollision += HandleFrogCollision;
    }

    private void OnDisable()
    {
        OnCandyCollision -= HandleFrogCollision;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Debug.Log($"[Frog] Frog ăn Candy Object: {collision.name}");
            OnCandyCollision?.Invoke(collision);
            Destroy(collision.gameObject, 0.015f);
        }
    }

    private void HandleFrogCollision(Collider2D collision)
    {   
        try
        {
            Debug.Log($"[Frog] Xử lý va chạm, Frog ăn Candy Object: {collision.name}");
            int stars = StarController.GetStarsInGameplay();
            string levelIndex = UserProfile.Instance.SelectedLevelIndex;
            UserProfile.Instance.SaveStars(levelIndex, stars);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Frog] Lỗi khi Frog ăn Candy Object: {collision.name} - {ex.Message}");
        }
    }
}