using System;
using System.Collections;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [Header("Loading UI")] 
    [SerializeField] private Image _leftPanel;
    [SerializeField] private Image _rightPanel;
    [SerializeField] private RectTransform _boxCutterRect;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private RectTransform _tagLeft;
    [SerializeField] private RectTransform _tagRight;
    
    // [SerializeField] private RectTransform _tagCuterByKnifeLeft;
    // [SerializeField] private RectTransform _tagCuterByKnifeRight;

    //Time animation
    private float _boxCutterAnimDuration = 1.2f;
    private float _boxCutterOffsetX = -519.2f;
    private float _durationLoadingCurtain = 0.75f;
    
    private MotionHandle _motionFirst, _motionSecond, _motionThird;
    private Coroutine _coroutine;

    private void Start()
    {
        InitLoadingUI();
    }

    private void InitLoadingUI()
    {
        _leftPanel.gameObject.SetActive(true);
        _rightPanel.gameObject.SetActive(true);
        _boxCutterRect.gameObject.SetActive(true);

        float width = _canvasRect.rect.width;

        _leftPanel.sprite = UserProfile.Instance.SelectedBoxData.BoxBGSprite;
        _rightPanel.sprite = UserProfile.Instance.SelectedBoxData.BoxBGSprite;

        // Set size
        _leftPanel.rectTransform.sizeDelta = new Vector2(width, _leftPanel.rectTransform.sizeDelta.y);
        _rightPanel.rectTransform.sizeDelta = new Vector2(width, _rightPanel.rectTransform.sizeDelta.y);

        // Set anchored position
        _leftPanel.rectTransform.anchoredPosition = new Vector2(-width / 2, 0);
        _rightPanel.rectTransform.anchoredPosition = new Vector2(-width / 2, 0);

        //Debug.Log("Width screen is: " + width);

        _coroutine =  StartCoroutine(StartLoadingSequence());
    }

    // private void InitCuterTagOffsets()
    // {
    //     _tagCuterByKnifeLeft.anchorMin = new Vector2(0, 0);
    //     _tagCuterByKnifeRight.anchorMax = new Vector2(1, 1);
    // }

    private IEnumerator StartLoadingSequence()
    {
        yield return StartCoroutine(PlayCutAnimation());
        yield return StartCoroutine(ShowLoadingCurtainCoroutine());
    }

    private IEnumerator PlayCutAnimation()
    {
        _boxCutterRect.gameObject.SetActive(true);

        float canvasHeight = _canvasRect.rect.height;
        float scissorsHeight = _boxCutterRect.rect.height;

        float startY = +scissorsHeight / 2;
        float endY = canvasHeight + scissorsHeight / 2;

        _boxCutterRect.anchoredPosition = new Vector2(_boxCutterOffsetX, startY);

        _motionFirst = LMotion.Create(_boxCutterRect.anchoredPosition, new Vector2(_boxCutterOffsetX, endY),
                _boxCutterAnimDuration)
            .WithEase(Ease.InOutSine)
            .BindToAnchoredPosition(_boxCutterRect);

        yield return new WaitForSeconds(_boxCutterAnimDuration);

        _boxCutterRect.gameObject.SetActive(false);
    }

    private IEnumerator ShowLoadingCurtainCoroutine()
    {
        float leftWidth = _leftPanel.rectTransform.rect.width;
        float rightWidth = _rightPanel.rectTransform.rect.width;

        _leftPanel.gameObject.SetActive(true);
        _rightPanel.gameObject.SetActive(true);

        StartCoroutine(ShrinkTagsCoroutine(_durationLoadingCurtain));
        //StartCoroutine(ShrinkHorizontalTagsCoroutine(_durationLoadingCurtain));

        // Animate slide panels
        _motionSecond = LMotion.Create(_leftPanel.rectTransform.anchoredPosition, new Vector2(-leftWidth, 0), _durationLoadingCurtain)
            .WithEase(Ease.InOutQuad)
            .BindToAnchoredPosition(_leftPanel.rectTransform);

        _motionThird = LMotion.Create(_rightPanel.rectTransform.anchoredPosition, new Vector2(rightWidth, 0),
                _durationLoadingCurtain * 2)
            .WithEase(Ease.InOutQuad)
            .BindToAnchoredPosition(_rightPanel.rectTransform);

        yield return new WaitForSeconds(_durationLoadingCurtain);

        _leftPanel.gameObject.SetActive(false);
        _rightPanel.gameObject.SetActive(false);
    }

    // private IEnumerator ShrinkHorizontalTagsCoroutine(float duration)
    // {
    //     float time = 0f;
    //
    //     Vector2 leftStartMin = _tagCuterByKnifeLeft.offsetMin;
    //     Vector2 rightStartMax = _tagCuterByKnifeRight.offsetMax;
    //
    //     float leftTargetX = leftStartMin.x - 100f;
    //     float rightTargetX = rightStartMax.x + 100f;
    //
    //     while (time < duration)
    //     {
    //         float t = time / duration;
    //         t = Mathf.Clamp01(t);
    //
    //         float leftX = Mathf.Lerp(leftStartMin.x, leftTargetX, t);
    //         float rightX = Mathf.Lerp(rightStartMax.x, rightTargetX, t);
    //
    //         _tagCuterByKnifeLeft.offsetMin = new Vector2(leftX, leftStartMin.y);
    //         _tagCuterByKnifeRight.offsetMax = new Vector2(rightX, rightStartMax.y);
    //
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     // Snap cuối cùng
    //     _tagCuterByKnifeLeft.offsetMin = new Vector2(leftTargetX, leftStartMin.y);
    //     _tagCuterByKnifeRight.offsetMax = new Vector2(rightTargetX, rightStartMax.y);
    // }


    private IEnumerator ShrinkTagsCoroutine(float duration)
    {
        float shrinkDuration = duration * 0.45f;
        float time = 0f;

        Vector2 leftStartMin = _tagLeft.offsetMin;
        Vector2 leftStartMax = _tagLeft.offsetMax;
        Vector2 rightStartMin = _tagRight.offsetMin;
        Vector2 rightStartMax = _tagRight.offsetMax;

        while (time < shrinkDuration)
        {
            float t = time / shrinkDuration;

            _tagLeft.offsetMin = Vector2.Lerp(leftStartMin, new Vector2(leftStartMin.x, 0), t);
            _tagLeft.offsetMax = Vector2.Lerp(leftStartMax, new Vector2(leftStartMax.x, 0), t);

            _tagRight.offsetMin = Vector2.Lerp(rightStartMin, new Vector2(rightStartMin.x, 0), t);
            _tagRight.offsetMax = Vector2.Lerp(rightStartMax, new Vector2(rightStartMax.x, 0), t);

            time += Time.deltaTime;
            yield return null;
        }

        // Snap 
        _tagLeft.offsetMin = new Vector2(leftStartMin.x, 0);
        _tagLeft.offsetMax = new Vector2(leftStartMax.x, 0);

        _tagRight.offsetMin = new Vector2(rightStartMin.x, 0);
        _tagRight.offsetMax = new Vector2(rightStartMax.x, 0);
    }

    private void OnDestroy()
    {
        CancelMotion(_motionFirst);
        CancelMotion(_motionSecond);
        CancelMotion(_motionThird);
        
        StopCoroutine(_coroutine);
        _coroutine = null;
    }
    
    private void CancelMotion(MotionHandle motion)
    {
        if (motion != null && motion.IsPlaying())
        {
            motion.Cancel();
        }
    }
}