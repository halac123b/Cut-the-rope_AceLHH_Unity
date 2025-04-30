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
   }

   public void OnBackClick()
   {
      StarController.SetStartLevel();
      Time.timeScale = 1;
      SceneManager.LoadScene("Home");
   }
}