using System;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class StarEffect : MonoBehaviour
{
    private MotionHandle _motion;

    private void Start()
    {
        StarAnimation();
    }

    private void StarAnimation()
    {
        if (transform != null)
        {
            _motion = LMotion.Create(0f, 0.1f, 1.15f) // Animate from 0f to 10f over 2 seconds
                .WithEase(Ease.OutQuad) // Specify easing function
                .WithLoops(-1, LoopType.Yoyo) // Specify loop count and type
                .WithDelay(0.2f) // Set delay
                .BindToPositionY(transform);
        }
    }

    private void OnDestroy()
    {
        if (_motion.IsPlaying())
        {
            _motion.Cancel();
        }
    }
}