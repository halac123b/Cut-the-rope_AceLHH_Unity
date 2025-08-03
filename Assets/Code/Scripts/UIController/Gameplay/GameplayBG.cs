using UnityEngine;

public class GameplayBG : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgImage;

    private void Awake()
    {
        Camera camera = Camera.main;
        transform.position = camera.transform.position + camera.transform.forward * (camera.farClipPlane - 0.01f);
        transform.forward = camera.transform.forward;
        transform.eulerAngles = new Vector3(0, 0, 90f);
        
        float cameraWidth = camera.orthographicSize * 2;
        float cameraHeight = cameraWidth * camera.aspect;

        Vector2 size = new Vector2(cameraWidth , cameraHeight);
        
        _bgImage.size = size;
    }

    private void OnEnable()
    {
        SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay);
    }

    private void SetBGImage(Sprite sprite)
    {
        _bgImage.sprite = sprite;
    }
}