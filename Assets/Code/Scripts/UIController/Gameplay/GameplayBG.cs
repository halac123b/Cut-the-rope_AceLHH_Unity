using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;


public class GameplayBG : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgImageUp;
    [SerializeField] private SpriteRenderer _bgImageTile;
    [SerializeField] private SpriteRenderer _bgImageDown;
    [SerializeField] private GameObject _bgPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _tileBGParent;
    private MotionHandle _motion;
    private List<Vector3> _positions;
    public float MaxBoundY => _bgImageUp.bounds.max.y;

    private void Awake()
    {
        _camera.transform.position = new Vector3(0, 0, -15f);
        _bgPrefab.transform.position = Vector3.zero;
    }

    private void Start()
    {
        SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay, UserProfile.Instance.SelectedBoxData.TiledBG,
            UserProfile.Instance.SelectedBoxData.AdjustPosition, UserProfile.Instance.ScrollLevelData);
    }

    private void SetBGImage(Sprite sprite, Sprite tileSprite, float posY, ScrollLevelData scrollLevelData)
    {
        _bgImageUp.sprite = sprite;
        _bgImageDown.sprite = sprite;
        _bgImageTile.sprite = tileSprite;
        _bgImageTile.transform.position = new Vector3(0f, -posY, 0f);

        if (scrollLevelData.IsScrollLevel)
        {
            LogTilePositions(scrollLevelData.ScrollAmount);
            MoveCameraToBottom((scrollLevelData.ScrollAmount + 1) * MaxBoundY);
            UserProfile.Instance.MaxPosY = ((scrollLevelData.ScrollAmount + 1) * MaxBoundY)/2;
        }
    }

    private void MoveCameraToBottom(float spriteTop)
    {
        float cameraSize = _camera.orthographicSize;

        _camera.transform.position = new Vector3(0f, spriteTop - cameraSize, -15f);

        if (transform != null)
        {
            _motion = LMotion.Create(spriteTop - cameraSize, 0f, 1f).WithOnComplete(() =>
            {
                UIController.Instance.IsCreatedLevel = true;
            }).BindToPositionY(_camera.transform);
        }
    }

    private void LogTilePositions(int count)
    {
        for (int x = 0; x < count; x++)
        {
            GameObject obj = ObjectSpawner.Instance?.SpawnWithAngle("BGScroll", Vector3.zero);
            obj?.GetComponent<GameplayBGScroll>().SetBG();
            Vector3 origin = new Vector3(0f, (x + 1) * MaxBoundY, 0f);
            obj.transform.position = origin;
        }
    }

    private void OnDisable()
    {
        if (_motion != null && _motion.IsActive())
        {
            _motion.Cancel();
        }
    }
}