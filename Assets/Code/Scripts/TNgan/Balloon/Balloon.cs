using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

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
            candy.SetBalloonState(true);
        }
    }

    public void PopBalloon()
    {
        Candy candy = transform.parent.GetComponent<Candy>();
        candy.SetBalloonState(false);
        candy.AddForceIfDestroyBalloon();
        Destroy(gameObject, 1f);
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