using System;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoxUIComponent : MonoBehaviour
{
    [SerializeField] private Button _btnBox;
    [HideInInspector] public BoxData MyBoxData;
    [FormerlySerializedAs("_boxSprite")] public Image BoxSprite;
    [SerializeField] private TMP_Text _boxName;
    public RectMask2D FrogMask;
    public bool IsPlayAnimation;
    public GameObject LockPart;
    public TMP_Text RequireStarAmount;

    private MotionHandle _motionFirstScale;

    private void Start()
    {
        _btnBox.onClick.AddListener(() => OnBoxButtonClicked());
        CheckUnlockPart();
    }

    private void OnBoxButtonClicked()
    {
        if (LockPart.activeSelf)
        {
            return;
        }
        
        UserProfile.Instance.SetBoxData(MyBoxData);
        Transition.Instance.Appear(() =>
        {
            EventDispatcher.Instance.Dispatch(MyBoxData, EventDispatcher.LoadLevelUI);
        });
    }

    public void SetUIBoxComponent()
    {
        BoxSprite.sprite = MyBoxData.BoxSprite;
        _boxName.text = MyBoxData.Index.ToString() + ". " + MyBoxData.BoxName;
    }

    private void OnDestroy()
    {
        CancelMotion(_motionFirstScale);
        
        _btnBox.onClick.RemoveListener(() => OnBoxButtonClicked());
        EventDispatcher.Instance.RemoveEvent(gameObject);
    }

    public void PlayBoxAnimation()
    {
        if (transform != null)
        {
            if (IsPlayAnimation) return;
            
            IsPlayAnimation = true;

            _motionFirstScale = LSequence.Create().Append(LMotion.Create(1f, 0.85f, 0.15f)
                    .BindToLocalScaleX(transform))
                .Join(LMotion.Create(1f, 1.1f, 0.15f).BindToLocalScaleY(transform))
                .Append(LMotion.Create(0.85f, 1.08f, 0.3f).BindToLocalScaleX(transform))
                .Join(LMotion.Create(1.1f, 1f, 0.3f).BindToLocalScaleY(transform))
                .Append(LMotion.Create(1.08f, 1f, 0.2f).BindToLocalScaleX(transform))
                .Run();
        }
    }

    private void CheckUnlockPart()
    {
        RequireStarAmount.text = MyBoxData.RequireStar.ToString();
        
        if (UserProfile.Instance.GetAllStars() >= MyBoxData.RequireStar)
        {
            LockPart.SetActive(false);
        }
        else
        {
            LockPart.SetActive(true);
        }
    }

    private void CancelMotion(MotionHandle motion)
    {
        if (motion != null && motion.IsPlaying())
        {
            motion.Cancel();
        }
    }
}