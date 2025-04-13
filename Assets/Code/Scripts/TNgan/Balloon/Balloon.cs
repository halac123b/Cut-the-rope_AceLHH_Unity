using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Camera mainCamera;
    
    private float _balloonSpeed = 13f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Candy candy = collision.GetComponent<Candy>();
        if (candy != null)
        {
            transform.SetParent(candy.transform);
            candy.SetBalloonState(true, _balloonSpeed );
        }
    }

    public void PopBalloon()
    {
        Candy candy = transform.parent.GetComponent<Candy>();
        if (candy == null)
        {
            return;
        }
        candy.SetBalloonState(false);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Balloon"))
            {
                PopBalloon();
            }
        }
    }
}