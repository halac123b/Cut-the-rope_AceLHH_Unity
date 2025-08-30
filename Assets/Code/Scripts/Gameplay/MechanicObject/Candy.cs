using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Candy : MonoBehaviour
{
    //public static event Action<Collider2D> OnCandyCollision;
    public List<Rope> AttachedRopes = new();
    public SpriteRenderer LightSprite;
    public Animator Animator;
    private Rigidbody2D _rb2D;
    private Camera _mainCamera;
    private float _tutorialTriggerYLevel05 = 1.6f;
    private float _tutorialTriggerXLevel05 = 0.4f;
    private float _stayTimer;
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        Animator = GetComponent<Animator>();
        EventDispatcher.Instance.AddEvent(gameObject, _ => PlayAnimationCollisionWithSpike(), EventDispatcher.TriggerSpike);
    }

    private void Update()
    {
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(transform.position);

        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1 && AttachedRopes.Count <= 0)
        {
            EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LevelFail);
            return;
        }
        
        if (transform.position.y > _tutorialTriggerYLevel05 && transform.position.y < _tutorialTriggerYLevel05 + 0.3f &&
            transform.position.x > _tutorialTriggerXLevel05 && transform.position.x < _tutorialTriggerXLevel05 + 0.3f)
        {
            int _tutorialId = 10;
            EventDispatcher.Instance.Dispatch(_tutorialId, EventDispatcher.TriggerTutorial);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StarEffect starEffect = collision.GetComponent<StarEffect>();
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Candy] Candy va chạm với Star Object: {collision.name}");
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnIncreaseStar);
            PlayAnimationTriggerStar();
            starEffect.PlayDisappearAnimation();
        }
    }

    private void PlayAnimationCollisionWithSpike()
    {
        if (this == null || gameObject == null) return;
        
        StartCoroutine(RemoveEventNextFrame());
    
        // run particle collision with spike here
        Destroy(gameObject);
        EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LevelFail);
    }

    private IEnumerator RemoveEventNextFrame()
    {
        yield return null;
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }
    
    public void PlayAnimationTriggerStar()
    {
        LightSprite.enabled = true;
        Animator.SetTrigger("TriggerStar");
    }

    public void AfterTriggerStar()
    {
        LightSprite.enabled = false;
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
        _rb2D.gravityScale = -0.15f;

        if (_rb2D.linearVelocityY < 0.25f)
        {
            _rb2D.AddForce(Vector3.up * balloonSpeed, ForceMode2D.Impulse);
        }
    }

    private void AddForceIfDestroyBalloon()
    {
        _rb2D.gravityScale = 1f;
    }
}