using System;
using UnityEngine;

public class ShadowPanel : MonoBehaviour
{
   public RectTransform ShadowPanelRect;
   public float ShadowSpeed = 30f;
   public float RatioX, RatioY;

   private float _shadowSpeedUp = 1f;

   private void Start()
   {
       if(IsTablet())
       {
           Debug.Log("Tablet or iPad");
           RatioX = 1.5f;
           RatioY = 0.8f;
       }
       else
       {
           Debug.Log("Phone");
           RatioX = 1.4f;
           RatioY = 0.3f;
       }

       ShadowPanelRect.sizeDelta = new Vector2(Screen.width * RatioX, Screen.height * RatioY);
   }

   private void Update()
   {
       float deltaAngle = Time.deltaTime * ShadowSpeed * _shadowSpeedUp;
       
       ShadowPanelRect.Rotate(0f, 0f, deltaAngle);
   }
   
   public static bool IsTablet()
   {
#if UNITY_IOS
    return UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
#elif UNITY_ANDROID
       float dpi = Screen.dpi;
       if (dpi == 0) dpi = 160f;

       float widthInches = Screen.width / dpi;
       float heightInches = Screen.height / dpi;
       float diagonal = Mathf.Sqrt(widthInches * widthInches + heightInches * heightInches);

       return diagonal >= 6.8f;
#else
    return false;
#endif
   }

}
