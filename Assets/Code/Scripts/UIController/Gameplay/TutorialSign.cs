using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialSign : MonoBehaviour
{
    [SerializeField] private TextMeshPro titleText;
    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    private float _fadeDuration = 1f;
    private float _showTime = 5f;
    private float _startWithDelay = 1.95f;

    public void SetContent(string title, Sprite bodySprite)
    {
        if (titleText)
        {
            titleText.text = title;
        }

        if (bodySpriteRenderer)
        {
            bodySpriteRenderer.sprite = bodySprite;
        }

        SetAlpha(0);
        StartCoroutine(StartWithDelay());
    }

    private IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(_startWithDelay);
        yield return Fade();
    }

    private IEnumerator Fade()
    {
        this.gameObject.SetActive(true);
        yield return LerpAlpha(0, 1, _fadeDuration); // Fade in
        yield return new WaitForSeconds(_showTime); // Wait
        yield return LerpAlpha(1, 0, _fadeDuration); // Fade out
        Destroy(gameObject);
    }

    private IEnumerator LerpAlpha(float a, float b, float t)
    {
        for (float time = 0; time < t; time += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(a, b, time / t);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(b);
    }

    private void SetAlpha(float a)
    {
        if (titleText)
        {
            titleText.color = new Color(titleText.color.r, titleText.color.g, titleText.color.b, a);
        }

        if (bodySpriteRenderer)
        {
            bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g,
                bodySpriteRenderer.color.b, a);
        }
    }
}