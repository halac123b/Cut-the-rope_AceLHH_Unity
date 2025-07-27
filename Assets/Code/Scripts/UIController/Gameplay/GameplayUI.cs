using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
   [SerializeField] private Image _bgImage;

   private void OnEnable()
   {
      SetBGImage(UserProfile.Instance.SelectedBoxData.BGGameplay);
   }

   public void SetBGImage(Sprite sprite)
   {
      _bgImage.sprite = sprite;
   }
}
