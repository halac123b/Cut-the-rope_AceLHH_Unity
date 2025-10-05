using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;


public class GameplayBG : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgImageUp;
    [SerializeField] private SpriteRenderer _bgImageTile;
    [SerializeField] private SpriteRenderer _bgImageDown;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _tileBGParent;
    private Vector2 _size;
    private MotionHandle _motion;
    private List<Vector3> _positions = new();

    private void Awake()
    {
        transform.position = _camera.transform.position + _camera.transform.forward * (_camera.farClipPlane - 0.01f);
        transform.forward = _camera.transform.forward;
        transform.eulerAngles = new Vector3(0, 0, -180f);

        float cameraWidth = _camera.orthographicSize * 2;
        float cameraHeight = cameraWidth * _camera.aspect;

        Vector2 size = new Vector2(cameraWidth, cameraHeight);
        _size = size;

        _camera.transform.position = new Vector3(0, 0, -10);
    }

    private void Start()
    {
        SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay, UserProfile.Instance.SelectedBoxData.TiledBG,
            UserProfile.Instance.SelectedBoxData.AdjustPosition, UserProfile.Instance.ScrollLevelData);
    }

    private void SetBGImage(Sprite sprite, Sprite tileSprite, float posY, ScrollLevelData scrollLevelData)
    {
        _size.y *= scrollLevelData.IsScrollLevel ? scrollLevelData.ScrollAmount : 1f;

        Debug.LogError($"Size {_size.y}");
        _bgImageUp.sprite = sprite;
        _bgImageDown.sprite = sprite;
        _bgImageTile.sprite = tileSprite;
        _bgImageTile.transform.position = new Vector3(0f, -posY, 0f);
        // _bgImage.size = _size;

        if (scrollLevelData.IsScrollLevel)
        {
            // MoveCameraToBottom(_bgImage);

            LogTilePositions(_bgImageUp);
        }
    }

    private void MoveCameraToBottom(SpriteRenderer sr)
    {
        float spriteTop = sr.bounds.max.y;
        float spriteBottom = sr.bounds.min.y;
        float cameraSize = _camera.orthographicSize;

        _camera.transform.position = new Vector3(0f, spriteTop - cameraSize, -10f);

        if (transform != null)
        {
            _motion = LMotion.Create(spriteTop - cameraSize, spriteBottom + cameraSize, 1f).WithOnComplete(() =>
            {
                UIController.Instance.IsCreatedLevel = true;
            }).BindToPositionY(_camera.transform);
        }
    }

    private void LogTilePositions(SpriteRenderer sr)
    {
        Debug.LogError($"Tile positions 1");

        Vector2 tileSize = sr.sprite.bounds.size;
        tileSize *= sr.transform.lossyScale;

        Debug.LogError($"Tile positions {tileSize}");

        int tileCountY = Mathf.RoundToInt(sr.size.y / _camera.orthographicSize);
        Debug.LogError($"Tile count {sr.size.y} - {_camera.orthographicSize} {tileCountY}");
        Bounds fullBounds = sr.bounds;

        Vector3 origin = new Vector3(
            (fullBounds.min.x + fullBounds.max.x) * 0.5f,
            fullBounds.min.y,
            fullBounds.center.z
        );

        origin = new Vector3(0f, origin.y, 0f);

        for (int y = 0; y < tileCountY; y++)
        {
            Vector3 tileCenter = origin +
                                 new Vector3(0f,
                                     (y + 0.25f) * tileSize.y,
                                     0f);
            Debug.LogError($"Tile positions for {y} - {tileCenter}");
            _positions.Add(tileCenter);
        }

        for (int x = 0; x < _positions.Count; x++)
        {
            GameObject obj = ObjectSpawner.Instance?.Spawn("Tile", _tileBGParent, new Vector3(0, 0, -180f),
                _positions[x]); // - new Vector3(0f, -1.2f, 0f));
            Debug.LogError($"Tile positions for {obj.name}");
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();

            sprite.sprite = UserProfile.Instance?.SelectedBoxData.TiledBG;
            // sprite.size = new Vector2(_size.x, _size.x / 2);
        }
    }

    private void OnDisable()
    {
        if (_motion != null && _motion.IsActive()) _motion.Cancel();
    }
}