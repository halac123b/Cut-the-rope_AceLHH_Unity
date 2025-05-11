using System;
using UnityEngine;

public class ShadowPanel : MonoBehaviour
{
   public RectTransform ShadowPanelRect;
   public float ShadowSpeed = 30f; // degrees per second

   private float _shadowAngle = 0f;

   private void Update()
   {
      _shadowAngle += Time.deltaTime * ShadowSpeed;
      ShadowPanelRect.localEulerAngles = new Vector3(0f, 0f, _shadowAngle);
   }
}
