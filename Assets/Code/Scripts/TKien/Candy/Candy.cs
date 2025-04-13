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