using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
   private void Start()
   {
      //gameObject.SetActive(false);
   }

   public void OnResumeClick()
   {
      Time.timeScale = 1;
      gameObject.SetActive(false);
   }
}