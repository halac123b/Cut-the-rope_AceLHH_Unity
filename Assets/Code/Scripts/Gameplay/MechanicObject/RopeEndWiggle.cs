using System;
using System.Collections;
using LitMotion;
using UnityEngine;

public class RopeEndWiggle : MonoBehaviour
{
    private LineRenderer _ropeCut;
    private Vector3 _basePos;
    private SpringJoint2D springJoint;
    private Rigidbody2D _endRb;
    private Coroutine _coroutine;

    public void Init()
    {
        _ropeCut = GetComponent<LineRenderer>();
        
        FadeAndDestroy();
    }

    private void FadeAndDestroy()
    {
        if (transform != null)
        {
            _coroutine = StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = _ropeCut.startColor;
        Color endColor = _ropeCut.endColor;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / 1f);

            _ropeCut.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            _ropeCut.endColor = new Color(endColor.r, endColor.g, endColor.b, alpha);

            yield return null;
        }
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopCoroutine(_coroutine);
    }
}