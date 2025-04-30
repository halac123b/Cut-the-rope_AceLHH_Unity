using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
   public void OnResumeClick()
   {
      Time.timeScale = 1;
      gameObject.SetActive(false);
   }

   public void OnBackClick()
   {
      EventDispatcher.Instance.Dispatch(gameObject, EventDispatcher.ResetMap);
      SceneManager.LoadScene("Home");
   }
}