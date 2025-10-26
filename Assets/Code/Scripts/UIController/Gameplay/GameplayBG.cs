using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameplayBG : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgImageUp;
    [SerializeField] private SpriteRenderer _bgImageTile;
    [SerializeField] private SpriteRenderer _bgImageDown;
    [SerializeField] private GameObject _bgPrefab;
    [SerializeField] private Camera _camera;
    private MotionHandle _motion;
    private List<Vector3> _positions;
    private float _boundY => _bgImageUp.bounds.max.y;
    private float _duration = 2f;

    private void Awake()
    {
        _camera.transform.position = new Vector3(0, 0, -15f);
        _bgPrefab.transform.position = Vector3.zero;
    }

    private void Start()
    {
        EventDispatcher.Instance.AddEvent(gameObject, _ => MoveCameraToCandy(), EventDispatcher.LoadScrollLevel);
        EventDispatcher.Instance.AddEvent(gameObject, _ => LoadBackgrondNextLevel(), EventDispatcher.LoadScrollNextLevel);
        SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay, UserProfile.Instance.SelectedBoxData.TiledBG,
            UserProfile.Instance.SelectedBoxData.AdjustPosition, UserProfile.Instance.ScrollLevelData);

        MoveCameraToCandy();
    }

    private void Update()
    {
        if (UIController.Instance.IsCreatedLevel || (!UserProfile.Instance.ScrollLevelData.IsScrollLevelHorizontal && !UserProfile.Instance.ScrollLevelData.IsScrollLevelVertical))
        {
            _camera.transform.position = new Vector3(0, 0, -15f);

            return;
        }

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            if (_motion.IsPlaying())
            {
                _motion.PlaybackSpeed = 4f;
            }
        }
    }

    private void LoadBackgrondNextLevel()
    {
        SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay, UserProfile.Instance.SelectedBoxData.TiledBG,
            UserProfile.Instance.SelectedBoxData.AdjustPosition, UserProfile.Instance.ScrollLevelData);
    }

    public void SetBGImage(Sprite sprite, Sprite tileSprite, float posY, ScrollLevelData scrollLevelData)
    {
        _bgImageUp.sprite = sprite;
        _bgImageDown.sprite = sprite;
        _bgImageTile.sprite = tileSprite;
        _bgImageTile.transform.position = new Vector3(0f, -posY, 0f);

        if (scrollLevelData.IsScrollLevelHorizontal || scrollLevelData.IsScrollLevelVertical)
        {
            LogTilePositions(scrollLevelData.ScrollAmount);
        }
    }

    private void MoveCameraToCandy()
    {
        if (!UserProfile.Instance.ScrollLevelData.IsScrollLevelHorizontal &&
            !UserProfile.Instance.ScrollLevelData.IsScrollLevelVertical)
        {
            return;
        }
        
        UIController.Instance.IsCreatedLevel = false;

        Vector3 frogPos = UserProfile.Instance.PosFrog;

        _camera.transform.position = new Vector3(frogPos.x, frogPos.y, -15f);

        if (_motion != null && _motion.IsActive())
        {
            _motion.Cancel();
        }

        if (UserProfile.Instance.ScrollLevelData.IsScrollLevelVertical)
        {
            _motion = LMotion.Create(frogPos.y, UserProfile.Instance.PosCandy.y, _duration).WithOnComplete(() =>
            {
                UIController.Instance.IsCreatedLevel = true;
            }).BindToPositionY(_camera.transform);
        }
        else if (UserProfile.Instance.ScrollLevelData.IsScrollLevelHorizontal)
        {
            _motion = LMotion.Create(frogPos.x, UserProfile.Instance.PosCandy.x, _duration).WithOnComplete(() =>
            {
                UIController.Instance.IsCreatedLevel = true;
            }).BindToPositionX(_camera.transform);
        }
    }

    private void LogTilePositions(int count)
    {
        for (int x = 0; x < count; x++)
        {
            GameObject obj = ObjectSpawner.Instance?.SpawnWithAngle("BGScroll", Vector3.zero);
            obj?.GetComponent<GameplayBGScroll>().SetBG();
            Vector3 origin = new Vector3(0f, (x + 1) * _boundY, 0f);
            obj.transform.position = origin;
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveEvent(gameObject);
        if (_motion != null && _motion.IsActive())
        {
            _motion.Cancel();
        }
    }
}