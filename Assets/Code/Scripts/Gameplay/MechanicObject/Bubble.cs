using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Bubble : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private Camera _mainCamera;
    
    private float _bubbleSpeed = 2f;
    private bool _isCarryCandy;

    private void Start()
    {
        _mainCamera = Camera.main;
        _isCarryCandy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Candy candy = collision.GetComponent<Candy>();

        if (candy != null)
        {
            transform.SetParent(candy.transform);
            transform.localPosition = Vector3.zero;
            candy.SetBubbleState(true, _bubbleSpeed);
            _animator.SetTrigger("candy");
            _isCarryCandy = true;
        }
    }

    private void PopBubble()
    {
        Candy candy = transform.parent.GetComponent<Candy>();
        
        if (candy == null)
        {
            return;
        }

        candy.SetBubbleState(false);
        _isCarryCandy = false;
        
        _animator.SetTrigger("pop");
    }

    private void DestroyBubble()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (!_isCarryCandy)
        {
            return;
        }

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 mouseWorldPos = _mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider == null)
            {
                return;
            }

            if (hit.collider.gameObject.CompareTag("Bubble") || hit.collider.gameObject.CompareTag("Candy"))
            {
                PopBubble();
            }
        }
    }
}