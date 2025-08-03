using System;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _bgImage;

    public void Awake()
    {
        var camera = Camera.main;
        transform.position = camera.transform.position + camera.transform.forward * (camera.farClipPlane - 0.01f);
        transform.forward = camera.transform.forward;
        transform.eulerAngles = new Vector3(0, 0, 90f);
        
        var cameraWidth = camera.orthographicSize * 2;
        var cameraHeight = cameraWidth * camera.aspect;

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