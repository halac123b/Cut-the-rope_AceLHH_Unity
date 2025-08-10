using UnityEngine;
using UnityEngine.InputSystem;

public class Balloon : MonoBehaviour
{
    public Camera mainCamera;
    
    private float _balloonSpeed = 0.05f;
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
            candy.SetStaticGravity(); 
            transform.SetParent(candy.transform);
            _isCarryCandy = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Candy candy = collision.GetComponent<Candy>();

        if (candy != null)
        { 
            candy.SetBalloonState(true, _balloonSpeed);
            transform.position = candy.transform.position;
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
        Destroy(gameObject);
        
        _isCarryCandy = false;
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

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Balloon") || hit.collider.gameObject.CompareTag("Candy"))
            {
                PopBalloon();
            }
        }
    }
}