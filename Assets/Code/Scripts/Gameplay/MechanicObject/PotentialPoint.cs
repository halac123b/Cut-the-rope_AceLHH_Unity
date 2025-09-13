using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class PotentialPoint : MonoBehaviour
{
    public int RopeLength;
    public float Scale = 0.25f;
    [SerializeField] private GameObject _ropePrefab;
    [SerializeField] private CircleCollider2D _circleCollider;
    private float _defaultScale = 0.25f;
    private float _numberAdjustScale;
    private MotionHandle _motion;

    private void Start()
    {
        if (Scale < 1f)
        {
            Scale = 1f;
        }

        _numberAdjustScale = Scale <= 1f ? 0.05f : 0.1f;
        Scale *= _defaultScale - _numberAdjustScale;

        _circleCollider.transform.localScale = new Vector3(Scale, Scale, Scale);
        _circleCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Candy"))
        {
            Candy candy = collision.GetComponent<Candy>();
            CreateRope(candy);
        }
    }

    private void CreateRope(Candy candy)
    {
        GameObject createdObj = Instantiate(_ropePrefab, Vector3.zero, Quaternion.identity);
        Rope rope = createdObj.GetComponent<Rope>();

        rope.RopeFirstObject = transform;
        rope.RopeSecondObject = candy.transform;
        rope.RopeLength = RopeLength;

        EventDispatcher.Instance.Dispatch(rope.gameObject, EventDispatcher.AddToListObjsLevel);
        FadeOutCircle();
    }

    private void FadeOutCircle()
    {
        SpriteRenderer spriteRenderer = _circleCollider.GetComponent<SpriteRenderer>();
        _motion = LMotion.Create(1f, 0f, 0.3f).WithOnComplete(() => { _circleCollider.enabled = false; })
            .BindToColorA(spriteRenderer);
    }
}