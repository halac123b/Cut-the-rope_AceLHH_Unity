using System.Collections;
using UnityEngine;

public class AutoScaleBackground : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    public bool IsDelayAFrame;

    public Canvas Canvas
    {
        get => _canvas;
        set => _canvas = value;
    }

    private void Start()
    {
        if (IsDelayAFrame)
        {
            StartCoroutine(DelayedStartCoroutine());
        }
        else
        {
            UpdateBgSize();
        }
    }

    private IEnumerator DelayedStartCoroutine()
    {
        yield return null;
        UpdateBgSize();
    }

    private void UpdateBgSize()
    {
        var canvasSize = _canvas.GetComponent<RectTransform>().sizeDelta;
        var rect = GetComponent<RectTransform>();
        Vector2 bgSize = rect.sizeDelta;

        float ratio;

        if (bgSize.x < canvasSize.x)
        {
            ratio = canvasSize.x / bgSize.x + 0.02f;
        }
        else if (bgSize.y < canvasSize.y)
        {
            ratio = canvasSize.y / bgSize.y + 0.02f;
        }
        else
        {
            return;
        }

        rect.sizeDelta = new Vector2(bgSize.x * ratio, bgSize.y * ratio);
    }
}