using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Candy : MonoBehaviour
{
    public static event Action<Collider2D> OnCandyCollision;

    private Rigidbody2D _rb2D;
    //public List<Rope> AttachedRopes = new();

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

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
            StarController.IncreaseStars();
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Candy] Lỗi khi xử lý va chạm với Star Object: {collision.name} - {ex.Message}");
        }
    }

    // public void AttachRope(Rope rope)
    // {
    //     if (!AttachedRopes.Contains(rope))
    //         AttachedRopes.Add(rope);
    // }
    //
    // public void DetachRope(Rope rope)
    // {
    //     if (AttachedRopes.Contains(rope))
    //         AttachedRopes.Remove(rope);
    // }

    public void SetBalloonState(bool isActive, float balloonSpeed = 0f)
    {
        if (isActive)
        {
            AddForceIfTriggerBalloon(balloonSpeed);
        }
        else
        {
            AddForceIfDestroyBalloon();
        }
    }

    private void AddForceIfTriggerBalloon(float balloonSpeed)
    {
        _rb2D.gravityScale = -0.25f;
        _rb2D.AddForce(Vector3.up * balloonSpeed, ForceMode2D.Force);
    }

    private void AddForceIfDestroyBalloon()
    {
        _rb2D.gravityScale = 1f;
    }
}