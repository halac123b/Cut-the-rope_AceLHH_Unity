using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
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
    [SerializeField] private ParticleSystem _candyParticlesCollisionSpike;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        Animator = GetComponent<Animator>();
        EventDispatcher.Instance.AddEvent(gameObject, _ => PlayAnimationCollisionWithSpike(),
            EventDispatcher.TriggerSpike);
    }

    private void Update()
    {
        Vector3 viewPos = _mainCamera.WorldToViewportPoint(transform.position);

        if ((viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) && AttachedRopes.Count <= 0)
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

        if ( UIController.Instance.IsCreatedLevel && gameObject != null && (UserProfile.Instance.ScrollLevelData.IsScrollLevelHorizontal || UserProfile.Instance.ScrollLevelData.IsScrollLevelVertical))
        {
            float posX = Mathf.Clamp(transform.position.x, 0f, UserProfile.Instance.PosFrog.x);
            float posY = Mathf.Clamp(transform.position.y, 0f, UserProfile.Instance.PosFrog.y);
            _mainCamera.transform.position = UserProfile.Instance.ScrollLevelData.IsScrollLevelHorizontal ? new Vector3(posX, 0f, -10f) : new Vector3(0f, posY, -10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StarEffect starEffect = collision.GetComponent<StarEffect>();
        if (collision.CompareTag("Star"))
        {
            Debug.Log($"[Candy] Candy va chạm với Star Object: {collision.name}");
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.OnIncreaseStar);
            EventDispatcher.Instance.Dispatch(null, EventDispatcher.CollectStar);
            PlayAnimationTriggerStar();
            starEffect.PlayDisappearAnimation();
        }
    }

    private void PlayAnimationCollisionWithSpike()
    {
        ParticleSystem ps = Instantiate(_candyParticlesCollisionSpike, transform.position, Quaternion.identity,
            transform.parent);
        ps.Play();
        EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.LevelFail);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    private void PlayAnimationTriggerStar()
    {
        LightSprite.enabled = true;
        Animator.SetTrigger("TriggerStar");
    }

    public void AfterTriggerStar()
    {
        LightSprite.enabled = false;
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

    public void SetBubbleState(bool isActive, float bubbleSpeed = 0f)
    {
        if (isActive)
        {
            AddForceIfTriggerBubble(bubbleSpeed);
        }
        else
        {
            AddForceIfDestroyBubble();
        }
    }

    private void AddForceIfTriggerBubble(float bublleSpeed)
    {
        _rb2D.gravityScale = -0.15f;

        if (_rb2D.linearVelocityY < 0.25f)
        {
            _rb2D.AddForce(Vector3.up * bublleSpeed, ForceMode2D.Impulse);
        }

        _rb2D.linearVelocity *= 0.5f;
        _rb2D.linearDamping = 1.3f;
        StartCoroutine(ResetDamping());
    }

    private IEnumerator ResetDamping()
    {
        yield return new WaitForSeconds(0.5f);
        _rb2D.linearDamping = 0.5f; // or your default value
    }

    private void AddForceIfDestroyBubble()
    {
        _rb2D.gravityScale = 1f;
    }

    public void FadeAllRopes()
    {
        foreach (Rope rope in AttachedRopes)
        {
            rope.StartFadeOut();
        }
    }

    public void BeEaten(Vector3 pos)
    {
        _rb2D.bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;
        float duration = 0.15f;

        LMotion.Create(transform.position, pos, duration)
            .WithEase(Ease.OutQuad)
            .BindToPosition(transform).AddTo(gameObject);

        LMotion.Create(transform.localScale, Vector3.zero, duration)
            .BindToLocalScale(transform).AddTo(gameObject);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        LMotion.Create(sr.color.a, 0f, duration)
        .WithEase(Ease.OutQuad)
        .Bind(alpha =>
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        })
        .AddTo(gameObject);
    }
}
