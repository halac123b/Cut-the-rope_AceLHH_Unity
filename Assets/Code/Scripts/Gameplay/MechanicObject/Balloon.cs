using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Balloon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private Camera _mainCamera;
    
    private float _balloonSpeed = 5f;
    private bool _isCarryCandy;
    private float _speed = 5f;
    
    [SerializeField] private SpriteRenderer _balloonSprite; 
    private float _travelDistance = 2f;
    private Coroutine _balloonRoutine;

    private void ShootBalloon()
    {
        if (_balloonRoutine != null)
        {
            StopCoroutine(_balloonRoutine);
        }
        
        _balloonSprite.transform.position = transform.position;
        _balloonSprite.gameObject.SetActive(true);
        _animator.enabled = true;

        _balloonRoutine = StartCoroutine(MoveBalloon(_balloonSprite.transform, Vector2.left));
    }

    private IEnumerator MoveBalloon(Transform balloon, Vector2 dir)
    {
        Vector3 startPos = balloon.position;
        Vector3 startScale = Vector3.zero; 
        Vector3 targetScale = new Vector3(2f, 2f, 2f);     
        float growDuration = 0.3f;      
        float elapsed = 0f;

        balloon.localScale = startScale;

        while (balloon != null && balloon.gameObject.activeSelf)
        {
            balloon.Translate(dir.normalized * _balloonSpeed * Time.deltaTime);
            if (elapsed < growDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / growDuration);
                balloon.localScale = Vector3.Lerp(startScale, targetScale, t);
            }

            if (Vector3.Distance(startPos, balloon.position) >= _travelDistance)
            {
                balloon.gameObject.SetActive(false);
                yield break;
            }

            yield return null;
        }
    }
    
    private void Start()
    {
        _mainCamera = Camera.main;
        _isCarryCandy = false;
        _animator.enabled = false;
    }

    private void PlayAnimationBalloon()
    {
        _animator.SetTrigger("Balloon");
        ShootBalloon();
    }

    private void DestroyBalloon()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Balloon"))
            {
                PlayAnimationBalloon();
            }
        }
    }
}