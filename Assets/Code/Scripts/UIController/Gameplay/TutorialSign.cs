using TMPro;
using UnityEngine;
using System.Collections;
using LitMotion;

public class TutorialSign : MonoBehaviour
{
    public TextMeshPro TitleText;
    [SerializeField] private SpriteRenderer _bodySpriteRenderer;
    public SpriteRenderer IconTutorialSign => _bodySpriteRenderer;
    private float _fadeDuration = 1f;
    private float _showTime = 5f;
    private float _startWithDelay = 1.95f;

    public void SetContent(string title, Sprite bodySprite, bool IsStartWithDelay = true)
    {
        if (TitleText)
            TitleText.text = title;

        if (_bodySpriteRenderer)
            _bodySpriteRenderer.sprite = bodySprite; 

        SetAlpha(0);
        StartCoroutine(StartWithDelay(IsStartWithDelay));
    }

    private IEnumerator StartWithDelay(bool isStartWithDelay = true)
    {
        if(isStartWithDelay)
            yield return new WaitForSeconds(_startWithDelay);
        
        yield return Fade();
    }

    private IEnumerator Fade()
    {
        gameObject.SetActive(true);

        // fade in
        yield return LMotion.Create(0f, 1f, _fadeDuration)
            .Bind(SetAlpha)
            .ToYieldInstruction();

        // wait
        yield return new WaitForSeconds(_showTime);

        // fade out
        yield return LMotion.Create(1f, 0f, _fadeDuration)
            .Bind(SetAlpha)
            .ToYieldInstruction();

        Destroy(gameObject);
    }

    private void SetAlpha(float a)
    {
        if (TitleText)
            TitleText.color = new Color(TitleText.color.r, TitleText.color.g, TitleText.color.b, a);

        if (_bodySpriteRenderer)
            _bodySpriteRenderer.color = new Color(_bodySpriteRenderer.color.r, _bodySpriteRenderer.color.g, _bodySpriteRenderer.color.b, a);
    }
}