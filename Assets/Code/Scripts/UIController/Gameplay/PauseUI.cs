using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
   [SerializeField] private Button _backBtn;
   [SerializeField] private Button _resumeBtn;

   private void Start()
   {
      _backBtn.onClick.AddListener(OnBackClick);
      _resumeBtn.onClick.AddListener(OnResumeClick);
   }

   public void OnResumeClick()
   {
      Time.timeScale = 1;
      gameObject.SetActive(false);
      UIController.Instance.SetUIStatus(false);
   }

   public void OnBackClick()
   {
      string levelIndex = UserProfile.Instance.SelectedLevelIndex;
      EventDispatcher.Instance.Dispatch(
         (Action<int>)(_ =>
         {
            UserProfile.Instance.SaveStars(levelIndex, 0);
         }),
         EventDispatcher.OnGetStarsRequest
      );

      Time.timeScale = 1;
      UIController.Instance.SetUIStatus(false);
      SceneManager.LoadScene("Home");
   }
}