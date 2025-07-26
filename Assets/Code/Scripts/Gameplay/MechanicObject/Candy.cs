using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Candy : MonoBehaviour
{
    //public static event Action<Collider2D> OnCandyCollision;
    public List<Rope> AttachedRopes = new();

    private Rigidbody2D _rb2D;
    private Camera _mainCamera;
    

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(transform.position);

        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1 && AttachedRopes.Count <= 0)
        {
            EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.RestartLevel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Candy] Candy va chạm với Star Object: {collision.name}");
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnIncreaseStar);
            Destroy(collision.gameObject);
        }
    }
    private void OnDestroyCandy(object obj)
    {
        if (this == null || gameObject == null)
        {
            return;
        }

        StartCoroutine(DelayDeactivate());
    }
    
    private void OnEnable()
    {
        EventDispatcher.Instance.AddEvent(gameObject, OnDestroyCandy, EventDispatcher.DestroyCandy);
    }
    
    private IEnumerator DelayDeactivate()
    {
        yield return null;
        gameObject.SetActive(false);
    }

    // private void OnEnable()
    // {
    //     OnCandyCollision += HandleCandyCollision;
    // }
    //
    // private void OnDisable()
    // {
    //     OnCandyCollision -= HandleCandyCollision;
    // }

    // private void HandleCandyCollision(Collider2D collision)
    // {
    //     try
    //     {
    //         Debug.Log($"[Candy] Xử lý va chạm với Star Object: {collision.name}");
    //         EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnIncreaseStar);
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogError($"[Candy] Lỗi khi xử lý va chạm với Star Object: {collision.name} - {ex.Message}");
    //     }
    // }

    public void AttachRope(Rope rope)
    {
        if (!AttachedRopes.Contains(rope))
            AttachedRopes.Add(rope);
    }
    
    public void DetachRope(Rope rope)
    {
        if (AttachedRopes.Contains(rope))
            AttachedRopes.Remove(rope);
    }

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