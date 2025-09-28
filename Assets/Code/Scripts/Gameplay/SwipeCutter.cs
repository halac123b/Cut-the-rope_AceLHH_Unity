using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TrailRenderer))]
public class SwipeCutter : MonoBehaviour
{
    private TrailRenderer _trail;   // already on GameObject
    private Camera _cam;

    private PlayerInput _playerInput;
    private InputAction _pointAction;
    private InputAction _clickAction;

    private bool _isSwiping;
    private Vector2 _lastPos;
    
    private void Awake()
    {
        _cam = Camera.main;
        
        _trail = GetComponent<TrailRenderer>();
        _playerInput = GetComponent<PlayerInput>(); // Requires PlayerInput component
        _pointAction = _playerInput.actions["Point"];
        _clickAction = _playerInput.actions["Click"];
    }

    private void OnEnable()
    {
        _clickAction.started += OnClickStart;
        _clickAction.canceled += OnClickEnd;
    }

    private void Update()
    {
        if (!_isSwiping)
            return;

        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f)); // z=10 to place in front of cam

        // Move the trail object to follow finger
        _trail.transform.position = worldPos;

        // Do rope cutting check
        RaycastHit2D[] hits = Physics2D.LinecastAll(_lastPos, worldPos);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Rope"))
            {
                hit.collider.GetComponent<Rope>().CutAtPoint(hit.point);
            }
        }

        _lastPos = worldPos;
    }

    private void OnDisable()
    {
        _clickAction.started -= OnClickStart;
        _clickAction.canceled -= OnClickEnd;
    }

    private void OnClickStart(InputAction.CallbackContext ctx)
    {
        _isSwiping = true;

        // Read current pointer position
        Vector2 screenPos = _pointAction.ReadValue<Vector2>();
        Vector3 worldPos = _cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));

        // Move trail immediately to finger start
        _trail.transform.position = worldPos;

        // Reset trail history so it doesn't connect with old swipe
        _trail.Clear();

        // Enable emitting
        _trail.emitting = true;

        // Reset last position for cutting checks
        _lastPos = worldPos;
    }
    
    private void OnClickEnd(InputAction.CallbackContext ctx)
    {
        _isSwiping = false;
        _trail.emitting = false; // stop drawing
    }
}