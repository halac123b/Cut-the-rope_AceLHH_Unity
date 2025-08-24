using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Balloon : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private Camera mainCamera;
    
    private float _balloonSpeed = 2f;
    private bool _isCarryCandy;

    private void Start()
    {
        mainCamera = Camera.main;
        _isCarryCandy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Candy candy = collision.GetComponent<Candy>();

        if (candy != null)
        {
            transform.SetParent(candy.transform);
            transform.localPosition = Vector3.zero;
            candy.SetBalloonState(true, _balloonSpeed);
            _animator.SetTrigger("candy");
            _isCarryCandy = true;
        }
    }

    private void PopBalloon()
    {
        Candy candy = transform.parent.GetComponent<Candy>();
        
        if (candy == null)
        {
            return;
        }

        candy.SetBalloonState(false);
        _isCarryCandy = false;
        
        _animator.SetTrigger("pop");
    }

    private void DestroyBalloon()
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
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider == null)
            {
                return;
            }

            if (hit.collider.gameObject.CompareTag("Balloon") || hit.collider.gameObject.CompareTag("Candy"))
            {
                PopBalloon();
            }
        }
    }
}