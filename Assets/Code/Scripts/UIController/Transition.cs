using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class Transition : MonoSingleton<Transition>
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Canvas _canvas;
    private MotionHandle _motionShow, _motionHide;

    private void Start()
    {
        ResetTransition();
    }

    public void Appear(Action callback)
    {
        _canvas.enabled = true;

        if (transform != null)
        {
            _motionShow = LMotion.Create(_canvasGroup.alpha, 1f, 0.2f).WithOnComplete((() =>
                {
                    callback?.Invoke();
                    
                    _motionHide = LMotion.Create(_canvasGroup.alpha, 0f, 0.2f)
                        .WithOnComplete(() =>
                        {
                            ResetTransition();
                        })
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