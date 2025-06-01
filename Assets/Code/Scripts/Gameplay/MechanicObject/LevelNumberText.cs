using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

public class LevelNumberText : MonoBehaviour
{
    [SerializeField] private CanvasGroup _levelTextCanvasGroup;
    [SerializeField] private TMP_Text _levelNumberText;

    private MotionHandle _motionFirstAnim, _motionSecondAnim, _motionThirdAnim;

    private void Start()
    {
        string levelNumber = UserProfile.Instance.SelectedLevelIndex;

        if (levelNumber.Contains("_"))
        {
            levelNumber = levelNumber.Replace("_", "-");
        }

        _levelNumberText.text = levelNumber;

        _levelTextCanvasGroup.alpha = 0f;

        PlayLevelTextAnimation();
    }

    public void PlayLevelTextAnimation()
    {
        if (_levelTextCanvasGroup == null) return;

        _motionFirstAnim = LMotion.Create(0f, 1f, 0.3f).WithOnComplete(() =>
        {
            _motionSecondAnim = LMotion.Create(1f, 1f, 0.4f).WithOnComplete(() =>
            {
                _motionThirdAnim = LMotion.Create(1f, 0f, 0.3f).BindToAlpha(_levelTextCanvasGroup);
            }).BindToAlpha(_levelTextCanvasGroup);
        }).BindToAlpha(_levelTextCanvasGroup);
    }

    private void OnDestroy()
    {
        CancelMotion(_motionFirstAnim);
        CancelMotion(_motionSecondAnim);
        CancelMotion(_motionThirdAnim);
    }

    private void CancelMotion(MotionHandle motion)
    {
        if (motion != null && motion.IsPlaying())
        {
            motion.Cancel();
        }
    }
}