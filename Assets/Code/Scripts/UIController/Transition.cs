using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoSingleton<Transition>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _imgTransition;
    private MotionHandle _motionShow, _motionHide;

    private void Start()
    {
        ResetTransition();
    }

    public void Appear( Color color, Action callback, float duration = 0.2f)
    {
        _canvas.enabled = true;
        _imgTransition.color = color;

        if (transform != null)
        {
            _motionShow = LMotion.Create(_canvasGroup.alpha, 1f, duration).WithOnComplete((() =>
                {
                    callback?.Invoke();

                    _motionHide = LMotion.Create(_canvasGroup.alpha, 0f, duration)
                        .WithOnComplete(() => { ResetTransition(); })
                        .BindToAlpha(_canvasGroup);
                }))
                .BindToAlpha(_canvasGroup);
        }
    }

    private void ResetTransition()
    {
        _canvas.enabled = false;
        _canvasGroup.alpha = 0f;
    }

    private void OnDestroy()
    {
        if (_motionShow.IsPlaying())
        {
            _motionShow.Cancel();
        }

        if (_motionHide.IsPlaying())
        {
            _motionHide.Cancel();
        }
    }
}