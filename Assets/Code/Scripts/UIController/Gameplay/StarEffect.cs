using System;
using System.Collections;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class StarEffect : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _animatorStar;
    private MotionHandle _motion, _motionDistance;
    public bool DisappearOnTrigger;
    public float DistanceMove;

    private void Start()
    {
        StarAnimation();

        _animatorStar.enabled = DisappearOnTrigger;
    }

    private void StarAnimation()
    {
        if (transform != null)
        {
            _motion = LMotion
                .Create(transform.position.y, transform.position.y + 0.05f,
                    1.15f) // Animate from 0f to 10f over 2 seconds
                .WithEase(Ease.OutQuad) // Specify easing function
                .WithLoops(-1, LoopType.Yoyo) // Specify loop count and type
                .WithDelay(0.2f) // Set delay
                .BindToPositionY(transform);

            if (DistanceMove != 0f)
            {
                _motionDistance = LMotion.Create(transform.position.x, transform.position.x + DistanceMove, 1.5f)
                    .WithEase(Ease.Linear)
                    .WithLoops(-1, LoopType.Yoyo)
                    .WithDelay(0.1f)
                    .BindToPositionX(transform);
            }
        }
    }

    public void PlayDisappearAnimation()
    {
        _animator.SetTrigger("collect");
    }

    private void PlayLightDisappearAnimation()
    {
        _animatorStar.SetTrigger("star_disappear");
    }

    public void TriggerDisappear(float delay = 2f)
    {
        if (DisappearOnTrigger)
        {
            StartCoroutine(PlayDisappearCoroutine(delay));
        }
    }

    private IEnumerator PlayDisappearCoroutine(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        PlayLightDisappearAnimation();
        yield return new WaitForSeconds(3.1f);
        DestroyStar();
    }

    private void DestroyStar()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_motion.IsPlaying())
        {
            _motion.Cancel();
        }

        if (_motionDistance.IsPlaying())
        {
            _motionDistance.Cancel();
        }
    }
}